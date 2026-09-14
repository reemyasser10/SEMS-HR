namespace HR.Api.Areas.Auth.Controllers;

using HR.Api.Utilities;
using HR.Application.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Auth")]
[Route("api/[area]/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto command)
    {
        var result = await _authService.RegisterAsync(command);
        if (!result.Success)
            return BadRequest(new { errors = result.Errors });

        var tokenResponse = new TokenResponse { JwtToken = result.Token, Expires = DateTime.UtcNow.AddMinutes(60) };
        var refreshResponse = new TokenResponse { JwtToken = result.RefreshToken, Expires = DateTime.UtcNow.AddDays(14) };
        
        SetToken(tokenResponse);
        SetRefresh(refreshResponse);
        return Ok(new { message = "Registration successful" });
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto command)
    {
        var result = await _authService.RegisterAdminAsync(command);
        if (!result.Success)
            return BadRequest(new { errors = result.Errors });

        var tokenResponse = new TokenResponse { JwtToken = result.Token, Expires = DateTime.UtcNow.AddMinutes(60) };
        var refreshResponse = new TokenResponse { JwtToken = result.RefreshToken, Expires = DateTime.UtcNow.AddDays(14) };
        
        SetToken(tokenResponse);
        SetRefresh(refreshResponse);
        return Ok(new { isAdmin = result.IsAdmin });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto command)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var result = await _authService.LoginAsync(command, ipAddress);
        
        if (!result.Success)
            return BadRequest(new { errors = result.Errors });

        var tokenResponse = new TokenResponse { JwtToken = result.Token, Expires = DateTime.UtcNow.AddMinutes(60) };
        var refreshResponse = new TokenResponse { JwtToken = result.RefreshToken, Expires = DateTime.UtcNow.AddDays(14) };
        
        SetToken(tokenResponse);
        SetRefresh(refreshResponse);
        return Ok(new { isAdmin = result.IsAdmin });
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto request)
    {
        if (string.IsNullOrEmpty(request.Token))
            return BadRequest(new { message = "Refresh token is required" });

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var result = await _authService.RefreshTokenAsync(request.Token, ipAddress);
        
        if (!result.Success)
            return BadRequest(new { errors = result.Errors });

        var tokenResponse = new TokenResponse { JwtToken = result.Token, Expires = DateTime.UtcNow.AddMinutes(60) };
        var refreshResponse = new TokenResponse { JwtToken = result.RefreshToken, Expires = DateTime.UtcNow.AddDays(14) };
        
        SetToken(tokenResponse);
        SetRefresh(refreshResponse);
        return Ok(new { message = "Token refreshed successfully" });
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenDto request)
    {
        if (string.IsNullOrEmpty(request.Token))
            return BadRequest(new { message = "Refresh token is required" });

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var result = await _authService.RevokeTokenAsync(request.Token, ipAddress);

        if (!result)
            return NotFound(new { message = "Token not found" });

        return Ok(new { message = "Token revoked" });
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword([FromBody] string email)
    {
        return Ok(new { message = "Please check your email for password reset instructions" });
    }

    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] string token)
    {
        return Ok(new { message = "Password reset successful" });
    }

    protected void SetRefresh(TokenResponse token)
    {
        Response.Headers.Append(HeadersConstants.SetRefresh, token.ToString());
    }

    protected void SetToken(TokenResponse token)
    {
        Response.Headers.Append(HeadersConstants.Expires, token.Expires.ToString("R")); // RFC1123 pattern, similar to CommaEncode
        Response.Headers.Append(HeadersConstants.Authorization, token.JwtToken);
    }
}
