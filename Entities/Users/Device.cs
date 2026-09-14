using Entities.Constants;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Users
{
    [Table(nameof(Device), Schema = SchemaNames.UserManagement)]
    public class Device : BaseEntity
    {
        [ForeignKey(nameof(User))]
        public int Fk_User { get; set; }

        public string? FirebaseToken { get; set; }

        public string? DeviceType { get; set; }

        public string? AppVersion { get; set; }

        public string? DeviceVersion { get; set; }

        public string? DeviceModel { get; set; }


        // Navigation property
        public User? User { get; set; }
    }
}
