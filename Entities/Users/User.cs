using Entities.Constants;
using Entities.DBModels.Common;
using Entities.DBModels.Permission;
using Entities.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Users
{
    [Table(nameof(User), Schema = SchemaNames.UserManagement)]
    public class User : TenantBaseEntity
    {
        [ForeignKey(nameof(Tenant))]
        public new int Fk_Tenant { get; set; }

        public string? FullName { get; set; }

        public required string UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
        public string? PhonePrefix { get; set; }
        public string? WorkAddress_Governorate { get; set; }
        public string? WorkAddress_City { get; set; }
        public string? WorkAddress_District { get; set; }

        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }

        public bool IsEmailVerified { get; set; }
        public bool IsApproved { get; set; }

        public bool IsPhoneVerified { get; set; }

        public bool IsLockedOut { get; set; }

        public bool IsBan { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime? LastLoginFailedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }


        [ForeignKey(nameof(Image))]
        public int? FK_Image { get; set; }
        public string? LastLoginBy { get; set; }

        // Navigation property
        public ICollection<RefreshToken>? RefreshTokens { get; set; }

        public ICollection<Verification>? Verifications { get; set; }

        public ICollection<Device>? Devices { get; set; }

        public ICollection<ExternalLogin>? ExternalLogins { get; set; }

        public Attachment? Image { get; set; }
        public Administrator? Administrator { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }

    
    }
}
