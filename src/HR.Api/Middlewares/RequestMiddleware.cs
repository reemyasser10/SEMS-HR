namespace HR.Api.Middlewares;

using HR.Api.Utilities;
using HR.Application.Common.Interfaces;
using Microsoft.Extensions.Primitives;

public class RequestMiddleware
{
    private readonly RequestDelegate _next;

    public RequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IJwtService jwtUtils)
    {
        if (context.Request.Headers.TryGetValue(HeadersConstants.AuthorizationToken, out StringValues tokenValue))
        {
            // Usually Bearer <token>
            var tokenStr = tokenValue.ToString().Replace("Bearer ", "");
            int? userId = jwtUtils.ValidateJwtToken(tokenStr);
            if (userId != null)
            {
                context.Items[ApiConstants.UserId] = userId;
            }
        }
        else
        {
            if (context.Request.Headers.TryGetValue(HeadersConstants.UserId, out var userIdValue) ||
                context.Request.Query.TryGetValue(HeadersConstants.UserId, out userIdValue))
            {
                if (int.TryParse(userIdValue, out int userId))
                {
                    context.Items[ApiConstants.UserId] = userId;
                }
            }
            else
            {
                context.Items[ApiConstants.UserId] = -1;
            }
        }

        if (context.Request.Headers.TryGetValue(HeadersConstants.DeviceId, out StringValues deviceIdValue))
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
