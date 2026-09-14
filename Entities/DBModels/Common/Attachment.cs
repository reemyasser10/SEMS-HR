using Entities.Constants;
using Entities.Enums;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Common
{
    [Table(nameof(Attachment), Schema = SchemaNames.Common)]
    public class Attachment : TenantBaseEntity
    {
        public EntityTypeEnum EntityType { get; set; }

        public int? EntityId { get; set; }

        public string? FileName { get; set; }

        public required string FilePath { get; set; }

        public required string ContainerName { get; set; }

        public required string StoragePath { get; set; }

        public string? FullPath => $"{StoragePath}/{ContainerName}/{FilePath}";

        public string? FileType { get; set; }

        public long? FileSize { get; set; }

        public string? Description { get; set; }
    }
}
