using ApiHub.Services;
using ApiHub.Services.HR;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Threading.Tasks;
using Utilities.Extensions;
using WebApp.Auth.Areas.HR.Models;
using WebApp.Auth.Controllers;

namespace WebApp.Auth.Areas.HR.Controllers
{
    [Area("HR")]
    public class ForgetPasswordController : ExtendControllerBase
    {
        private readonly HRServices _HRServices;
        public ForgetPasswordController(IHttpClientFactory httpClient, ApiHubService apiHub) : base(httpClient, apiHub)
        {
            _HRServices = new HRServices(_apiBase);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ForgetPasswordDto user)
        {
           ApiResponse<DataDto> response = await _HRServices.ForgetPasswordServices
                .ForgetPassword(user);
          
            return Ok(response);
        }

        [HttpGet]
        public IActionResult ForgetPassword(string userName)
        {
            ResetPasswordDto model = new ResetPasswordDto
            {
                Email = userName,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };
            return View(model);
        }

        [HttpPost]
        public  IActionResult ForgetPassword(ResetPasswordDto user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
           DataBoolenDto result =  _HRServices.ForgetPasswordServices
                .ResetPassword(user).Result.Data;
            if (result.Value.Value)
                return RedirectToAction("Index", "Login");
            else 
                return View(user);


        }
    }
}
