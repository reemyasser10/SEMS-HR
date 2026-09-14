using Identity.Entities;
using Shared.DTOs;

namespace ApiHub.Services.HR.Authentication
{
    public class LoginServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Authentication/Login";
        private readonly string _serviceName;

        public LoginServices(ApiBase apiHub, string serviceName)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
        }

        public async Task<ApiResponse<DataDto>> CheckUserExist(CheckUserExistDto user)
        {
            return await _apiHub.PostAsync<DataDto>(_serviceName, $"{_group}/{nameof(CheckUserExist)}", user);
        }

        public async Task<ApiResponse<UserDto>> LoginByUserName(UserLoginDto user)
        {
            return await _apiHub.PostAsync<UserDto>(_serviceName, $"{_group}/{nameof(LoginByUserName)}", user);
        }

        public async Task<ApiResponse<UserDto>> LoginByExternalProvider(ExternalLoginProviderDto user)
        {
            return await _apiHub.PostAsync<UserDto>(_serviceName, $"{_group}/{nameof(LoginByExternalProvider)}", user);
        }

        public async Task<ApiResponse<UserDto>> RefreshToken(UserRefreshTokenDto user)
        {
            return await _apiHub.PostAsync<UserDto>(_serviceName, $"{_group}/{nameof(RefreshToken)}", user);
        }
        public async Task<ApiResponse<DataBoolenDto>> RevokeToken(UserRefreshTokenDto user)
        {
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(RevokeToken)}", user);
        }

        public async Task<ApiResponse<UserDto>> ValidateToken(TokenValidationRequestDto user)
        {
            return await _apiHub.PostAsync<UserDto>(_serviceName, $"{_group}/{nameof(ValidateToken)}", user);
        }

        public async Task<ApiResponse<DataDto>> GetLoginLink()
        {
            return await _apiHub.GetAsync<DataDto>(_serviceName, $"{_group}/{nameof(GetLoginLink)}");
        }
    }
}
