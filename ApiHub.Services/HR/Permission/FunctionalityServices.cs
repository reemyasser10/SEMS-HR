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
    public class FunctionalityServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Permission/Functionality";
        private readonly string _serviceName;
        private readonly string _accessToken;

        public FunctionalityServices(ApiBase apiHub, string serviceName, string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<List<FunctionalityDto>>> GetFunctionalities(FunctionalityParameters parameters)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            return await _apiHub.GetAsync<List<FunctionalityDto>>(_serviceName, $"{_group}/{nameof(GetFunctionalities)}", queryParams, headers);
        }

        public async Task<ApiResponse<List<CustomLookUpDto>>> GetFunctionalitiesCustomLookup(FunctionalityParameters parameters)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            return await _apiHub.GetAsync<List<CustomLookUpDto>>(_serviceName, $"{_group}/{nameof(GetFunctionalitiesCustomLookup)}", queryParams, headers);
        }

        public async Task<ApiResponse<FunctionalityDto>> GetFunctionalityById(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            return await _apiHub.GetAsync<FunctionalityDto>(_serviceName, $"{_group}/{nameof(GetFunctionalityById)}", queryParams, headers);
        }

        public async Task<ApiResponse<ValidationResultDto>> CreateFunctionality(FunctionalityCreateDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<ValidationResultDto>(_serviceName, $"{_group}/{nameof(CreateFunctionality)}", model, headers: headers);
        }

        public async Task<ApiResponse<ValidationResultDto>> EditFunctionality(int id, FunctionalityEditDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            return await _apiHub.PostAsync<ValidationResultDto>(_serviceName, $"{_group}/{nameof(EditFunctionality)}", model, queryParams, headers: headers);
        }

        public async Task<ApiResponse<DataBoolenDto>> DeleteFunctionality(int id)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };

            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(DeleteFunctionality)}", data: null, queryParams, headers: headers);
        }
    }
}
