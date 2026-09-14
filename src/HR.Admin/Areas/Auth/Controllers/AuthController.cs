using Microsoft.AspNetCore.Mvc;

namespace HR.Admin.Controllers;

[Area("Auth")]
public class AuthController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // TODO: Call API endpoint to authenticate and redirect
        // For now, simple mock redirect to Home
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        return RedirectToAction("Login");
    }
}
