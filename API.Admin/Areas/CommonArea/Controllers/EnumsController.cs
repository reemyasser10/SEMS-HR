using API.Admin.Controllers;
using AutoMapper;
using Data;
using Entities.Enums;
using Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilities.ActionFilters;
using Utilities.Extensions;
using Utilities.RequestHandler;
using Utilities.Settings;
using static Entities.Enums.SeedDataEnum;

namespace API.Admin.Areas.CommonArea.Controllers
{
    [Area("Common")]
    [ApiExplorerSettings(GroupName = "Common")]
    [Route("api/[area]/[controller]")]
    public class EnumsController : ExtendControllerBase
    {
        public EnumsController(TenantConnectionResolver tenantService,
                                JwtUtil jwtUtil, LinkGenerator linkGenerator,
                                IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetAllEnums))]
        [ProducesResponseType(typeof(Dictionary<string, List<EnumDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAllEnums()
        {
            Dictionary<string, List<EnumDto>> enums = EnumHelper.GetAllEnums(
                typeof(ApplicationEnum),
                typeof(EntityTypeEnum),
                typeof(ExternalLoginProviderEnum),
                typeof(LookupEntityTypeEnum),
                typeof(GenderEnum)
            );

            return Ok(enums);
        }
    }
}
