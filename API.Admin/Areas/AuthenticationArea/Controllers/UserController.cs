using AutoMapper;
using Data;
using Entities.DBModels.Users;
using Entities.Enums;
using Identity;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.DTOs;
using Shared.DTOs.HR.PermissionModels;
using Shared.DTOs.SharedModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Utilities.ActionFilters;
using Utilities.RequestHandler;
using Utilities.Settings;

namespace API.Admin.Areas.AuthenticationArea.Controllers
{
    [Area("Authentication")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    [Route("api/[area]/[controller]")]
    public class UserController : AuthController
    {
        public UserController(TenantConnectionResolver tenantService, JwtUtil jwtUtil,
                                         LinkGenerator linkGenerator,
                                         IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto, IWebHostEnvironment webHostEnvironment) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto,webHostEnvironment)
        {
        }

        [HttpGet]
        [Route(nameof(GetUserProfile))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserProfile()
        {
            return Ok(await GetUser(GetUserId()));
        }

        [HttpPost]
        [Route(nameof(EditUser))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditUser([FromQuery] int id,
                                                  [FromQuery] ApplicationEnum applicationEnum,
                                                  [FromBody] EditUserDto model)
        {
            if(applicationEnum!= ApplicationEnum.WebApp_AdminPortal)
            {
                id = GetUserId();
            }


            User userDB = await UnitOfWork.UserRepository.GetByIdAsync(id, trackChanges: true);

            userDB.Email = model.Email;
            userDB.Phone = model.Phone;
            userDB.FullName = model.FullName;
            userDB.UserName = model.UserName;
            userDB.PhonePrefix = model.PhonePrefix;
            userDB.FK_Image = model.FK_Image; 

            if (!string.IsNullOrEmpty(model.Password))
            {
                #region set password


                PasswordHasher.CreatePasswordHash(model.Password, out string passwordHash, out string passwordSalt);

                userDB.PasswordHash = passwordHash;
                userDB.PasswordSalt = passwordSalt;

                #endregion

            }
            await SetAuditFieldsForUpdate(userDB);

            await UnitOfWork.SaveChangesAsync();

            return Ok(await GetUser(GetUserId()));
        }
      
        [HttpPost]
        [Route(nameof(GetShortLink))]
        [ProducesResponseType(typeof(DataDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetShortLink([FromBody] UserShortLinkDto user)
        {
            UserDto userDto = await GetUser();
            string refreshToken = await GetRefreshToken();
            string portal = user.Role + "Portal";
            Dictionary<string, string> claims = new()
            {
                { ClaimTypes.Role, user.Role },
                { "aud", portal },
                { "refreshToken", refreshToken }
            };

            // Email is optional (parents can register without one) — skip null to avoid Claim() crash
            if (!string.IsNullOrEmpty(userDto.Email))
            {
                claims.Add(JwtRegisteredClaimNames.Email, userDto.Email);
            }

           

            string jwt = _jwtUtil.GenerateJwtToken(GetUserId(), 5, claims);
            string url = GetPortalUrl(portal);
            string redirectUrl = $"{url}/auth/callback?token={jwt}";
            return Ok(new DataDto
            {
                Value = redirectUrl
            });
        }

        [HttpGet]
        [Route(nameof(GetUserPermissions))]
        [ProducesResponseType(typeof(UserPermissionCacheDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPermissions()
        {
            await IsAuth();
            int fk_User = GetUserId();

           var data = (new UserPermissionCacheDto
            {
                FunctionalityCodes = await UnitOfWork.RolePermissionRepository.GetUserPermissions(fk_User)
            });
            return Ok(data);
        }
    }
}
