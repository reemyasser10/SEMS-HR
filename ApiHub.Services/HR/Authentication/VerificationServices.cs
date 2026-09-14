using Identity.Entities;
using Shared.DTOs;
using Utilities.ActionFilters;
using Utilities.Constants;

namespace ApiHub.Services.HR.Authentication
{
    public class VerificationServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Authentication/Verification";

        private readonly string _serviceName;

        public VerificationServices(ApiBase apiHub, string serviceName)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
        }
        [AllowAnonymous]
        public async Task<ApiResponse<DataDto>> GetVerificationEmailCode(EmailVerificationDto model)
        {
            return await _apiHub.PostAsync<DataDto>(_serviceName, $"{_group}/{nameof(GetVerificationEmailCode)}", model);
        }
        [AllowAnonymous]

        public async Task<ApiResponse<DataDto>> VerifyEmail( UserVerificationDto userVerification)
        {
            return await _apiHub.PostAsync<DataDto>(_serviceName, $"{_group}/{nameof(VerifyEmail)}", userVerification);
        }
    }
}
