namespace HR.Application.Common.Interfaces;

using HR.Domain.Entities.Auth;
using HR.Domain.Entities.System;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<User> Users { get; }
    IGenericRepository<Role> Roles { get; }
    IGenericRepository<Permission> Permissions { get; }
    IGenericRepository<UserRole> UserRoles { get; }
    IGenericRepository<RolePermission> RolePermissions { get; }
    IGenericRepository<RefreshToken> RefreshTokens { get; }
    IGenericRepository<Administrator> Administrators { get; }
    IGenericRepository<AuditLog> AuditLogs { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
