using Microsoft.AspNetCore.Http;
using Utilities.Constants;
using Utilities.ResponseHandler;

namespace Utilities.Middleware
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Append(HeadersConstants.Status, new ResponseStatus(true).ToString());

            context.Response.Headers.Append(HeadersConstants.ContentSecurityPolicy, "upgrade-insecure-requests");
            context.Response.Headers.Append(HeadersConstants.AccessControlAllowOrigin, "*");

            await _next.Invoke(context);
        }
    }
}
