namespace HR.Application.Auth;

using HR.Application.Common.Interfaces;
using HR.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IApplicationDbContext _context; // Required for complex includes until repository handles includes

    public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService, IApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDto request, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.Users.FindAsync(u => u.Username == request.Username || u.Email == request.Email, cancellationToken);
        if (existing.Any())
        {
            return new AuthResult { Success = false, Errors = new[] { "User already exists." } };
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var token = _jwtService.GenerateAccessToken(user, Array.Empty<string>());
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id, "0.0.0.0");
        
        await _unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AuthResult { Success = true, Token = token, RefreshToken = refreshToken.Token };
    }

    public async Task<AuthResult> RegisterAdminAsync(RegisterDto request, CancellationToken cancellationToken = default)
    {
        var result = await RegisterAsync(request, cancellationToken);
        if (!result.Success) return result;

        var user = await _unitOfWork.Users.FindAsync(u => u.Username == request.Username, cancellationToken);
        var createdUser = user.FirstOrDefault();

        if (createdUser != null)
        {
            await _unitOfWork.Administrators.AddAsync(new Administrator { UserId = createdUser.Id }, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            result.IsAdmin = true;
        }

        return result;
    }

    public async Task<AuthResult> LoginAsync(LoginDto request, string ipAddress, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null || !user.IsActive || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return new AuthResult { Success = false, Errors = new[] { "Invalid username or password." } };
        }

        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToList();

        var token = _jwtService.GenerateAccessToken(user, permissions);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id, ipAddress);
        
        await _unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var isAdmin = await _context.Administrators.AnyAsync(a => a.UserId == user.Id, cancellationToken);

        return new AuthResult { Success = true, Token = token, RefreshToken = refreshToken.Token, IsAdmin = isAdmin };
    }

    public async Task<AuthResult> RefreshTokenAsync(string token, string ipAddress, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(t => t.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .SingleOrDefaultAsync(t => t.Token == token, cancellationToken);

        if (refreshToken == null || !refreshToken.IsActive)
        {
            return new AuthResult { Success = false, Errors = new[] { "Invalid token" } };
        }

        var newRefreshToken = _jwtService.GenerateRefreshToken(refreshToken.UserId, ipAddress);
        
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReplacedByToken = newRefreshToken.Token;

        var permissions = refreshToken.User.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToList();

        var jwtToken = _jwtService.GenerateAccessToken(refreshToken.User, permissions);

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        _unitOfWork.RefreshTokens.Update(refreshToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AuthResult { Success = true, Token = jwtToken, RefreshToken = newRefreshToken.Token };
    }

    public async Task RevokeTokenAsync(string token, string ipAddress, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.RefreshTokens.FindAsync(t => t.Token == token, cancellationToken);
        var refreshToken = existing.SingleOrDefault();

        if (refreshToken == null || !refreshToken.IsActive) throw new Application.Common.Exceptions.NotFoundException("InvalidToken");

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = "Revoked by user request";

        _unitOfWork.RefreshTokens.Update(refreshToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
