using Entities.Constants;
using Entities.Enums;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Common
{
    [Table(nameof(Lookup), Schema = SchemaNames.Common)]
    public class Lookup : TenantBaseEntity
    {
        public LookupEntityTypeEnum EntityType { get; set; }

        public int? EntityId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? ColorCode { get; set; }

        [ForeignKey(nameof(Image))]
        public int?  Fk_Image { get; set; }

        public Attachment? Image { get; set; }
        public LookupLang? LookupLang { get; set; }
    }
    [Table(nameof(LookupLang), Schema = SchemaNames.Common)]
    public class LookupLang : LangEntity<Lookup>
    {
        public required string Name { get; set; }
    }
}
