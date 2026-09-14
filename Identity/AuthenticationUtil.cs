using Entities.DBModels.Users;
using Entities.Enums;
using Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Utilities.Extensions;
using Utilities.RandomHelper;
using Utilities.Settings;

namespace Identity
{
    public class AuthenticationUtil
    {
        private const int MaxFailedAttempts = 5; // Lockout threshold
        private readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15); // Lockout time

        private User _user;
        private RefreshToken _refreshToken;
        private TokenResponse _jwtToken;

        private readonly UnitOfWork _unitOfWork;
        private readonly JwtUtil _jwtUtil;
        public AuthenticationUtil(
            UnitOfWork unitOfWork,
            JwtUtil jwtUtils)
        {
            _unitOfWork = unitOfWork;
            _jwtUtil = jwtUtils;
        }
        // Authenticate
        public async Task<AuthenticatedUserDto> Authenticate(int tenantId, string username, string password, string ipAddress,bool isSignUp = false)
        {
            if (username.IsEmpty() || !await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && (x.UserName == username || x.Email == username)).AnyAsync())
            {
                throw new Exception("User not found");
            }

            _user = await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && (x.UserName == username || x.Email == username), trackChanges: true).FirstAsync();

            if (_user.IsBan)
            {
                throw new Exception(username + " account is banned");

            }
            else if (_user.IsLockedOut)
            {
                if (_user.LastLoginFailedDate.HasValue &&
                    DateTime.UtcNow - _user.LastLoginFailedDate.Value > LockoutDuration)
                {
                    // Unlock after lockout duration
                    _user.IsLockedOut = false;
                    _user.FailedLoginAttempts = 0;
                }
                else
                {
                    throw new Exception(username + " account is locked out " + "for " + LockoutDuration + " minutes");
                }
            }
            else if (!_user.IsActive)
            {
                throw new Exception(username + " account is inactive");
            }
            else if (!isSignUp && !_user.IsEmailVerified && !_user.IsPhoneVerified)
            {
                throw new Exception(username  +" not verified");
            }

            if (!PasswordHasher.VerifyPasswordHash(password, _user.PasswordHash, _user.PasswordSalt))
            {
                _user.FailedLoginAttempts++;
                _user.LastLoginFailedDate = DateTime.UtcNow;

                if (_user.FailedLoginAttempts >= MaxFailedAttempts)
                {
                    _user.IsLockedOut = true; // Lock the user
                }

                _ = await _unitOfWork.SaveChangesAsync();

                throw new Exception("Password is incorrect");
            }

            await AuthenticationSucceeded("UserName & Password");
            await GetRefreshToken(tenantId, ipAddress);
            return await GetAuthenticatedUser(tenantId);
        }
        public async Task<AuthenticatedUserDto> Authenticate(int tenantId, ExternalLoginProviderEnum provider, string providerKey, string ipAddress)
        {
            if (providerKey.IsEmpty() || !await _unitOfWork.ExternalLoginRepository.Find(x => x.Fk_Tenant == tenantId && x.Provider == provider && x.ProviderKey == providerKey).AnyAsync())
            {
                throw new Exception("User not found");
            }

            int userId = await _unitOfWork.ExternalLoginRepository.Find(x => x.Fk_Tenant == tenantId && x.Provider == provider && x.ProviderKey == providerKey).Select(x => x.Fk_User).FirstAsync();

            _user = await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && x.Id == userId, trackChanges: true).FirstAsync();

            await AuthenticationSucceeded("External Provider");

            if (!_user.IsActive)
            {
                throw new Exception(_user.UserName + " account is inactive");
            }

            await GetRefreshToken(tenantId, ipAddress);
            return await GetAuthenticatedUser(tenantId);
        }
        public async Task<AuthenticatedUserDto> Authenticate(int tenantId, string token)
        {
            if (!await _unitOfWork.RefreshTokenRepository.Find(x => x.Fk_Tenant == tenantId &&
                                                                    x.Token == token &&
                                                                    !x.IsRevoked &&
                                                                    x.ExpiresAt > DateTime.UtcNow).AnyAsync())
            {
                throw new Exception("Invalid token");
            }

            _refreshToken = await _unitOfWork.RefreshTokenRepository.Find(x => x.Fk_Tenant == tenantId &&
                                                                               x.Token == token &&
                                                                               !x.IsRevoked &&
                                                                               x.ExpiresAt > DateTime.UtcNow).FirstAsync();

            _user = await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && x.Id == _refreshToken.Fk_User, trackChanges: true).FirstAsync();

            await AuthenticationSucceeded("Refresh Token");

            if (!_user.IsActive)
            {
                throw new Exception(_user.UserName + " account is inactive");
            }

            return await GetAuthenticatedUser(tenantId);
        }
        // Get Info
        public async Task<UserDto> UserDto(int? tenantId, int userId)
        {
            return !await IsExist(tenantId, userId)
                ? throw new Exception("User not found")
                : await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && x.Id == userId).Select(x => new UserDto
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    Phone = x.Phone,
                    IsEmailVerified = x.IsEmailVerified,
                    IsPhoneVerified = x.IsPhoneVerified,
                }).FirstAsync();
        }
        public async Task<string?> UserName(int? tenantId, int userId)
        {
            return !await IsExist(tenantId, userId)
                ? throw new Exception("User not found")
                : await _unitOfWork.UserRepository
                                    .Find(x => x.Fk_Tenant == tenantId && x.Id == userId)
                                    .Select(selector: x => x.FullName)
                                    .FirstAsync();
        }
        public async Task<string?> UserName(int? tenantId, string userName)
        {
            return !await IsExist(tenantId, userName)
                ? throw new Exception("User not found")
                : await _unitOfWork.UserRepository
                                    .Find(x => x.Fk_Tenant == tenantId && (x.UserName == userName || x.Email == userName))
                                    .Select(selector: x => x.FullName)
                                    .FirstAsync();
        }
        public async Task<(string FullName, bool IsEmailVerified)> GetUserInfo(int? tenantId, string userName)
        {
            if (!await IsExist(tenantId, userName))
                throw new Exception("User not found");

            var user = await _unitOfWork.UserRepository
                .Find(x => x.Fk_Tenant == tenantId && (x.UserName == userName || x.Email == userName))
                .Select(x => new { x.FullName, x.IsEmailVerified })
                .FirstAsync();

            return (user.FullName, user.IsEmailVerified);
        }

        public async Task<int?> GetUserIdByUserName(int? tenantId, string userName)
        {
            return !await IsExist(tenantId, userName)
                ? throw new Exception("User not found")
                : await _unitOfWork.UserRepository
                                    .Find(x => x.Fk_Tenant == tenantId && (x.UserName == userName || x.Email == userName))
                                    .Select(selector: x => x.Id)
                                    .FirstAsync();
        }

        public async Task<bool> IsExist(int? tenantId, int userId)
        {
            return await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && x.Id == userId).AnyAsync();
        }
        public async Task<bool> IsExist(int? tenantId, string userName)
        {
            return await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && (x.UserName == userName || x.Email == userName)).AnyAsync();
        }
        public async Task<bool> IsEmailExist(int? tenantId, string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && x.Email == email).AnyAsync();
        }
        public async Task<bool> IsActive(int? tenantId, int userId)
        {
            return await _unitOfWork.UserRepository.Find(x => x.Fk_Tenant == tenantId && x.Id == userId && x.IsActive).AnyAsync();
        }

        public bool CheckVerificationCodeExist(string code)
        {
            string token = PasswordHasher.CreateCodeHash(code);

            return _unitOfWork.VerificationRepository.Find(a => a.Token == token).Any();
        }
        public async Task<string> CreateVerification(int tenantId, int userId, int verificationTTL, string ipAddress)
        {
            string code = "";
            do
            {
                code = RandomGenerator.GenerateInteger(length: 6, minVal: 111111, maxVal: 999999).ToString();
            }

            while (CheckVerificationCodeExist(code));

            string token = PasswordHasher.CreateCodeHash(code);

            Verification? lastUserVerification = await _unitOfWork.VerificationRepository
                                                  .Find(a => a.Fk_User == userId && a.Fk_Tenant == tenantId)
                                                  .OrderByDescending(a => a.CreatedAt)
                                                  .FirstOrDefaultAsync();

            await _unitOfWork.VerificationRepository.AddAsync(new Verification
            {
                Fk_Tenant = tenantId,
                Fk_User = userId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(verificationTTL),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                Token = token
            });
            _ = await _unitOfWork.SaveChangesAsync();

            return code;
        }

        public async Task VerifyEmail(int tenantId, int userId, string code, int verificationTTL)
        {
            Verification userVerification = await _unitOfWork.VerificationRepository.Find(a => a.Fk_Tenant == tenantId
                                                                   && a.Fk_User == userId, trackChanges: true)
                                                                   .OrderByDescending(a => a.CreatedAt)
                                                                   .FirstAsync();

            if (userVerification == null || userVerification.IsUsed || userVerification.IsExpired || !PasswordHasher.VerifyCodeHash(code, userVerification.Token))
            {
                throw new Exception("Invalid code");
            }
            else
            {
                userVerification.IsUsed = true;

                _ = await _unitOfWork.SaveChangesAsync();

                User userDB = await _unitOfWork.UserRepository.Find(a => a.Id == userId, trackChanges: true).SingleAsync();

                userDB.IsEmailVerified = true;

                _ = await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task<string> GetRefreshToken(int tenantId, int userId)
        {
            return await _unitOfWork.RefreshTokenRepository.Find(x => x.Fk_Tenant == tenantId &&
                                                                      x.Fk_User == userId &&
                                                                      !x.IsRevoked &&
                                                                      x.ExpiresAt > DateTime.UtcNow)
                                                           .Select(a => a.Token)
                                                           .FirstAsync();
        }
        // Helper methods
        private async Task AuthenticationSucceeded(string lastLoginBy)
        {
            _user.LastLoginDate = DateTime.UtcNow;
            _user.FailedLoginAttempts = 0;
            _user.LastLoginBy = lastLoginBy;

            _ = await _unitOfWork.SaveChangesAsync();

            _jwtToken = _jwtUtil.GenerateJwtToken(_user.Id);
        }

        public async Task RevokeToken(int tenantId, string token)
        {
            if (!await _unitOfWork.RefreshTokenRepository.Find(a => a.Token == token &&
                                                               a.Fk_Tenant == tenantId &&
                                                               a.ExpiresAt > DateTime.UtcNow &&
                                                               !a.IsRevoked).AnyAsync())
            {
                throw new Exception("Invalid Token");
            }

            var refreshToken = await _unitOfWork.RefreshTokenRepository.Find(a => a.Token == token &&
                                                           a.Fk_Tenant == tenantId &&
                                                           a.ExpiresAt > DateTime.UtcNow &&
                                                           !a.IsRevoked, trackChanges: true).FirstOrDefaultAsync();

            _user = await _unitOfWork.UserRepository.Find(a => a.Id == refreshToken.Fk_User)
                                                    .Include(a => a.RefreshTokens).SingleOrDefaultAsync();

            refreshToken.IsRevoked = true;

            _jwtUtil.RemoveOldTokens(_user);

            await _unitOfWork.SaveChangesAsync();
        }


        private async Task<AuthenticatedUserDto> GetAuthenticatedUser(int? tenantId)
        {
            var adminAccess = await _unitOfWork.PermissionRepository.GetAdministratorAccessAsync(_user.Id);
            var isAdmin = adminAccess.IsAdmin; 


            var claims = new Dictionary<string, string>();
          
           
            // Update JWT token with newly fetched claims
            _jwtToken = _jwtUtil.GenerateJwtToken(_user.Id, claims);

            return new AuthenticatedUserDto
            {
                User = new UserDto
                {
                    UserId = _user.Id,
                    FullName = _user.FullName,
                    Email = _user.Email,
                    Phone = _user.Phone,
                    IsEmailVerified = _user.IsEmailVerified,
                    IsPhoneVerified = _user.IsPhoneVerified,
                    IsAdmin = isAdmin,
                    TenantId = tenantId,
                },
                Token = _jwtToken ?? null,
                RefreshToken = _refreshToken == null ? null : new TokenResponse(_refreshToken.Token, _refreshToken.ExpiresAt)
            };
        }
        private async Task GetRefreshToken(int tenantId, string ipAddress)
        {
            if (await _unitOfWork.RefreshTokenRepository.Find(x => x.Fk_Tenant == tenantId &&
                                                                   x.Fk_User == _user.Id &&
                                                                   !x.IsRevoked &&
                                                                   x.ExpiresAt > DateTime.UtcNow).AnyAsync())
            {
                _refreshToken = await _unitOfWork.RefreshTokenRepository.Find(x => x.Fk_Tenant == tenantId &&
                                                                                    x.Fk_User == _user.Id &&
                                                                                    !x.IsRevoked &&
                                                                                    x.ExpiresAt > DateTime.UtcNow).FirstAsync();
            }
            else
            {
                _refreshToken = _jwtUtil.GenerateRefreshToken(tenantId, _user.Id, ipAddress);
                await _unitOfWork.RefreshTokenRepository.AddAsync(_refreshToken);
                _ = await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
