using Shared.DTOs.SharedModels;
using Utilities.RequestHandler;

namespace Shared.DTOs.CommonModels
{
    public class ApplicationLanguageParameters : RequestParameters
    {
        public new bool? IsActive { get; set; }
    }
    public class ApplicationLanguageDto 
    {
        public string? Name { get; set; }
        public string? LanguageCode { get; set; }
        public bool IsMain { get; set; }
        public bool IsRTL { get; set; }

        //Foreign Key
        public int? Fk_Image { get; set; }
        public TenantBaseEntityDto? BaseEntity { get; set; }


    }
}
