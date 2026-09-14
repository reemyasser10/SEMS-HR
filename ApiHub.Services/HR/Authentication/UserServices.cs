using Azure.Core;
using Entities.Enums;
using Identity.Entities;
using Shared.DTOs;
using Shared.DTOs.HR.PermissionModels;
using Shared.DTOs.SharedModels;
using Utilities.Constants;

namespace ApiHub.Services.HR.Authentication
{
    public class UserServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Authentication/User";
        private readonly string _serviceName;
        private readonly string _accessToken;


        public UserServices(ApiBase apiHub, string serviceName, string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<UserDto>> GetUserProfile(string authToken)
        {
            return await _apiHub.GetAsync<UserDto>(_serviceName, $"{_group}/{nameof(GetUserProfile)}", headers: new Dictionary<string, string>
            {
                {HeadersConstants.AuthorizationToken, authToken }
            });
        }

        public async Task<ApiResponse<DataDto>> GetShortLink(UserShortLinkDto user, string authToken)
        {
            return await _apiHub.PostAsync<DataDto>(_serviceName, $"{_group}/{nameof(GetShortLink)}", user, headers: new Dictionary<string, string>
            {
                {HeadersConstants.AuthorizationToken, authToken }
            });
        }

        public async Task<ApiResponse<UserDto>> EditUser(int id,EditUserDto user)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken,_accessToken }
            };

            Dictionary<string, string> querParam = new()
            {
                { "id", $"{id}" },
                {"applicationEnum",$"{ApplicationEnum.WebApp_AdminPortal}" }
            };
            return await _apiHub.PostAsync<UserDto>(_serviceName, $"{_group}/{nameof(EditUser)}", user,querParam,headers);
        }

        public async Task<ApiResponse<UserPermissionCacheDto>> GetUserPermissions(int? tenantId, string accessToken)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, accessToken },
                { ApiConstants.TenantId, $"{tenantId}" },
            };
            return await _apiHub.GetAsync<UserPermissionCacheDto>(_serviceName, $"{_group}/{nameof(GetUserPermissions)}", queryParams: null, headers);
        }
    }
}
