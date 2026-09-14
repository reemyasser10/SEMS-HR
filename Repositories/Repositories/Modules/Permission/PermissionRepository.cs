using Data;
using Entities.DBModels.Permission;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.CommonModels;
using Shared.DTOs.HR.PermissionModels;
using Shared.DTOs.HR.PermissionModels;
using Utilities.Extensions;

namespace Repositories.Repositories.Modules.Permission
{
    public class PermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Utilities.RequestHandler.CryptoService _crypto;

        public PermissionRepository(ApplicationDbContext context, Utilities.RequestHandler.CryptoService crypto)
        {
            _context = context;
            _crypto = crypto;
        }

        // ──────────────────────────────────────────────
        // ROLE
        // ──────────────────────────────────────────────

        public IQueryable<RoleDto> GetRoles(RoleParameters parameters)
        {
            return _context.Role
                .Where(r => !r.SoftDelete)
                .Sort(parameters.OrderBy)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    EncryptedId = _crypto.EncryptObject(r.Id),
                    Name = r.Name,
                    Description = r.Description,
                    ColorCode = r.ColorCode,
                    IsActive=r.IsActive
                });
        }

        public IQueryable<CustomLookUpDto> GetRolesCustomLookup(RoleParameters parameters)
        {
            return _context.Role
                .Where(r => !r.SoftDelete && r.IsActive)
                .Select(r => new CustomLookUpDto
                {
                    Id = r.Id,
                    Name = r.Name
                });
        }

        public async Task<RoleCreateDto?> GetRoleByIdAsync(int id)
        {
            Role? role = null;
            if (id > 0)
            {
                role = await _context.Role
                    .Where(r => r.Id == id && !r.SoftDelete)
                    .FirstOrDefaultAsync();
                if (role == null) return null;
            }

            var resources = await _context.Resources
                .Where(res => !res.SoftDelete)
                .Select(res => new RoleResourceCreateDto
                {
                    Id = res.Id,
                    Name = res.Name,
                    Functionalities = res.Functionalities!
                        .Where(f => !f.SoftDelete)
                        .Select(f => new RoleFunctionalityCreateDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Code = f.FunctionalityCode,
                            IsSelected = id > 0 && f.RolePermissions!.Any(rp => rp.Fk_Role == id && !rp.SoftDelete)
                        }).ToList()
                }).ToListAsync();

            return new RoleCreateDto
            {
                Name = role?.Name ?? string.Empty,
                Description = role?.Description,
                ColorCode = role?.ColorCode,
                IsActive = role?.IsActive ?? true,
                Resources = resources
            };
        }

        public async Task<int> CreateRoleAsync(RoleCreateDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            var role = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                ColorCode = dto.ColorCode,
                IsActive = dto.IsActive,
                Fk_Tenant = tenantId,
                CreatedByUserID = createdByUserId,
                CreatedByName = createdByName
            };
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            var permissions = dto.Fk_Functionalities.Select(fId => new RolePermission
            {
                Fk_Role = role.Id,
                Fk_Functionality = fId,
                IsActive = true,
                Fk_Tenant = tenantId,
                CreatedByUserID = createdByUserId,
                CreatedByName = createdByName
            }).ToList();

            _context.RolePermissions.AddRange(permissions);
            await _context.SaveChangesAsync();

            return role.Id;
        }

        public async Task EditRoleAsync(int id, RoleEditDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            var role = await _context.Role.FirstOrDefaultAsync(r => r.Id == id && !r.SoftDelete);
            if (role == null) throw new Exception("Role not found");

            role.Name = dto.Name;
            role.Description = dto.Description;
            role.ColorCode = dto.ColorCode;
            role.IsActive = dto.IsActive;

            var existingPerms = await _context.RolePermissions
                .Where(rp => rp.Fk_Role == id && !rp.SoftDelete)
                .ToListAsync();

            foreach (var p in existingPerms) p.SoftDelete = true;

            foreach (var fId in dto.Fk_Functionalities)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    Fk_Role = id,
                    Fk_Functionality = fId,
                    IsActive = true,
                    Fk_Tenant = tenantId,
                    CreatedByUserID = createdByUserId,
                    CreatedByName = createdByName
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRolePermissionsAsync(UpdateResourcePermissionsDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            var role = await _context.Role.FirstOrDefaultAsync(r => r.Id == dto.Fk_Role && !r.SoftDelete);
            if (role != null)
            {
                if (dto.RoleName != null) role.Name = dto.RoleName;
                if (dto.Description != null) role.Description = dto.Description;
                if (dto.ColorCode != null) role.ColorCode = dto.ColorCode;
                if (dto.IsActive.HasValue) role.IsActive = dto.IsActive.Value;
            }

            var existingPerms = await _context.RolePermissions
                .Where(rp => rp.Fk_Role == dto.Fk_Role && !rp.SoftDelete
                    && rp.Functionality!.Fk_Resource == dto.Fk_Resource)
                .ToListAsync();

            foreach (var p in existingPerms) p.SoftDelete = true;

            foreach (var fId in dto.Fk_Functionalities)
            {
                if (!await _context.RolePermissions.AnyAsync(rp =>
                    rp.Fk_Role == dto.Fk_Role && rp.Fk_Functionality == fId && !rp.SoftDelete))
                {
                    _context.RolePermissions.Add(new RolePermission
                    {
                        Fk_Role = dto.Fk_Role,
                        Fk_Functionality = fId,
                        IsActive = true,
                        Fk_Tenant = tenantId,
                        CreatedByUserID = createdByUserId,
                        CreatedByName = createdByName
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Role.FirstOrDefaultAsync(r => r.Id == id && !r.SoftDelete);
            if (role == null) return false;
            role.SoftDelete = true;

            var perms = await _context.RolePermissions
                .Where(rp => rp.Fk_Role == id && !rp.SoftDelete)
                .ToListAsync();
            foreach (var p in perms) p.SoftDelete = true;

            var userRoles = await _context.UserRole
                .Where(ur => ur.Fk_Role == id && !ur.SoftDelete)
                .ToListAsync();
            foreach (var ur in userRoles) ur.SoftDelete = true;

            await _context.SaveChangesAsync();
            return true;
        }

        // ──────────────────────────────────────────────
        // RESOURCE
        // ──────────────────────────────────────────────

        public IQueryable<ResourceDto> GetResources(ResourceParameters parameters)
        {
            return _context.Resources
                .Where(r => !r.SoftDelete)
                .Sort(parameters.OrderBy)
                .Select(r => new ResourceDto
                {
                    Id = r.Id,
                    EncryptedId = _crypto.EncryptObject(r.Id),
                    Name = r.Name,
                    Description = r.Description,
                    IsActive=r.IsActive
                });
        }

        public IQueryable<CustomLookUpDto> GetResourcesCustomLookup(ResourceParameters parameters)
        {
            return _context.Resources
                .Where(r => !r.SoftDelete && r.IsActive)
                .Select(r => new CustomLookUpDto
                {
                    Id = r.Id,
                    Name = r.Name
                });
        }

        public async Task<ResourceDto?> GetResourceByIdAsync(int id)
        {
            return await _context.Resources
                .Where(r => r.Id == id && !r.SoftDelete)
                .Select(r => new ResourceDto
                {
                    Id = r.Id,
                    EncryptedId = _crypto.EncryptObject(r.Id),
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive
                }).FirstOrDefaultAsync();
        }

        public async Task<int> CreateResourceAsync(ResourceCreateDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            var entity = new Resource
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Fk_Tenant = tenantId,
                CreatedByUserID = createdByUserId,
                CreatedByName = createdByName
            };
            _context.Resources.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateResourceAsync(int id, ResourceEditDto dto)
        {
            var entity = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id && !r.SoftDelete);
            if (entity == null) throw new Exception("Resource not found");
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteResourceAsync(int id)
        {
            var entity = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id && !r.SoftDelete);
            if (entity == null) return false;
            entity.SoftDelete = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // ──────────────────────────────────────────────
        // FUNCTIONALITY
        // ──────────────────────────────────────────────

        public IQueryable<FunctionalityDto> GetFunctionalities(FunctionalityParameters parameters)
        {
            var query = _context.Functionalities.Where(f => !f.SoftDelete);

            if (parameters.Fk_Resource > 0)
                query = query.Where(f => f.Fk_Resource == parameters.Fk_Resource);

            return query.Sort(parameters.OrderBy).Select(f => new FunctionalityDto
            {
                Id = f.Id,
                EncryptedId = _crypto.EncryptObject(f.Id),
                Fk_Resource = f.Fk_Resource,
                ResourceName = f.Resource!.Name,
                Name = f.Name,
                FunctionalityCode = f.FunctionalityCode,
                Description = f.Description,
                IsActive=f.IsActive,
                Assignees = f.RolePermissions!
                    .Where(rp => !rp.SoftDelete)
                    .Select(rp => new RoleAssigneeDto
                    {
                        Fk_Role = rp.Fk_Role,
                        RoleName = rp.Role!.Name,
                        ColorCode = rp.Role.ColorCode
                    }).ToList()
            });
        }

        public IQueryable<CustomLookUpDto> GetFunctionalitiesCustomLookup(FunctionalityParameters parameters)
        {
            var query = _context.Functionalities.Where(f => !f.SoftDelete && f.IsActive);
            if (parameters.Fk_Resource > 0)
                query = query.Where(f => f.Fk_Resource == parameters.Fk_Resource);

            return query.Select(f => new CustomLookUpDto
            {
                Id = f.Id,
                Name = f.Name
            });
        }

        public async Task<FunctionalityDto?> GetFunctionalityByIdAsync(int id)
        {
            return await _context.Functionalities
                .Where(f => f.Id == id && !f.SoftDelete)
                .Select(f => new FunctionalityDto
                {
                    Id = f.Id,
                    EncryptedId = _crypto.EncryptObject(f.Id),
                    Fk_Resource = f.Fk_Resource,
                    ResourceName = f.Resource!.Name,
                    Name = f.Name,
                    FunctionalityCode = f.FunctionalityCode,
                    Description = f.Description,
                    IsActive = f.IsActive
                }).FirstOrDefaultAsync();
        }

        public async Task<int> CreateFunctionalityAsync(FunctionalityCreateDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            var entity = new Functionality
            {
                Fk_Resource = dto.Fk_Resource,
                Name = dto.Name,
                FunctionalityCode = dto.FunctionalityCode!,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Fk_Tenant = tenantId,
                CreatedByUserID = createdByUserId,
                CreatedByName = createdByName
            };
            _context.Functionalities.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateFunctionalityAsync(int id, FunctionalityEditDto dto)
        {
            var entity = await _context.Functionalities.FirstOrDefaultAsync(f => f.Id == id && !f.SoftDelete);
            if (entity == null) throw new Exception("Functionality not found");
            entity.Fk_Resource = dto.Fk_Resource;
            entity.Name = dto.Name;
            entity.FunctionalityCode = dto.FunctionalityCode!;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteFunctionalityAsync(int id)
        {
            var entity = await _context.Functionalities.FirstOrDefaultAsync(f => f.Id == id && !f.SoftDelete);
            if (entity == null) return false;
            entity.SoftDelete = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // ──────────────────────────────────────────────
        // ADMINISTRATOR
        // ──────────────────────────────────────────────

        public IQueryable<AdministratorDto> GetAdministrators(UserParameters parameters)
        {
            var query = _context.Administrators.Where(a => !a.SoftDelete);

            return query
                .Sort(parameters.OrderBy)
                .Select(a => new AdministratorDto
                {
                    Id = a.Id,
                    EncryptedId = _crypto.EncryptObject(a.Id),
                    Fk_User = a.Fk_User,
                    FullName = a.User!.FullName,
                    UserName = a.User.UserName,
                    EmailAddress = a.User.Email,
                    PhoneNumber = a.User.Phone,
                    JobTitle = a.JobTitle,
                    IsActive=a.IsActive,
                    Roles = a.User.UserRoles!
                        .Where(ur => !ur.SoftDelete)
                        .Select(ur => new RoleAssigneeDto
                        {
                            Fk_Role = ur.Fk_Role,
                            RoleName = ur.Role!.Name,
                            ColorCode = ur.Role.ColorCode
                        }).ToList(),
                });
        }

        public async Task<AdministratorDto?> GetAdministratorByIdAsync(int id)
        {
            return await _context.Administrators
                .Where(a => a.Id == id && !a.SoftDelete)
                .Select(a => new AdministratorDto
                {
                    Id = a.Id,
                    EncryptedId = _crypto.EncryptObject(a.Id),
                    Fk_User = a.Fk_User,
                    FullName = a.User!.FullName,
                    UserName = a.User.UserName,
                    EmailAddress = a.User.Email,
                    PhoneNumber = a.User.Phone,
                    JobTitle = a.JobTitle,
                    Roles = a.User.UserRoles!
                        .Where(ur => !ur.SoftDelete)
                        .Select(ur => new RoleAssigneeDto
                        {
                            Fk_Role = ur.Fk_Role,
                            RoleName = ur.Role!.Name,
                            ColorCode = ur.Role.ColorCode
                        }).ToList(),
                }).FirstOrDefaultAsync();
        }

        public async Task<AdministratorEditDto?> GetAdministratorEditDtoAsync(int id)
        {
            return await _context.Administrators
                .Where(a => a.Id == id && !a.SoftDelete)
                .Select(a => new AdministratorEditDto
                {
                    FullName = a.User!.FullName,
                    UserName = a.User.UserName,
                    EmailAddress = a.User.Email,
                    PhoneNumber = a.User.Phone,
                    JobTitle = a.JobTitle,
                    IsActive = a.User.IsApproved,
                    Fk_Roles = a.User.UserRoles!
                        .Where(ur => !ur.SoftDelete)
                        .Select(ur => ur.Fk_Role)
                        .ToList(),
                
                }).FirstOrDefaultAsync();
        }

        public async Task<int> CreateAdministratorAsync(AdministratorCreateDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => (u.UserName == dto.UserName || u.Email == dto.EmailAddress) && u.Fk_Tenant == tenantId && !u.SoftDelete);

            if (user == null)
            {
                user = new Entities.DBModels.Users.User
                {
                    UserName = dto.UserName!,
                    FullName = dto.FullName,
                    Email = dto.EmailAddress,
                    Phone = dto.PhoneNumber,
                    Fk_Tenant = tenantId,
                    IsApproved = dto.IsActive,
                    CreatedByUserID = createdByUserId,
                    CreatedByName = createdByName,
                    IsActive = dto.IsActive,
                    IsEmailVerified = true,
                    IsPhoneVerified = true
                };

                if (!string.IsNullOrEmpty(dto.PasswordHash))
                {
                    user.PasswordHash = dto.PasswordHash;
                    user.PasswordSalt = dto.PasswordSalt;
                }

                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }

            var admin = await _context.Administrators.FirstOrDefaultAsync(a => a.Fk_User == user.Id && !a.SoftDelete);
            if (admin == null)
            {
                admin = new Administrator
                {
                    Fk_User = user.Id,
                    JobTitle = dto.JobTitle,
                    Fk_Tenant = tenantId,
                    CreatedByUserID = createdByUserId,
                    CreatedByName = createdByName,
                    IsActive = dto.IsActive
                };
                _context.Administrators.Add(admin);
            }
            else 
            {
                admin.JobTitle = dto.JobTitle;
                admin.IsActive = dto.IsActive;
                admin.UpdatedAt = DateTime.UtcNow;
                admin.UpdatedByUserID = createdByUserId;
                admin.UpdatedByName = createdByName;
            }
            await _context.SaveChangesAsync();

          
            // Set Roles
            if (dto.Fk_Roles?.Any() == true)
            {
                var existingRoles = await _context.UserRole
                    .Where(ur => ur.Fk_User == user.Id)
                    .Select(ur => ur.Fk_Role)
                    .ToListAsync();

                foreach (var roleId in dto.Fk_Roles)
                {
                    if (!existingRoles.Contains(roleId))
                    {
                        _context.UserRole.Add(new UserRole
                        {
                            Fk_User = user.Id,
                            Fk_Role = roleId,
                            Fk_Tenant = tenantId,
                            CreatedByUserID = createdByUserId,
                            CreatedByName = createdByName
                        });
                    }
                }
            }
            await _context.SaveChangesAsync();

            return admin.Id;
        }

        public async Task EditAdministrator(int id, AdministratorEditDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            var admin = await _context.Administrators
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id && !a.SoftDelete);

            if (admin == null) throw new Exception("Administrator not found");

            admin.JobTitle = dto.JobTitle;
            admin.IsActive = dto.IsActive; 
            if (admin.User != null)
            {
                admin.User.FullName = dto.FullName;
                admin.User.Email = dto.EmailAddress;
                admin.User.Phone = dto.PhoneNumber;
                admin.User.IsApproved = dto.IsActive;
                admin.User.IsActive = dto.IsActive; 
            }

            // Update roles: sync existing with new list to avoid duplicate keys
            var userId = admin.Fk_User;
            var currentRoles = await _context.UserRole
                .Where(ur => ur.Fk_User == userId)
                .ToListAsync();

            var newRoleIds = dto.Fk_Roles ?? new List<int>();

            // 1. Remove roles that are not in the new list
            foreach (var ur in currentRoles.Where(ur => !ur.SoftDelete && !newRoleIds.Contains(ur.Fk_Role)))
            {
                ur.SoftDelete = true;
                ur.UpdatedAt = DateTime.UtcNow;
                ur.UpdatedByUserID = createdByUserId;
                ur.UpdatedByName = createdByName;
            }

            // 2. Add or reactivate roles in the new list
            foreach (var roleId in newRoleIds)
            {
                var existingRole = currentRoles.FirstOrDefault(ur => ur.Fk_Role == roleId);
                if (existingRole == null)
                {
                    _context.UserRole.Add(new UserRole
                    {
                        Fk_User = userId,
                        Fk_Role = roleId,
                        Fk_Tenant = tenantId,
                        CreatedByUserID = createdByUserId,
                        CreatedByName = createdByName
                    });
                }
                else if (existingRole.SoftDelete)
                {
                    existingRole.SoftDelete = false;
                    existingRole.UpdatedAt = DateTime.UtcNow;
                    existingRole.UpdatedByUserID = createdByUserId;
                    existingRole.UpdatedByName = createdByName;
                }
            }

         


            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAdministratorAsync(int id)
        {
            var admin = await _context.Administrators
                .Include(a => a.User)
                .ThenInclude(u => u!.UserRoles)
                .FirstOrDefaultAsync(a => a.Id == id && !a.SoftDelete);

            if (admin == null) return false;

            admin.SoftDelete = true;

           
            if (admin.User != null)
            {
                admin.User.SoftDelete = true;
                admin.User.IsActive = false;
                admin.User.IsApproved = false;

                if (admin.User.UserRoles != null)
                {
                    foreach (var role in admin.User.UserRoles)
                    {
                        role.SoftDelete = true;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task AssignUserToAdminAsync(int userId, string? jobTitle, List<int> roleIds, int tenantId, int createdByUserId, string createdByName)
        {
            // Check if Administrator record already exists
            bool adminExists = await _context.Administrators.AnyAsync(a => a.Fk_User == userId && !a.SoftDelete);
            if (!adminExists)
            {
                _context.Administrators.Add(new Administrator
                {
                    Fk_User = userId,
                    JobTitle = jobTitle,
                    Fk_Tenant = tenantId,
                    CreatedByUserID = createdByUserId,
                    CreatedByName = createdByName
                });
            }

            // Add roles
            foreach (var roleId in roleIds)
            {
                var existingRole = await _context.UserRole.FirstOrDefaultAsync(ur =>
                    ur.Fk_User == userId && ur.Fk_Role == roleId);

                if (existingRole == null)
                {
                    _context.UserRole.Add(new UserRole
                    {
                        Fk_User = userId,
                        Fk_Role = roleId,
                        Fk_Tenant = tenantId,
                        CreatedByUserID = createdByUserId,
                        CreatedByName = createdByName
                    });
                }
                else if (existingRole.SoftDelete)
                {
                    existingRole.SoftDelete = false;
                    existingRole.UpdatedAt = DateTime.UtcNow;
                    existingRole.UpdatedByUserID = createdByUserId;
                    existingRole.UpdatedByName = createdByName;
                }
            }

            await _context.SaveChangesAsync();
        }

        // ──────────────────────────────────────────────
        // USER ROLE
        // ──────────────────────────────────────────────

        public async Task AssignRoleToUserAsync(UserRoleDto dto, int tenantId, int createdByUserId, string createdByName)
        {
            bool exists = await _context.UserRole.AnyAsync(ur =>
                ur.Fk_User == dto.Fk_User && ur.Fk_Role == dto.Fk_Role && !ur.SoftDelete);

            if (exists) throw new Exception("User already has this role");

            _context.UserRole.Add(new UserRole
            {
                Fk_User = dto.Fk_User,
                Fk_Role = dto.Fk_Role,
                StartsAtUtc = dto.StartsAtUtc,
                EndsAtUtc = dto.EndsAtUtc,
                Fk_Tenant = tenantId,
                CreatedByUserID = createdByUserId,
                CreatedByName = createdByName
            });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRole
                .FirstOrDefaultAsync(ur => ur.Fk_User == userId && ur.Fk_Role == roleId && !ur.SoftDelete);

            if (userRole == null) throw new Exception("UserRole not found");
            userRole.SoftDelete = true;
            await _context.SaveChangesAsync();
        }

        // ──────────────────────────────────────────────
        // USER PERMISSIONS (for cache)
        // ──────────────────────────────────────────────

        public async Task<UserPermissionCacheDto> GetUserPermissionsAsync(int userId)
        {
            var now = DateTime.UtcNow;

            var codes = await _context.UserRole
                .Where(ur => ur.Fk_User == userId && !ur.SoftDelete
                    && (ur.StartsAtUtc == null || ur.StartsAtUtc <= now)
                    && (ur.EndsAtUtc == null || ur.EndsAtUtc >= now))
                .SelectMany(ur => ur.Role!.RolePermissions!
                    .Where(rp => !rp.SoftDelete && rp.Functionality!.IsActive)
                    .Select(rp => rp.Functionality!.FunctionalityCode))
                .Distinct()
                .ToListAsync();

            return new UserPermissionCacheDto
            {
                FunctionalityCodes = new HashSet<string>(codes)
            };
        }

        public async Task<AdminAccessDto> GetAdministratorAccessAsync(int userId)
        {
            var admin = await _context.Administrators
                .FirstOrDefaultAsync(a => a.Fk_User == userId && !a.SoftDelete);

            if (admin == null)
            {
                return new AdminAccessDto { IsAdmin = false };
            }

            // Check if user has any active roles
            bool hasActiveRoles = await _context.UserRole
                .AnyAsync(ur => ur.Fk_User == userId && !ur.SoftDelete && !ur.Role!.SoftDelete);

            if (!hasActiveRoles)
            {
                return new AdminAccessDto { IsAdmin = false };
            }

            return new AdminAccessDto
            {
                IsAdmin = true,
            };
        }
    }
}
