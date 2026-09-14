using Shared.DTOs;
using Shared.DTOs.CommonModels;
using Shared.DTOs.HR.PermissionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.RequestHandler;

namespace ApiHub.Services.HR.Permission
{
    public class RoleServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Permission/Role";
        private readonly string _serviceName;
        private readonly string _accessToken;

        public RoleServices(ApiBase apiHub, string serviceName, string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<List<RoleDto>>> GetRoles(RoleParameters parameters)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            return await _apiHub.GetAsync<List<RoleDto>>(_serviceName, $"{_group}/{nameof(GetRoles)}", queryParams, headers);
        }

        public async Task<ApiResponse<List<CustomLookUpDto>>> GetRolesCustomLookup(RoleParameters parameters)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            return await _apiHub.GetAsync<List<CustomLookUpDto>>(_serviceName, $"{_group}/{nameof(GetRolesCustomLookup)}", queryParams, headers);
        }
        public async Task<ApiResponse<RoleCreateDto>> GetRoleById(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            return await _apiHub.GetAsync<RoleCreateDto>(_serviceName, $"{_group}/{nameof(GetRoleById)}", queryParams, headers);
        }
        public async Task<ApiResponse<RoleDto>> CreateRole(RoleCreateDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<RoleDto>(_serviceName, $"{_group}/{nameof(CreateRole)}", model, headers: headers);
        }
        public async Task<ApiResponse<DataBoolenDto>> EditRole(int id, RoleEditDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };

            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(EditRole)}",  model, queryParams, headers: headers);
        }


        public async Task<ApiResponse<DataBoolenDto>> UpdateRolePermissions(UpdateRolePermissionsDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/UpdateRolePermissions", model, headers: headers);
        }
        public async Task<ApiResponse<DataBoolenDto>> DeleteRole(int id)
        {
            Dictionary<string, string> headers = new()
             {
                 { ApiConstants.AccessToken, _accessToken }
             };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };

            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(DeleteRole)}", data: null, queryParams, headers: headers);
        }
    }
}
