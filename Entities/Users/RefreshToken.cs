using Entities.Constants;
using Entities.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Users
{
    [Table(nameof(RefreshToken), Schema = SchemaNames.UserManagement)]
    public class RefreshToken : TenantBaseEntity
    {
        [ForeignKey(nameof(Tenant))]
        public new int Fk_Tenant { get; set; }

        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        public required string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        public string? ReplacedByToken { get; set; }

        [DisplayName(nameof(IsExpired))]
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        [DisplayName(nameof(IsActive))]
        public bool IsValid => !IsRevoked && !IsExpired;

        // Navigation property
        public User? User { get; set; }
    }
}
