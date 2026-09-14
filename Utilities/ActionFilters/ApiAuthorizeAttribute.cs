using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Utilities.Constants;
using Utilities.Extensions;
using Utilities.Settings;

namespace Utilities.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            int? tenantId = (int?)context.HttpContext.Items[ApiConstants.TenantId];
            if (!(tenantId > 0))
            {
                context.Result = new BadRequestResult();
                return;
            }

#if !DEBUG

            string appKey = context.HttpContext.Request.Headers[HeadersConstants.AppKey];
            
                IServiceProvider services = context.HttpContext.RequestServices;
                AppSettings _appSettings = services.GetService<IOptions<AppSettings>>().Value;
            
                if (appKey.IsEmpty())
                {
                    context.Result = new BadRequestResult();
                    return;
                }
                else if (appKey != _appSettings.AppKey)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
#endif


            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            int? userId = (int?)context.HttpContext.Items[ApiConstants.UserId];
            if (!(userId > 0))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
