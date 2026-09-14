using Entities.Constants;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Tenants
{
    [Table(nameof(TenantSetting), Schema = SchemaNames.Tenant)]
    public class TenantSetting : TenantBaseEntity
    {
        [ForeignKey(nameof(Tenant))]
        public new int Fk_Tenant { get; set; }

        public required string Key { get; set; }

        public string? Value { get; set; }
    }
}
