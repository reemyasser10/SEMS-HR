namespace HR.Infrastructure.Identity;

using HR.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public int? UserId
    {
        get
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User?.FindFirst("sub")?.Value;

            if (!string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var id))
            {
                return id;
            }

            if (_httpContextAccessor.HttpContext?.Items.TryGetValue("UserId", out var itemVal) == true)
            {
                if (itemVal is int itemInt && itemInt > 0)
                {
                    return itemInt;
                }
            }

            return null;
        }
    }

    public string? UserName =>
        User?.FindFirst(ClaimTypes.Name)?.Value
        ?? User?.FindFirst("name")?.Value
        ?? User?.Identity?.Name;

    public string? DisplayName =>
        User?.FindFirst("displayName")?.Value
        ?? UserName;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();

    public IEnumerable<string> Permissions =>
        User?.FindAll("Permission").Select(c => c.Value) ?? Enumerable.Empty<string>();

    public bool HasPermission(string permission) =>
        Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
}
