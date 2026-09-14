namespace HR.Application.Admin;

using HR.Application.Common.Models;

public interface IRoleService
{
    Task<int> CreateRoleAsync(CreateRoleDto request, CancellationToken cancellationToken = default);
    Task UpdateRoleAsync(UpdateRoleDto request, CancellationToken cancellationToken = default);
    Task DeleteRoleAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<RoleDto>> GetRolesAsync(PaginationRequest pagination, CancellationToken cancellationToken = default);
    Task AssignPermissionsAsync(AssignPermissionsDto request, CancellationToken cancellationToken = default);
}

public interface IPermissionService
{
    Task<PagedResult<PermissionDto>> GetPermissionsAsync(PaginationRequest pagination, CancellationToken cancellationToken = default);
}
