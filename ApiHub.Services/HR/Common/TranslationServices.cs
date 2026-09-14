using Entities.Enums;
using Shared.DTOs;
using Shared.DTOs.CommonModels;
using Utilities.Constants;
using Utilities.RequestHandler;

namespace ApiHub.Services.HR.Common
{
    public class TranslationServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Common/Translation";
        private readonly string _serviceName;
        private readonly string _accessToken;

        public TranslationServices(ApiBase apiHub, string serviceName, string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<Dictionary<string, string>>> GetTranslations(ApplicationEnum applicationEnum,string culture = "en")
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                {"applicationEnum", ((int)applicationEnum).ToString() }
            };

            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken,_accessToken },
                {ApiConstants.Culture , culture }
            };

            return await _apiHub.GetAsync<Dictionary<string, string>>(_serviceName, $"{_group}/{nameof(GetTranslations)}", queryParams,headers);
        }
        public async Task<ApiResponse<List<TranslationDto>>> GetTranslationsPaged(TranslationParameters parameters)
        {
            Dictionary<string, string> queryParams = QueryParameterHelper.ToQueryParameters(parameters);

            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken,_accessToken }
            };
            return await _apiHub.GetAsync<List<TranslationDto>>(_serviceName, $"{_group}/{nameof(GetTranslationsPaged)}", queryParams,headers);
        }
        public async Task<ApiResponse<TranslationDto>> GetTranslationById(int id)
        {
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken,_accessToken }
            };

            return await _apiHub.GetAsync<TranslationDto>(_serviceName, $"{_group}/{nameof(GetTranslationById)}", queryParams,headers);
        }
        public async Task<ApiResponse<DataBoolenDto>> CreateTranslation(TranslationCreateDto model)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken,_accessToken }
            };
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(CreateTranslation)}", model,queryParams:null,headers);
        }

        public async Task<ApiResponse<DataBoolenDto>> EditTranslation(int id, TranslationEditDto model)
        {
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken,_accessToken }
            };
            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(EditTranslation)}", model, queryParams,headers);
        }

        public async Task<ApiResponse<DataBoolenDto>> DeleteTranslation(int id)
        {
            Dictionary<string, string> queryParams = new()
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken,_accessToken }
            };

            return await _apiHub.PostAsync<DataBoolenDto>(_serviceName, $"{_group}/{nameof(DeleteTranslation)}", data: null, queryParams,headers);
        }
    }
}
