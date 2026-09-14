namespace HR.Application.Admin;

using HR.Application.Common.Extensions;
using HR.Application.Common.Interfaces;
using HR.Application.Common.Models;
using HR.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationDbContext _context;

    public RoleService(IUnitOfWork unitOfWork, IApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<int> CreateRoleAsync(CreateRoleDto request, CancellationToken cancellationToken = default)
    {
        var role = new Role { Name = request.Name, Description = request.Description };
        await _unitOfWork.Roles.AddAsync(role, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return role.Id;
    }

    public async Task UpdateRoleAsync(UpdateRoleDto request, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.Id, cancellationToken);
        if (role == null) throw new Application.Common.Exceptions.NotFoundException("RoleNotFound");

        role.Name = request.Name;
        role.Description = request.Description;
        
        _unitOfWork.Roles.Update(role);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task DeleteRoleAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id, cancellationToken);
        if (role == null) throw new Application.Common.Exceptions.NotFoundException("RoleNotFound");

        _unitOfWork.Roles.Remove(role);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<PagedResult<RoleDto>> GetRolesAsync(PaginationRequest pagination, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Roles.AsQueryable()
            .OrderBy(r => r.Id)
            .ToPagedResultAsync(
                pagination,
                r => new RoleDto { Id = r.Id, Name = r.Name, Description = r.Description },
                cancellationToken);
    }

    public async Task AssignPermissionsAsync(AssignPermissionsDto request, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles.Include(r => r.RolePermissions).FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role == null) throw new Application.Common.Exceptions.NotFoundException("RoleNotFound");

        _context.RolePermissions.RemoveRange(role.RolePermissions);
        var newPermissions = request.PermissionIds.Select(pid => new RolePermission { RoleId = request.RoleId, PermissionId = pid });
        _context.RolePermissions.AddRange(newPermissions);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<PermissionDto>> GetPermissionsAsync(PaginationRequest pagination, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Permissions.AsQueryable()
            .OrderBy(p => p.Id)
            .ToPagedResultAsync(
                pagination,
                p => new PermissionDto { Id = p.Id, Name = p.Name, Module = p.Module, Description = p.Description },
                cancellationToken);
    }
}
