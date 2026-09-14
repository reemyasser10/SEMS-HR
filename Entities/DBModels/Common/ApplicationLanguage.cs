using Entities.Constants;
using Entities.DBModels.Users;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Common
{
    [Table(nameof(ApplicationLanguage), Schema = SchemaNames.Common)]
    public class ApplicationLanguage : TenantBaseEntity
    {
        public required string Name { get; set; }
        public required string LanguageCode { get; set; }
        public bool IsMain { get; set; }
        public bool IsRTL { get; set; }

        //Foreign Key

        [ForeignKey(nameof(Image))]
        public int? Fk_Image { get; set; }

        //Navigation
        public required Attachment Image { get; set; }

    }
}
