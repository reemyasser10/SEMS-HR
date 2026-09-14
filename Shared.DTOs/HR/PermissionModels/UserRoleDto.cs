using Entities.DBModels.Permission;
using Entities.DBModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.RequestHandler;

namespace Shared.DTOs.HR.PermissionModels
{
    public class UserRoleParameters : RequestParameters
    {
        public int Fk_User { get; set; }
        public int Fk_Role { get; set; }
    }
    public class UserRoleDto
    {
        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        [ForeignKey(nameof(Role))]
        public int Fk_Role { get; set; }

        public DateTime? StartsAtUtc { get; set; }
        public DateTime? EndsAtUtc { get; set; }

    }
}
