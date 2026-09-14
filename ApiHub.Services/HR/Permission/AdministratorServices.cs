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
    public class AdministratorServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Permission/Administrator";
        private readonly string _serviceName;
        private readonly string _accessToken;

        public AdministratorServices(ApiBase apiHub, string serviceName, string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<List<AdministratorDto>>> GetAdministrators(UserParameters parameters)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            return await _apiHub.GetAsync<List<AdministratorDto>>(_serviceName, $"{_group}/{nameof(GetAdministrators)}", queryParams, headers);
        }

        public async Task<ApiResponse<AdministratorDto>> GetAdministratorById(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            return await _apiHub.GetAsync<AdministratorDto>(_serviceName, $"{_group}/{nameof(GetAdministratorById)}", queryParams, headers);
        }

        public async Task<ApiResponse<AdministratorEditDto>> GetAdministratorEditDto(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            return await _apiHub.GetAsync<AdministratorEditDto>(_serviceName, $"{_group}/{nameof(GetAdministratorEditDto)}", queryParams, headers);
        }

        public async Task<ApiResponse<DataBoolenDto>> CreateAdministrator(AdministratorCreateDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(CreateAdministrator)}", model, headers: headers);
        }

        public async Task<ApiResponse<ValidationResultDto>> EditAdministrator(int id, AdministratorEditDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };

            return await _apiHub.PostAsync<ValidationResultDto>(_serviceName, $"{_group}/{nameof(EditAdministrator)}", model, queryParams, headers: headers);
        }

        public async Task<ApiResponse<DataBoolenDto>> DeleteAdministrator(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };

            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(DeleteAdministrator)}", data: null, queryParams, headers: headers);
        }
    }
}
