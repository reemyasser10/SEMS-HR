using AutoMapper;
using Data;
using Entities.DBModels.Users;
using Identity;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.DTOs;
using Utilities.ActionFilters;
using Utilities.Extensions;
using Utilities.RequestHandler;
using Utilities.Settings;

namespace API.Admin.Areas.AuthenticationArea.Controllers
{
    [Area("Authentication")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    [Route("api/[area]/[controller]")]
    public class VerificationController : AuthController
    {
        public VerificationController(TenantConnectionResolver tenantService, JwtUtil jwtUtil,
                                         LinkGenerator linkGenerator,
                                         IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto, IWebHostEnvironment webHostEnvironment) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto,webHostEnvironment)
        {
        }

        [HttpPost]
        [Route(nameof(GetVerificationEmailCode))]
        [ProducesResponseType(typeof(DataDto), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetVerificationEmailCode([FromBody] EmailVerificationDto model)
        {
            int? userId = await AuthenticationUtil.GetUserIdByUserName(TenantId, model.Email);


            if (userId.IsNull())
            {
                throw new Exception("User not Found");
            }
            string code = await AuthenticationUtil.CreateVerification(TenantId, userId.Value, _appSettings.VerificationTTL, IpAddress());

            await SendVerification(code,userId.Value);

            return Ok(new DataDto
            {
                Value = code
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(VerifyEmail))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyEmail([FromBody] UserVerificationDto userVerification)
        {
            User user = await UnitOfWork.UserRepository.Find(a => a.UserName == userVerification.Email || a.Email == userVerification.Email).SingleOrDefaultAsync();

            await AuthenticationUtil.VerifyEmail(TenantId, user.Id, userVerification.Code, _appSettings.VerificationTTL);
            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }
    }
}
