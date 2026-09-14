using ApiHub.Services;
using ApiHub.Services.HR;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using WebApp.Auth.Controllers;

namespace WebApp.Auth.Areas.HR.Controllers
{
    [Area("HR")]
    public class RegisterController : ExtendControllerBase
    {
        private readonly HRServices _HRServices;

        public RegisterController(IHttpClientFactory httpClient, ApiHubService apiHub) : base(httpClient, apiHub)
        {
            _HRServices = new HRServices(_apiBase);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Verify(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto user)
        {
            user.Email = user.UserName;

            ApiResponse<UserDto> result = await _HRServices.RegisterServices.Register(user);

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail([FromBody] UserVerificationDto verificationDto)
        {
            ApiResponse<DataDto> result = await _HRServices.VerificationServices.VerifyEmail(verificationDto);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResendVerification([FromBody] EmailVerificationDto model)
        {
            ApiResponse<DataDto> result = await _HRServices.VerificationServices.GetVerificationEmailCode(model);
            return Json(result);

        }
    }
}
