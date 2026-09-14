using Entities.Constants;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Common
{
    [Table(nameof(EmailTemplate), Schema = SchemaNames.Common)]
    public class EmailTemplate : TenantBaseEntity
    {
        public required string Name { get; set; }

        public required string Subject { get; set; }

        public required string Body { get; set; }
    }
}
