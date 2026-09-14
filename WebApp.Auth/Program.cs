using ApiHub.Services;
using EducationalManagement.ServiceDefaults;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Converters;
using WebApp.Auth.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
builder.Services.AddMemoryCache(); // Add MemoryCache for caching

builder.Services.AddSingleton<ApiHubService>(); // Ensure ApiHubService is registered
builder.Services.AddHttpClient<ApiBase>();      // Register ApiBase with HttpClientFactory

builder.Services.AddScoped<HRTranslationService>();
builder.Services.AddScoped<IStringLocalizer, HRLocalizationService>();


WebApplication app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area=HR}/{controller=Login}/{action=Index}/{id?}"
    ).WithStaticAssets();
});

app.Run();
