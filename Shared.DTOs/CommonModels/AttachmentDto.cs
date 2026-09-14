using Entities.Enums;
using Shared.DTOs.SharedModels;
using System.Text.Json.Serialization;

namespace Shared.DTOs.CommonModels
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public EntityTypeEnum EntityType { get; set; }

        public int? EntityId { get; set; }

        public string? FileName { get; set; }

        [JsonIgnore]
        public string? StoragePath { get; set; }

        [JsonIgnore]
        public string? FilePath { get; set; }

        [JsonIgnore]
        public string? ContainerName { get; set; }

        public string? FileType { get; set; }

        public long? FileSize { get; set; }

        public string? Description { get; set; }

        public string? FullPath { get; set; }

        public bool NotUsed => EntityId == null;

        public TenantBaseEntityDto? BaseEntity { get; set; }
    }
}
