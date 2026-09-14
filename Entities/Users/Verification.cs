using Entities.Constants;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Users
{
    [Table(nameof(Verification), Schema = SchemaNames.UserManagement)]
    public class Verification : TenantBaseEntity
    {
        [ForeignKey(nameof(Tenant))]
        public new int Fk_Tenant { get; set; }

        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        public required string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsValid => !IsExpired;

        // Navigation property
        public User? User { get; set; }
    }
}
