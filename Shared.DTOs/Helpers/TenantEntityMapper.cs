using Entities.Shared;
using Shared.DTOs.SharedModels;

namespace Shared.DTOs.Helpers
{
    public static class TenantEntityMapper
    {
        public static TenantBaseEntityDto MapAuditFields(TenantBaseEntity entity)
        {
            return new TenantBaseEntityDto
            {
                Id = entity.Id,
                Fk_Tenant = entity.Fk_Tenant,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedByUserID = entity.CreatedByUserID,
                CreatedByName = entity.CreatedByName,
                CreatedByDeviceID = entity.CreatedByDeviceID,
                CreatedByIp = entity.CreatedByIp,
                UpdatedAt = entity.UpdatedAt,
                UpdatedByUserID = entity.UpdatedByUserID,
                UpdatedByName = entity.UpdatedByName,
                UpdatedByDeviceID = entity.UpdatedByDeviceID,
                UpdatedByIp = entity.UpdatedByIp,
                SoftDelete = entity.SoftDelete,
                Sort = entity.Sort
            };
        }

    }
}
