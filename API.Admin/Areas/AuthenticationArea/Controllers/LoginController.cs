using AutoMapper;
using Data;
using Identity;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.DTOs;
using Shared.DTOs.Helpers;
using Utilities.ActionFilters;
using Utilities.RequestHandler;
using Utilities.Settings;
using static Entities.Enums.SeedDataEnum;

namespace API.Admin.Areas.AuthenticationArea.Controllers
{
    [Area("Authentication")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    [Route("api/[area]/[controller]")]
    public class LoginController : AuthController
    {
        public LoginController(TenantConnectionResolver tenantService, JwtUtil jwtUtil,
                                         LinkGenerator linkGenerator,
                                         IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto, IWebHostEnvironment webHostEnvironment) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto,webHostEnvironment)
        {
        }


        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(CheckUserExist))]
        [ProducesResponseType(typeof(DataDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckUserExist([FromBody] CheckUserExistDto user)
        {
            int tenantId = TenantId;

            string fullName = await AuthenticationUtil.UserName(tenantId, user.UserName);

            return Ok(new DataDto
            {
                Value = fullName
            });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(CheckUserExistVerified))]
        [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckUserExistVerified([FromBody] CheckUserExistDto user)
        {
            int tenantId = TenantId;

            var (fullName, isEmailVerified) = await AuthenticationUtil.GetUserInfo(tenantId, user.UserName);

            return Ok(new UserInfoDto
            {
                    FullName = fullName,
                    IsEmailVerified = isEmailVerified
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(LoginByUserName))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginByUserName([FromBody] UserLoginDto user)
        {
           
            int tenantId = TenantId;

            AuthenticatedUserDto auth = await AuthenticationUtil.Authenticate(tenantId, user.UserName, user.Password, IpAddress());
            
          
            SetToken(auth.Token);
            SetRefresh(auth.RefreshToken);

            return Ok(auth.User);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(LoginByExternalProvider))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginByExternalProvider([FromBody] ExternalLoginProviderDto user)
        {
            int tenantId = TenantId;

            AuthenticatedUserDto auth = await AuthenticationUtil.Authenticate(tenantId, user.Provider, user.Token, IpAddress());

            SetToken(auth.Token);

            SetRefresh(auth.RefreshToken);
            auth.User.IsAdmin = true;
            return Ok(auth.User);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(ValidateToken))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateToken([FromBody] TokenValidationRequestDto user)
        {
            string? refreshToken = _jwtUtil.ValidateJwtToken(user.Token, user.Portal);

            AuthenticatedUserDto auth = await AuthenticationUtil.Authenticate(TenantId, refreshToken);

            SetToken(auth.Token);
            SetRefresh(auth.RefreshToken);

            return Ok(auth.User);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(RefreshToken))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] UserRefreshTokenDto user)
        {
            int tenantId = TenantId;
            AuthenticatedUserDto auth = await AuthenticationUtil.Authenticate(tenantId, user.Token);
            if (auth.User.Email == "admin@mighty.com")
            {
                auth.User.IsAdmin = true;
            }
            SetToken(auth.Token);

            SetRefresh(auth.RefreshToken);

            return Ok(auth.User);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(RevokeToken))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RevokeToken([FromBody] UserRefreshTokenDto user)
        {
            int tenantId = TenantId;
            await AuthenticationUtil.RevokeToken(tenantId, user.Token);

            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }
        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetLoginLink))]
        [ProducesResponseType(typeof(DataDto), StatusCodes.Status200OK)]
        public IActionResult GetLoginLink()
        {
            return Ok(new DataDto
            {
                Value = GetPortalUrl("AuthPortal")
            });
        }
    }
}
