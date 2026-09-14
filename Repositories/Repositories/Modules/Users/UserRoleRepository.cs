using Data;
using Entities.DBModels.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Extensions;

namespace Repositories.Repositories.Modules.Users
{
    public class UserRoleRepository : GenericRepository<UserRole>
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task UpdateUserRoles(int fk_User, List<int> fk_Roles)
        {
            fk_Roles = fk_Roles ?? new List<int>();

            List<int> oldData = Find(a => !a.SoftDelete && a.Fk_User == fk_User)
                                 .Select(a => a.Fk_Role)
                                 .ToList();

            List<int> dataToAdd = fk_Roles.Except(oldData).ToList();
            List<int> dataToRemove = oldData.Except(fk_Roles).ToList();

            await AddRoles(fk_User, dataToAdd);
            RemoveRoles(fk_User, dataToRemove);
        }

        public async Task AddRoles(int fk_User, List<int> fk_Roles)
        {
            if (fk_Roles.IsNotNull() && fk_Roles.Any())
            {
                foreach (int fk_Role in fk_Roles)
                {
                     Delete(new UserRole
                    {
                        Fk_Role = fk_Role,
                        Fk_User = fk_User
                    });
                }
            }
        }

        public void RemoveRoles(int fk_User, List<int> fk_Roles)
        {
            if (fk_Roles.IsNotNull() && fk_Roles.Any())
            {
                IQueryable<UserRole> data = Find(ur => ur.Fk_User == fk_User && fk_Roles.Contains(ur.Fk_Role));

                _context.UserRole.RemoveRange(data);
            }
        }
    }

}
