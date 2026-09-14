namespace HR.Application.Common.Interfaces;

using HR.Domain.Entities.Auth;

public interface IJwtService
{
    string GenerateAccessToken(User user, IEnumerable<string> permissions);
    RefreshToken GenerateRefreshToken(int userId, string ipAddress);
    int? ValidateJwtToken(string token);
}
