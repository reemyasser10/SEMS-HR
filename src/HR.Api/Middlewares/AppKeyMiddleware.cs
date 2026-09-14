namespace HR.Api.Middlewares;

public class AppKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string AppKeyHeaderName = "app-key";

    public AppKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        var expectedAppKey = configuration["AppSettings:appKey"];

        if (string.IsNullOrEmpty(expectedAppKey))
        {
            // If the key isn't configured, we might want to throw an exception in a real app, 
            // but we'll let it pass or fail securely here.
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(AppKeyHeaderName, out var extractedAppKey))
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = "app-key header is missing" });
            return;
        }

        if (!expectedAppKey.Equals(extractedAppKey))
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = "Invalid app-key" });
            return;
        }

        await _next(context);
    }
}
