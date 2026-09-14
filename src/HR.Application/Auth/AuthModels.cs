namespace HR.Application.Auth;

public class RegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResult
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string[] Errors { get; set; } = Array.Empty<string>();
    public bool IsAdmin { get; set; }
}

public class TokenResponse
{
    public string JwtToken { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public override string ToString() => JwtToken; // For simplicity in SetRefresh
}

public class RefreshTokenDto
{
    public string Token { get; set; } = string.Empty;
}
