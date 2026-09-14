namespace Shared.DTOs.SharedModels
{
    public class TenantBaseEntityDto : BaseEntityDto
    {
        public int? Fk_Tenant { get; set; }
        public bool IsCommon => Fk_Tenant == null;
    }
}
