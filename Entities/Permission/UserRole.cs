using Entities.Constants;
using Entities.DBModels.Users;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Permission
{
    [Table(nameof(UserRole), Schema = SchemaNames.Permission)]
    public class UserRole : TenantBaseEntity
    {
        public DateTime? StartsAtUtc { get; set; }
        public DateTime? EndsAtUtc { get; set; }

        [ForeignKey(nameof(Tenant))]
        public new int Fk_Tenant { get; set; }

        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        [ForeignKey(nameof(Role))]
        public int Fk_Role { get; set; }

        // Navigation property

        public User? User { get; set; }

        public Role? Role { get; set; }
      
    }
}
