using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Extensions
{
    public static class UserPermissionExtension
    {
        /// <summary>
        /// Checks if the current user has one or more permissions (as enums).
        /// </summary>
        public static bool HasPermission(this ClaimsPrincipal user, IEnumerable<RoleFunctionalityEnum> permissions, bool requireAll = false)
        {
            var httpContext = user?.Identity?.IsAuthenticated == true
                ? new HttpContextAccessor().HttpContext
                : null;

            if (httpContext.IsNull())
                return false;

            var cache = httpContext.RequestServices.GetService<IMemoryCache>();
            if (cache.IsNull())
                return false;

            var cacheKey = user.FindFirst(PortalConstants.PermissionCacheKey)?.Value;
            if (cacheKey.IsNull())
                return false;

            if (!cache.TryGetValue(cacheKey, out HashSet<string>? userPermissions) || userPermissions == null)
                return false;

            var requiredPermissions = permissions
                .Select(p => p.ToString())
                .ToList();

            return requireAll
                ? requiredPermissions.All(p => userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase))
                : requiredPermissions.Any(p => userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase));


        }

        /// <summary>
        /// Overload: check single permission only.
        /// </summary>
        public static bool HasPermission(this ClaimsPrincipal user, RoleFunctionalityEnum permission)
        {
            return HasPermission(user, new[] { permission });
        }
    }
}
