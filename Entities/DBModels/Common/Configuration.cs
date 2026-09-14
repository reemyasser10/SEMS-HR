using Entities.Constants;
using Entities.Enums;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Common
{
    [Table(nameof(Configuration), Schema = SchemaNames.Common)]
    public class Configuration : TenantBaseEntity
    {
        public ModuleEnum Module { get; set; }

        public required string Key { get; set; }

        public string? Value { get; set; }
    }
}
