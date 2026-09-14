using Data;
using Entities.DBModels.Permission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Modules.Permission
{
    public class RolePermissionRepository : GenericRepository<RolePermission>
    {
        public RolePermissionRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<HashSet<string>> GetUserPermissions(int fk_User)
        {
            DateTime now = DateTime.UtcNow;

            // Get permissions based on user roles
            var query = Find(rp =>
                    !rp.Functionality.SoftDelete &&
                    !rp.Functionality.Resource.SoftDelete &&
                     rp.Functionality.IsActive &&
                    rp.Functionality.Resource.IsActive &&
                    rp.Role.UserRoles.Any(u =>
                        u.Fk_User == fk_User &&
                        (u.StartsAtUtc == null || u.StartsAtUtc <= now) &&
                        (u.EndsAtUtc == null || u.EndsAtUtc >= now)
                    ) &&
                    !rp.SoftDelete
                )
                .Select(rp => rp.Functionality.FunctionalityCode);

            var list = await query.ToListAsync();
            return list.ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}
