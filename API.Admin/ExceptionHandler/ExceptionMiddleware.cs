using System.Net;
using Utilities.Constants;
using Utilities.ResponseHandler;

namespace API.Admin.ExceptionHandler
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                using IServiceScope scope = _scopeFactory.CreateScope();
                ExceptionUtil exceptionUtil = scope.ServiceProvider.GetRequiredService<ExceptionUtil>();

                int? tenantId = (int?)context.Items[ApiConstants.TenantId];
                string? culture = (string?)context.Items[ApiConstants.Culture];

                ResponseStatus details = await exceptionUtil.Error(tenantId, culture, ex);
                context.Response.Headers[HeadersConstants.Status] = details.ToString();
            }
        }
    }

}
