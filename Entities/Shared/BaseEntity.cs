using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Shared
{
    public abstract class BaseEntity
    {
        // Base properties

        [Key]
        public int Id { get; set; }

        public int Sort { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public bool SoftDelete { get; set; }

        // Audit properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedByName { get; set; }

        public string? UpdatedByName { get; set; }

        public int? CreatedByUserID { get; set; }

        public int? UpdatedByUserID { get; set; }

        public int? CreatedByDeviceID { get; set; }

        public int? UpdatedByDeviceID { get; set; }

        public string? CreatedByIp { get; set; }

        public string? UpdatedByIp { get; set; }
    }
}
