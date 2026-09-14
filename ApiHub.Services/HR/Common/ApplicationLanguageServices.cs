using Entities.DBModels.Tenants;
using Shared.DTOs.CommonModels;
using Utilities.Constants;
using Utilities.RequestHandler;

namespace ApiHub.Services.Common
{
    public class ApplicationLanguageServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Common/ApplicationLanguage";
        private readonly string _serviceName;

        public ApplicationLanguageServices(ApiBase apiHub, string serviceName)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
        }

        #region Lookup

        public async Task<ApiResponse<List<ApplicationLanguageDto>>> GetApplicationLanguages(ApplicationLanguageParameters parameters,int tenantId)
        {
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            Dictionary<string, string> headers = new()
            {
                { ApiConstants.TenantId, $"{tenantId}" }
            };
            return await _apiHub.GetAsync<List<ApplicationLanguageDto>>(_serviceName, $"{_group}/{nameof(GetApplicationLanguages)}", queryParams,headers);
        }

        public async Task<ApiResponse<List<ApplicationLanguageDto>>> GetApplicationLanguages(ApplicationLanguageParameters parameters)
        {
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

          
            return await _apiHub.GetAsync<List<ApplicationLanguageDto>>(_serviceName, $"{_group}/{nameof(GetApplicationLanguages)}", queryParams, headers:null);
        }


        #endregion

    }
}
