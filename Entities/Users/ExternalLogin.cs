using Entities.Constants;
using Entities.Enums;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Users
{
    [Table(nameof(ExternalLogin), Schema = SchemaNames.UserManagement)]
    public class ExternalLogin : TenantBaseEntity
    {
        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        public ExternalLoginProviderEnum Provider { get; set; }

        public required string ProviderKey { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}
