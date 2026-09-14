using Entities.Constants;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Permission
{
    [Table(nameof(Role), Schema = SchemaNames.Permission)]
    public class Role : TenantBaseEntity
    {
        public required string Name { get; set; }

        public string? Description { get; set; }
        public string? ColorCode { get; set; }

        // Navigation property
        public ICollection<RolePermission>? RolePermissions { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
