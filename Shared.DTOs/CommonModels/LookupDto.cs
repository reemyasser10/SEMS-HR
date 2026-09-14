
using Entities.Enums;
using Shared.DTOs.SharedModels;
using Utilities.RequestHandler;

namespace Shared.DTOs.CommonModels
{
    public class LookupParameters : RequestParameters
    {
        public LookupEntityTypeEnum? EntityType { get; set; }
        public List<int>? EntityTypes { get; set; }
    }

    public class LookupDto
    {
        public int Id { get; set; }
        public string? EncryptedId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required string Name { get; set; }
        public string? LocalizedName { get; set; }
        public required string Description { get; set; }
        public required string ColorCode { get; set; }
        public required string EntityType { get; set; }
        public required int EntityTypeId { get; set; }
        public int? EntityId { get; set; }
        public TenantBaseEntityDto? BaseEntity { get; set; }
    }

    public class CustomLookUpDto
    {
        public int Id { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public string Description { get; set; }
        public bool Flag { get; set; }
        public LookupEntityTypeEnum? EntityType { get; set; }
        public int? EntityId { get; set; }
    }

    public class CreateLookupDto
    {
        public required string Name { get; set; }
        public string? LocalizedName { get; set; }
        public required string Description { get; set; }
        public required string ColorCode { get; set; }
        public int EntityType { get; set; }
        public int? EntityId { get; set; }
    }

    public class EditLookupDto : CreateLookupDto
    {
    }

    public class LookupCustomMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LocationHierarchyDto
    {
        public Dictionary<string, Dictionary<string, List<string>>> Locations { get; set; } = new();
    }

    public class CreateGovernorateDto
    {
        public required string Name { get; set; }
        public string? NameAr { get; set; }
        public int SortOrder { get; set; }
    }

    public class CreateCityDto
    {
        public required string Name { get; set; }
        public string? NameAr { get; set; }
        public int Fk_Governorate { get; set; }
    }

    public class CreateDistrictDto
    {
        public required string Name { get; set; }
        public string? NameAr { get; set; }
        public int Fk_City { get; set; }
    }
}


