using Entities.Constants;
using Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBModels.Permission
{
    [Table(nameof(RolePermission), Schema = SchemaNames.Permission)]
    public class RolePermission : TenantBaseEntity
    {
        [ForeignKey(nameof(Functionality))]
        public int Fk_Functionality { get; set; }

        [ForeignKey(nameof(Role))]
        public int Fk_Role { get; set; }

        // Navigation property

        public Functionality? Functionality { get; set; }

        public Role? Role { get; set; }
    }
}
