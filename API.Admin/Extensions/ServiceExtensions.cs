using API.Admin.ExceptionHandler;
using AspNetCoreRateLimit;
using Data;
using Identity;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Utilities.ActionFilters;
using Utilities.Constants;
using Utilities.EmailServices;

namespace API.Admin.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {

            _ = services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    _ = builder.SetIsOriginAllowed(a => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .WithExposedHeaders(HeadersConstants.Status,
                                               HeadersConstants.Pagination,
                                               HeadersConstants.Authorization,
                                               HeadersConstants.Expires,
                                               HeadersConstants.SetRefresh);
                });
            });
        }

        public static void ConfigureSingletonServices(this IServiceCollection services)
        {
            _ = services.AddSingleton<JwtUtil>();
        }

        public static void ConfigureScopedServices(this IServiceCollection services)
        {
            _ = services.AddScoped<ExceptionUtil>();


        }
    
        public static void ConfigureDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            _ = services.AddScoped<DbContextFactory>();
            _ = services.AddScoped<TenantConnectionResolver>();

            var x = configuration.GetConnectionString("DefaultConnection");
            _ = services.AddScoped<MigrationDbContext>((provider) =>
            {
                return new MigrationDbContext(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            _ = services.AddResponseCaching();
        }
        public static void ConfigureEmailSender(this IServiceCollection services)
        {
            _ = services.AddScoped<EmailSender>();
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            _ = services.AddSwaggerGen(c =>
            {
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("hh:mm:ss")
                });

                c.MapType<DateTime>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("yyyy-MM-ddThh:mm:ss")
                });

                c.SwaggerDoc("Authentication", new OpenApiInfo { Title = "Authentication" });
                c.SwaggerDoc("Tenant", new OpenApiInfo { Title = "Tenant" });
                c.SwaggerDoc("Common", new OpenApiInfo { Title = "Common" });
              

                c.OperationFilter<SwaggerOperations>();

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
       
        public static void ConfigureRequestsLimit(this IServiceCollection services)
        {
            // needed to store rate limit counters and ip rules
            _ = services.AddMemoryCache();

            _ = services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.GeneralRules =
                [
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 100,
                    }
                ];
            });

            _ = services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            _ = services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            _ = services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            _ = services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            _ = services.AddInMemoryRateLimiting();
        }
    }
}
