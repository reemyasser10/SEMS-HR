namespace Shared.DTOs.SharedModels
{
    public class BaseEntityDto
    {
        public int Id { get; set; }

        public int Sort { get; set; }

        public bool IsActive { get; set; }

        public bool SoftDelete { get; set; }

        // Audit properties

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
