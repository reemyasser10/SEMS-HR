namespace HR.Infrastructure.Persistence;

using HR.Application.Common.Interfaces;
using HR.Domain.Entities.Auth;
using HR.Domain.Entities.System;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IGenericRepository<User> Users { get; }
    public IGenericRepository<Role> Roles { get; }
    public IGenericRepository<Permission> Permissions { get; }
    public IGenericRepository<UserRole> UserRoles { get; }
    public IGenericRepository<RolePermission> RolePermissions { get; }
    public IGenericRepository<RefreshToken> RefreshTokens { get; }
    public IGenericRepository<Administrator> Administrators { get; }
    public IGenericRepository<AuditLog> AuditLogs { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new GenericRepository<User>(_context);
        Roles = new GenericRepository<Role>(_context);
        Permissions = new GenericRepository<Permission>(_context);
        UserRoles = new GenericRepository<UserRole>(_context);
        RolePermissions = new GenericRepository<RolePermission>(_context);
        RefreshTokens = new GenericRepository<RefreshToken>(_context);
        Administrators = new GenericRepository<Administrator>(_context);
        AuditLogs = new GenericRepository<AuditLog>(_context);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
