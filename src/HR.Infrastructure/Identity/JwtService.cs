namespace HR.Infrastructure.Identity;

using HR.Application.Common.Interfaces;
using HR.Domain.Entities.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;
    private readonly IConfiguration _configuration;

    public JwtService(IOptions<JwtSettings> settings, IConfiguration configuration)
    {
        _settings = settings.Value;
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenTTL = _configuration.GetValue<int>("AppSettings:tokenTTL", 60);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenTTL),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(int userId, string ipAddress)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshTokenTTL = _configuration.GetValue<int>("AppSettings:refreshTokenTTL", 14);

        return new RefreshToken
        {
            UserId = userId,
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(refreshTokenTTL),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    public int? ValidateJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Secret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            
            // Sub claim holds the user id based on GenerateAccessToken
            var userIdStr = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub || x.Type == ClaimTypes.NameIdentifier).Value;
            return int.Parse(userIdStr);
        }
        catch
        {
            return null;
        }
    }
}
