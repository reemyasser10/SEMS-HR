namespace HR.Api.Areas.Admin.Controllers;

using HR.Application.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissions()
    {
        return Ok(await _permissionService.GetPermissionsAsync());
    }
}
