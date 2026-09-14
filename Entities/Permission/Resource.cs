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
    [Table(nameof(Resource), Schema = SchemaNames.Permission)]
    public class Resource : TenantBaseEntity
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        // Navigation property
        public ICollection<Functionality>? Functionalities { get; set; }
    }
}
