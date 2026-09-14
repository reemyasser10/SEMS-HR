using Entities.Constants;
using Entities.Enums;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Common
{
    [Table(nameof(Translation), Schema = SchemaNames.Common)]
    public class Translation : TenantBaseEntity
    {
        public ApplicationEnum ApplicationEnum { get; set; }

        public required string LanguageCode { get; set; }

        public required string RowText { get; set; }

        public required string TranslatedText { get; set; }
    }
}
