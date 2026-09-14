namespace Shared.DTOs.SharedModels
{
    public class TenantDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Domain { get; set; }
        public string? Description { get; set; }

        public ICollection<TenantSettingDto>? Settings { get; set; }
    }

    public class TenantSettingDto
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
