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
    public class ResourceServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Permission/Resource";
        private readonly string _serviceName;
        private readonly string _accessToken;

        public ResourceServices(ApiBase apiHub, string serviceName, string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<List<ResourceDto>>> GetResources(ResourceParameters parameters)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            return await _apiHub.GetAsync<List<ResourceDto>>(_serviceName, $"{_group}/{nameof(GetResources)}", queryParams, headers);
        }

        public async Task<ApiResponse<List<CustomLookUpDto>>> GetResourcesCustomLookup(ResourceParameters parameters)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            return await _apiHub.GetAsync<List<CustomLookUpDto>>(_serviceName, $"{_group}/{nameof(GetResourcesCustomLookup)}", queryParams, headers);
        }

        public async Task<ApiResponse<ResourceDto>> GetResourceById(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            return await _apiHub.GetAsync<ResourceDto>(_serviceName, $"{_group}/{nameof(GetResourceById)}", queryParams, headers);
        }

        public async Task<ApiResponse<ValidationResultDto>> CreateResource(ResourceCreateDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<ValidationResultDto>(_serviceName, $"{_group}/{nameof(CreateResource)}", model, headers: headers);
        }

        public async Task<ApiResponse<ValidationResultDto>> EditResource(int id, ResourceEditDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };

            return await _apiHub.PostAsync<ValidationResultDto>(_serviceName, $"{_group}/{nameof(EditResource)}", model, queryParams, headers: headers);
        }

        public async Task<ApiResponse<DataBoolenDto>> DeleteResource(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };

            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(DeleteResource)}", data: null, queryParams, headers: headers);
        }
    }
}
