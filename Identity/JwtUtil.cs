using Entities.DBModels.Users;
using Identity.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utilities.RandomHelper;
using Utilities.Settings;

namespace Identity
{
    public class JwtUtil
    {
        private readonly AppSettings _appSettings;
        private readonly byte[] _secret;
        private readonly string _key;
        private readonly int _expires;

        public JwtUtil(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _secret = Encoding.UTF8.GetBytes(_appSettings.Secret);
            _key = "userId";
            _expires = _appSettings.TokenTTL;
        }

        public TokenResponse GenerateJwtToken(int id)
        {
            return GenerateJwtToken(id, null);
        }

        public TokenResponse GenerateJwtToken(int id, Dictionary<string, string>? claims)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            List<Claim> allClaims = new List<Claim>
            {
                new Claim(_key, id.ToString())
            };

            if (claims != null)
            {
                foreach (var pair in claims)
                {
                    // Claim() throws ArgumentNullException if value is null (e.g. Email for no-email parents)
                    allClaims.Add(new Claim(pair.Key, pair.Value ?? string.Empty));
                }
            }

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(allClaims),
                Expires = DateTime.UtcNow.AddMinutes(_expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponse(tokenHandler.WriteToken(token), tokenDescriptor.Expires ?? DateTime.UtcNow);
        }

        public string GenerateJwtToken(int id, int expires, Dictionary<string, string> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            List<Claim> allClaims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            // Add any additional custom claims passed in
            if (claims != null)
            {
                foreach (KeyValuePair<string, string> pair in claims)
                {
                    // Claim() throws ArgumentNullException if value is null (e.g. Email for no-email parents)
                    allClaims.Add(new Claim(pair.Key, pair.Value ?? string.Empty));
                }
            }

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(allClaims),
                Expires = DateTime.UtcNow.AddMinutes(expires),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_secret),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = "LoginServer",        // Optional but recommended
                Audience = claims?.GetValueOrDefault("aud") // Optional audience claim
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string? ValidateJwtToken(string token, string portal)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();

                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "LoginServer",
                    ValidateAudience = true,
                    ValidAudience = portal + "Portal",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                string refreshToken = jwtToken.Claims.First(x => x.Type == "refreshToken").Value;

                return refreshToken;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public JwtSecurityToken? ValidateJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();

                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public RefreshToken GenerateRefreshToken(int tenantId, int userId, string ipAddress)
        {
            byte[] randomBytes = RandomGenerator.GenerateBytes(64);

            RefreshToken refreshToken = new()
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiresAt = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenTTL),
                CreatedAt = DateTime.UtcNow,
                Fk_Tenant = tenantId,
                Fk_User = userId,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }

        public async Task RemoveOldTokens(User user)
        {
            int refreshTokenTTL = _appSettings.RefreshTokenTTL;

            var expiredTokens = user.RefreshTokens
                .Where(x => x.CreatedAt.AddDays(refreshTokenTTL) <= DateTime.UtcNow)
                .ToList();

            foreach (var token in expiredTokens)
            {
                user.RefreshTokens.Remove(token);
            }
        }


    }
}
