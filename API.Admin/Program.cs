using API.Admin.ExceptionHandler;
using API.Admin.Extensions;
using API.Admin.MappingProfileCls;
using AutoMapper;
using EducationalManagement.ServiceDefaults;
using Identity;
using Microsoft.AspNetCore.Mvc;
using Utilities.Middleware;
using Utilities.RequestHandler;
using Utilities.Settings;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.Timeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
}).CreateMapper());

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<List<TenantConfig>>(builder.Configuration.GetSection("Tenants"));


// Add custom services
builder.Services.ConfigureCors();
builder.Services.ConfigureSingletonServices();
builder.Services.AddSingleton<CryptoService>(); // Register CryptoService
builder.Services.AddHttpContextAccessor(); // Required for MultiTenantHangfireFilter
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureScopedServices();

builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureRequestsLimit();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddRequestTimeouts();

builder.Services.AddApplicationInsightsTelemetry();

WebApplication app = builder.Build();


app.MapDefaultEndpoints();
app.UseHttpsRedirection();
app.ConfigureStaticFiles();
app.UseFileServer();

app.UseRouting();

app.UseCors();

app.UseResponseCaching();


// Configure custom pipeline.
app.ConfigureSwagger();
app.UseMiddleware<CultureMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestMiddleware>();
app.UseMiddleware<HeaderMiddleware>();

app.UseRequestTimeouts();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();

});

app.Run();
