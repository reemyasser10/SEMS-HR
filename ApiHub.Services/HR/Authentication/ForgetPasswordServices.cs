using Identity.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace ApiHub.Services.HR.Authentication
{
   public class ForgetPasswordServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Authentication/ResetUser";
        private readonly string _serviceName;
        private readonly string? _accessToken;
        
        public ForgetPasswordServices(ApiBase apiHub, string serviceName, string? accessToken = null)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<DataDto>> ForgetPassword(ForgetPasswordDto user)
        {
            return await _apiHub.PostAsync<DataDto>(_serviceName, $"{_group}/{nameof(ForgetPassword)}", user);
        }

        public async Task<ApiResponse<DataBoolenDto>> ResetPassword(ResetPasswordDto user)
        {
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(ResetPassword)}", user);
        }

        public async Task<ApiResponse<DataBoolenDto>> ChangePassword(ChangePasswordDto user)
        {
            Dictionary<string, string> headers = null;
            if (!string.IsNullOrEmpty(_accessToken))
            {
                headers = new Dictionary<string, string>
                {
                    { ApiConstants.AccessToken, _accessToken }
                };
            }
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(ChangePassword)}", user, null, headers);
        }
    }
}
