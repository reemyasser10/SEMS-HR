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
    [Table(nameof(Functionality), Schema = SchemaNames.Permission)]
    public class Functionality : TenantBaseEntity
    {
        [ForeignKey(nameof(Resource))]
        public int Fk_Resource { get; set; }

        public required string Name { get; set; }

        public required string FunctionalityCode { get; set; }

        public string? Description { get; set; }

        public new bool IsActive { get; set; } = true;

        // Navigation property

        public Resource? Resource { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
