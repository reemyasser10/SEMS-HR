using Microsoft.AspNetCore.Http;
using Utilities.Constants;

namespace Identity
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtUtil jwtUtils)
        {
            if (context.Request.Headers.TryGetValue(HeadersConstants.AuthorizationToken, out Microsoft.Extensions.Primitives.StringValues tokenValue))
            {
                var jwtToken = jwtUtils.ValidateJwtToken(tokenValue);
                if (jwtToken != null)
                {
                    // Update principal with token claims
                    var identity = new System.Security.Claims.ClaimsIdentity(jwtToken.Claims, "jwt");
                    context.User = new System.Security.Claims.ClaimsPrincipal(identity);

                    // Extract user ID as before for downstream compatibility
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        context.Items[ApiConstants.UserId] = userId;
                    }
                }
            }
            else
            {
                context.Items[ApiConstants.UserId] = -1;
            }

            if (context.Request.Headers.TryGetValue(HeadersConstants.TenantId, out Microsoft.Extensions.Primitives.StringValues tenantIdValue))
            {
                if (int.TryParse(tenantIdValue, out int tenantId))
                {
                    context.Items[ApiConstants.TenantId] = tenantId;
                }
            }
            else
            {
                context.Items[ApiConstants.TenantId] = -1;
            }


            if (context.Request.Headers.TryGetValue(HeadersConstants.DeviceId, out Microsoft.Extensions.Primitives.StringValues deviceIdValue))
            {
                if (int.TryParse(deviceIdValue, out int deviceId))
                {
                    context.Items[ApiConstants.DeviceId] = deviceId;
                }
            }
            else
            {
                context.Items[ApiConstants.DeviceId] = -1;
            }

            await _next(context);
        }
    }
}
