using Entities.DBModels.Tenants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Shared
{
    public abstract class TenantBaseEntity : BaseEntity
    {
        [ForeignKey(nameof(Tenant))]
        public int? Fk_Tenant { get; set; }

        // Navigation property

        public Tenant? Tenant { get; set; }

        public bool IsCommon => Fk_Tenant == null;
    }
}
