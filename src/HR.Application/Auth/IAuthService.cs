namespace HR.Application.Auth;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto request, CancellationToken cancellationToken = default);
    Task<AuthResult> RegisterAdminAsync(RegisterDto request, CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(LoginDto request, string ipAddress, CancellationToken cancellationToken = default);
    Task<AuthResult> RefreshTokenAsync(string token, string ipAddress, CancellationToken cancellationToken = default);
    Task RevokeTokenAsync(string token, string ipAddress, CancellationToken cancellationToken = default);
}
