using Entities.Constants;
using Entities.DBModels.Users;
using Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBModels.Permission
{
    [Table(nameof(Administrator), Schema = SchemaNames.Permission)]
    public class Administrator : TenantBaseEntity
    {
        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        public string? JobTitle { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}
