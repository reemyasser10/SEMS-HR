using AutoMapper;
using Data;
using Entities.DBModels.Users;
using Identity;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Utilities.ActionFilters;
using Utilities.RequestHandler;
using Utilities.Settings;

namespace API.Admin.Areas.AuthenticationArea.Controllers
{
    [Area("Authentication")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    [Route("api/[area]/[controller]")]
    public class RegisterController : AuthController
    {
        public RegisterController(TenantConnectionResolver tenantService, JwtUtil jwtUtil,
                                         LinkGenerator linkGenerator,
                                         IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto, IWebHostEnvironment webHostEnvironment) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto,webHostEnvironment)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(Register))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        
        {
            int tenantId = TenantId;

            if (await AuthenticationUtil.IsExist(tenantId, user.UserName) || 
                (!string.IsNullOrEmpty(user.Email) && await AuthenticationUtil.IsEmailExist(tenantId, user.Email)))
            {
                throw new Exception("Email Address already registered");
            }
            User userDB = _mapper.Map<User>(user);

            userDB.Fk_Tenant = tenantId;

            #region set password


            PasswordHasher.CreatePasswordHash(user.Password, out string passwordHash, out string passwordSalt);

            userDB.PasswordHash = passwordHash;
            userDB.PasswordSalt = passwordSalt;

            #endregion

            _ = await SetAuditFieldsForCreation(userDB);

            await UnitOfWork.UserRepository.AddAsync(userDB);
            _ = await UnitOfWork.SaveChangesAsync();

        

            AuthenticatedUserDto auth = await AuthenticationUtil.Authenticate(tenantId, user.UserName, user.Password, IpAddress(),isSignUp:true);
            SetToken(auth.Token);

            SetRefresh(auth.RefreshToken);
           
            return Ok(auth.User);
        }
    }
}
