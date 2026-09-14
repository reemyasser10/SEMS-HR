using Shared.DTOs;
using Shared.DTOs.CommonModels;
using Utilities.Constants;
using Utilities.RequestHandler;

namespace ApiHub.Services.HR.Common
{
    public class LookupServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Common/Lookup";
        private readonly string _serviceName;
        private readonly string _accessToken;

        public LookupServices(ApiBase apiHub, string serviceName, string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        #region Lookup

        public async Task<ApiResponse<List<LookupDto>>> GetLookups(LookupParameters parameters)
        {
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.GetAsync<List<LookupDto>>(_serviceName, $"{_group}/{nameof(GetLookups)}", queryParams,
                headers);
        }

        public async Task<ApiResponse<List<CustomLookUpDto>>> GetCustomLookups(LookupParameters parameters)
        {
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.GetAsync<List<CustomLookUpDto>>(_serviceName, $"{_group}/{nameof(GetCustomLookups)}",
                queryParams, headers);
        }

        public async Task<ApiResponse<List<CustomLookUpDto>>> GetInvoiceStatusCustomLookups(int fk_InvoiceType,
            LookupParameters parameters)
        {
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);
            queryParams.Add("fk_InvoiceType", $"{fk_InvoiceType}");
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.GetAsync<List<CustomLookUpDto>>(_serviceName,
                $"{_group}/{nameof(GetInvoiceStatusCustomLookups)}", queryParams, headers);
        }

        public async Task<ApiResponse<LookupDto>> GetLookupById(int id)
        {
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.GetAsync<LookupDto>(_serviceName, $"{_group}/{nameof(GetLookupById)}", queryParams,
                headers);
        }

        public async Task<ApiResponse<LookupDto>> CreateLookup(CreateLookupDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<LookupDto>(_serviceName, $"{_group}/{nameof(CreateLookup)}", model, null,
                headers);
        }

        public async Task<ApiResponse<LookupDto>> EditLookup(int id, EditLookupDto model)
        {
            Dictionary<string, string> queryParams = new()
            {
                { "id", $"{id}" }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<LookupDto>(_serviceName, $"{_group}/{nameof(EditLookup)}", model,
                queryParams,
                headers);
        }

        public async Task<ApiResponse<DataBoolenDto>> DeleteLookup(int id)
        {
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(DeleteLookup)}", data: null,
                queryParams, headers);
        }

        #endregion
    }
}
