using Identity.Entities;

namespace ApiHub.Services.HR.Authentication
{
    public class RegisterServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Authentication/Register";

        private readonly string _serviceName;

        public RegisterServices(ApiBase apiHub, string serviceName)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
        }

        public async Task<ApiResponse<UserDto>> Register(UserRegistrationDto user)
        {
            return await _apiHub.PostAsync<UserDto>(_serviceName, $"{_group}/{nameof(Register)}", user);
        }
    }
}
