using ApiHub.Services;
using ApiHub.Services.HR;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Utilities.Extensions;
using WebApp.Auth.Areas.HR.Models;
using WebApp.Auth.Controllers;

namespace WebApp.Auth.Areas.HR.Controllers
{
    [Area("HR")]
    public class LoginController : ExtendControllerBase
    {
        private readonly HRServices _HRServices;

        public LoginController(IHttpClientFactory httpClient, ApiHubService apiHub) : base(httpClient, apiHub)
        {
            _HRServices = new HRServices(_apiBase);
        }

        public IActionResult Index()
        {
            ViewBag.Error = TempData["LoginError"];
            return View(new CheckUserVM
            {
                UserName = TempData["UserName"]?.ToString() ?? string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(CheckUserVM user)
        {
            ApiResponse<DataDto> checkUser = await _HRServices.LoginServices.CheckUserExist(new CheckUserExistDto { UserName = user.UserName });
            if (checkUser.IsSuccess)
            {
                return RedirectToAction(nameof(Password), new { userName = user.UserName });
            }

            TempData["LoginError"] = checkUser.Status?.ErrorMessage.FromBase64();
            TempData["UserName"] = user.UserName;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Password(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = TempData["PasswordError"];
            ViewBag.NotVerified = TempData["NotVerified"] is true;

            return View(new UserLoginVM
            {
                UserName = userName,
                Password = string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(UserLoginVM user)
        {
            ApiResponse<UserDto> checkUser = await _HRServices.LoginServices.LoginByUserName(new UserLoginDto { UserName = user.UserName, Password = user.Password });

            if (checkUser.IsSuccess)
            {
              if ( checkUser.Data.IsAdmin)
                {
                    ApiResponse<DataDto> shortUrl = await _HRServices.UserServices.GetShortLink(new UserShortLinkDto { Role = "Admin" }, checkUser.Authorization);
                    return RedirectToDestination(shortUrl.Data.Value.ToString());
                }
                else
                {
                    TempData["RoleUserName"] = user.UserName;
                    TempData["RoleToken"] = checkUser.Authorization;
                    TempData["IsAdmin"] = checkUser.Data.IsAdmin;

                    return RedirectToAction(nameof(Role));
                }
            }
            else
            {
                string error = checkUser.Status?.ErrorMessage.FromBase64() ?? "";
                if (error.Contains("not verified", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["NotVerified"] = true;
                    TempData["PasswordError"] = "Your account is not verified yet.";
                }
                else
                {
                    TempData["PasswordError"] = error;
                }

                return RedirectToAction(nameof(Password), new { userName = user.UserName });
            }
        }

        [HttpGet]
        public IActionResult Role()
        {
            string? userName = TempData["RoleUserName"]?.ToString();
            string? token = TempData["RoleToken"]?.ToString();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new SelectRoleVM
            {
                UserName = userName,
                Token = token,
                Role = string.Empty,
                IsParent = TempData["IsParent"] is true,
                IsTeacher = TempData["IsTeacher"] is true,
                IsAdmin = TempData["IsAdmin"] is true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Role(SelectRoleVM user)
        {
            ApiResponse<DataDto> shortUrl = await _HRServices.UserServices.GetShortLink(new UserShortLinkDto { Role = user.Role }, user.Token);

            return RedirectToDestination(shortUrl.Data.Value.ToString());
        }

        [HttpGet]
        public IActionResult Continue()
        {
            string? destinationUrl = TempData["DestinationUrl"]?.ToString();

            return string.IsNullOrWhiteSpace(destinationUrl)
                ? RedirectToAction(nameof(Index))
                : Redirect(destinationUrl);
        }

        private IActionResult RedirectToDestination(string destinationUrl)
        {
            TempData["DestinationUrl"] = destinationUrl;
            return RedirectToAction(nameof(Continue));
        }
    }
}
