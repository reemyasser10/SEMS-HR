namespace HR.Api.Areas.Admin.Controllers;

using HR.Application.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _roleService.GetRolesAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleDto command)
    {
        var id = await _roleService.CreateRoleAsync(command);
        return CreatedAtAction(nameof(GetRoles), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto command)
    {
        if (id != command.Id) return BadRequest();
        var success = await _roleService.UpdateRoleAsync(command);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var success = await _roleService.DeleteRoleAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> AssignPermissions(int id, AssignPermissionsDto command)
    {
        if (id != command.RoleId) return BadRequest();
        var success = await _roleService.AssignPermissionsAsync(command);
        if (!success) return NotFound();
        return NoContent();
    }
}
