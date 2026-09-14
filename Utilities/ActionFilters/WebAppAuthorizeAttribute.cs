using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Entities.Enums;
using Utilities.Extensions;
using Utilities.Constants;
using System.Net;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Utilities.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class WebAppAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly RoleFunctionalityEnum[]? _permissionsEnum;
        private readonly bool _requireAll;


        public WebAppAuthorizeAttribute(params RoleFunctionalityEnum[] permissions)
            : this(requireAll: false, permissions: permissions)
        {
        }

        public WebAppAuthorizeAttribute(bool requireAll, params RoleFunctionalityEnum[] permissions)
        {
            _permissionsEnum = permissions;
            _requireAll = requireAll;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check if the access token has expired
            var expiryClaim = user.FindFirst(PortalConstants.AccessTokenExpires);
            if (expiryClaim != null && DateTime.TryParse(expiryClaim.Value, out DateTime expires) &&
                expires < DateTime.UtcNow)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool ok = user.HasPermission(_permissionsEnum, _requireAll);

            if (!ok)
            {
                // If the user is authenticated but HasPermission failed, 
                // it might be because the permission cache has expired or is missing.
                var cacheKey = user.FindFirst(PortalConstants.PermissionCacheKey)?.Value;
                if (string.IsNullOrEmpty(cacheKey))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
                if (cache == null || !cache.TryGetValue(cacheKey, out _))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}
