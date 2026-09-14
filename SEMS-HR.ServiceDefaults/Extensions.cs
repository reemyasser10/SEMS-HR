using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Polly;

namespace EducationalManagement.ServiceDefaults;

// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This project should be referenced by each service project in your solution.
// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
public static class ResilienceKeys
{
    public static readonly ResiliencePropertyKey<TimeSpan> TotalTimeout = new("TotalTimeout");
    public static readonly ResiliencePropertyKey<TimeSpan> AttemptTimeout = new("AttemptTimeout");
}

public static class Extensions
{
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        _ = builder.ConfigureOpenTelemetry();

        _ = builder.AddDefaultHealthChecks();

        _ = builder.Services.AddServiceDiscovery();

        _ = builder.Services.ConfigureHttpClientDefaults(http =>
        {
            if (builder.Environment.IsDevelopment())
            {
                _ = http.AddStandardResilienceHandler(options =>
                {
                    options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(5);
                    options.AttemptTimeout.Timeout = TimeSpan.FromMinutes(5);

                    options.Retry.MaxRetryAttempts = 1;

                    // 2. Neutralize Circuit Breaker so it doesn't interfere with debugging
                    options.CircuitBreaker.FailureRatio = 0.99;
                    options.CircuitBreaker.MinimumThroughput = 1000;
                    options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(10);

                    //For custom time out endpoints 
                    // 2. Dynamic Total Timeout Override
                    //options.TotalRequestTimeout.TimeoutGenerator = args =>
                    //{
                    //    if (args.Context.Properties.TryGetValue(ResilienceKeys.TotalTimeout, out var custom))
                    //        return ValueTask.FromResult(custom);
                    //    return ValueTask.FromResult(options.TotalRequestTimeout.Timeout);
                    //};

                    //// 3. Dynamic Attempt Timeout Override
                    //options.AttemptTimeout.TimeoutGenerator = args =>
                    //{
                    //    if (args.Context.Properties.TryGetValue(ResilienceKeys.AttemptTimeout, out var custom))
                    //        return ValueTask.FromResult(custom);
                    //    return ValueTask.FromResult(options.AttemptTimeout.Timeout);
                    //};

                });
            }
            else // Production / Live
            {
                // Turn on resilience by default
                //_ = http.AddStandardResilienceHandler();
                _ = http.AddStandardResilienceHandler(options =>
                {
                    //For custom time out endpoints 
                    //options.TotalRequestTimeout.TimeoutGenerator = args =>
                    //{
                    //    if (args.Context.Properties.TryGetValue(ResilienceKeys.TotalTimeout, out var custom))
                    //        return ValueTask.FromResult(custom);
                    //    return ValueTask.FromResult(options.TotalRequestTimeout.Timeout);
                    //};

                    //options.AttemptTimeout.TimeoutGenerator = args =>
                    //{
                    //    if (args.Context.Properties.TryGetValue(ResilienceKeys.AttemptTimeout, out var custom))
                    //        return ValueTask.FromResult(custom);
                    //    return ValueTask.FromResult(options.AttemptTimeout.Timeout);
                    //};
                });
            }


            // Turn on service discovery by default
            _ = http.AddServiceDiscovery();
        });

        // Uncomment the following to restrict the allowed schemes for service discovery.
        // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
        // {
        //     options.AllowedSchemes = ["https"];
        // });

        return builder;
    }

    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        _ = builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        _ = builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                _ = metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                _ = tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation()
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        _ = builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        bool useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            _ = builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        //{
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        //}

        return builder;
    }

    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        _ = builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            _ = app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            _ = app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }

        return app;
    }

}

public static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage SetCustomTimeouts(this HttpRequestMessage request, TimeSpan total, TimeSpan attempt)
    {
        // 1. Get or create the ResilienceContext from the request
        var context = request.GetResilienceContext() ?? ResilienceContextPool.Shared.Get();

        // 2. Set the properties on the context
        context.Properties.Set(ResilienceKeys.TotalTimeout, total);
        context.Properties.Set(ResilienceKeys.AttemptTimeout, attempt);

        // 3. Ensure the request is carrying this context
        request.SetResilienceContext(context);

        return request;
    }
}
