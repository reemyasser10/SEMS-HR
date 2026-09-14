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
using Utilities.RequestHandler;
using Utilities.Settings;

namespace API.Admin.Areas.AuthenticationArea.Controllers
{
    [Area("Authentication")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    [Route("api/[area]/[controller]")]
    public class ResetUserController : AuthController
    {
        public ResetUserController(TenantConnectionResolver tenantService, JwtUtil jwtUtil,
                                        LinkGenerator linkGenerator,
                                        IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto, IWebHostEnvironment webHostEnvironment) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto,webHostEnvironment)
        {
        }
        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(ForgetPassword))]
        [ProducesResponseType(typeof(DataDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto model)
        {
            int tenantId = TenantId;

            if (await AuthenticationUtil.IsExist(tenantId, model.UserName))
            {
                User? user = await UnitOfWork.UserRepository.Find(a => a.UserName == model.UserName || a.Email == model.UserName).SingleOrDefaultAsync();

                string code = await AuthenticationUtil.CreateVerification(tenantId, user.Id, _appSettings.VerificationTTL, IpAddress());

                await SendVerification(code, user.Id);

                return Ok(new DataDto
                {
                    Value = code
                });
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(ResetPassword))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            int tenantId = TenantId;
            User? user = await UnitOfWork.UserRepository.Find(a => a.UserName == model.Email || a.Email == model.Email, trackChanges: true).SingleOrDefaultAsync();
            #region set password


            PasswordHasher.CreatePasswordHash(model.Password, out string passwordHash, out string passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

             await UnitOfWork.UserRepository.SaveChangesAsync();

            #endregion

            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }
        [HttpPost]
        [Route(nameof(ChangePassword))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            int userId = GetUserId();
            User? user = await UnitOfWork.UserRepository.Find(a => a.Id == userId, trackChanges: true).SingleOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!PasswordHasher.VerifyPasswordHash(model.OldPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Invalid old password");
            }

            PasswordHasher.CreatePasswordHash(model.NewPassword, out string passwordHash, out string passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await UnitOfWork.UserRepository.SaveChangesAsync();

            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }
    }
}
