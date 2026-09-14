using Identity.Entities;
using Shared.DTOs.CommonModels;
using System.Text;
using System.Text.Json;
using Utilities.Constants;
using Utilities.Extensions;
using Utilities.PaginationHelper;
using Utilities.ResponseHandler;

namespace ApiHub.Services
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public ResponseStatus? Status { get; set; }
        public PaginationMetaData? Pagination { get; set; }

        public bool IsSuccess => Status?.Success ?? false;

        public string? Expires { get; set; }
        public string? Authorization { get; set; }

        public TokenResponse? RefreshToken { get; set; }
    }

    public class ApiBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiHubService _apiHubService;

        public ApiBase(IHttpClientFactory httpClientFactory, ApiHubService apiHubService)
        {
            _httpClientFactory = httpClientFactory;
            _apiHubService = apiHubService;
        }

        private HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient();
        }

        private string BuildUrl(ServiceConfig config, string endpoint, Dictionary<string, string>? queryParams = null)
        {
            string baseUrl = config.BaseUrl.TrimEnd('/');
            string finalUrl = $"{baseUrl}/{endpoint.TrimStart('/')}";

            if (queryParams != null && queryParams.Count > 0)
            {
                string queryString = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                finalUrl += "?" + queryString;
            }

            return finalUrl;
        }

        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
        {

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _ = request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        private T? ExtractJsonHeader<T>(HttpResponseMessage response, string headerKey) where T : class
        {
            if (response.Headers.TryGetValues(headerKey, out IEnumerable<string>? values))
            {
                string? jsonValue = values.FirstOrDefault().CommaDecode();
                if (jsonValue.IsExisting())
                {
                    try
                    {
                        return JsonSerializer.Deserialize<T>(jsonValue);
                    }
                    catch
                    {
                        return null; // If parsing fails, return null
                    }
                }
            }
            return null;
        }

        private string? ExtractHeader(HttpResponseMessage response, string headerKey)
        {
            if (response.Headers.TryGetValues(headerKey, out IEnumerable<string>? values))
            {
                string? value = values.FirstOrDefault().CommaDecode();
                if (value.IsExisting())
                {
                    try
                    {
                        return value;
                    }
                    catch
                    {
                        return null; // If parsing fails, return null
                    }
                }
            }
            return null;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(
        string serviceName,
        string endpoint,
        Dictionary<string, string>? queryParams = null,
        Dictionary<string, string>? headers = null)
        {
            ServiceConfig config = _apiHubService.GetServiceConfig(serviceName);
            TenantConfig tenantConfig = _apiHubService.GetTenantConfig();
            string url = BuildUrl(config, endpoint, queryParams);
            if (headers.IsNull())
            {
                headers = [];
            }
           
            if (!headers.Any(a => a.Key == "Api-Key"))
            {
                headers.Add("Api-Key", config.ApiKey);
            }
            if (!headers.Any(a => a.Key == ApiConstants.TenantId))
            {
                headers.Add(ApiConstants.TenantId, tenantConfig.Id);
            }
            using HttpRequestMessage request = new(HttpMethod.Get, url);
            AddHeaders(request, headers);
            
            HttpClient client = CreateClient();
           HttpResponseMessage response = await client.SendAsync(request);
            _ = response.EnsureSuccessStatusCode();

            ApiResponse<T> apiResponse = new()
            {
                Status = ExtractJsonHeader<ResponseStatus>(response, HeadersConstants.Status),
                Pagination = ExtractJsonHeader<PaginationMetaData>(response, HeadersConstants.Pagination)
            };

            string? content = await response.Content.ReadAsStringAsync();
            if (content.IsExisting())
            {
                try
                {
                    apiResponse.Data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Failed to deserialize API response. Content: '{content}'. Error: {ex.Message}");
                }
            }

            return apiResponse;
        }
        public async Task<int> GetAsyncInt(
    string serviceName,
    string endpoint,
    Dictionary<string, string>? queryParams = null,
    Dictionary<string, string>? headers = null)

        {
            ServiceConfig config = _apiHubService.GetServiceConfig(serviceName);
            TenantConfig tenantConfig = _apiHubService.GetTenantConfig();
            string url = BuildUrl(config, endpoint, queryParams);

            if (headers.IsNull())
            {
                headers = new Dictionary<string, string>();
            }

            headers.Add("Api-Key", config.ApiKey);
            headers.Add("TenantId", tenantConfig.Id);

            using HttpRequestMessage request = new(HttpMethod.Get, url);
            AddHeaders(request, headers);

            HttpClient client = CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            _ = response.EnsureSuccessStatusCode();

            string? content = await response.Content.ReadAsStringAsync();


            if (content.IsExisting())
            {

                return JsonSerializer.Deserialize<int>(content);
            }

            return 0;
        }

        public async Task<HttpResponseMessage> GetRawAsync(string serviceName, string path, Dictionary<string, string> headers = null)
        {
            var client = _httpClientFactory.CreateClient(serviceName);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (!client.DefaultRequestHeaders.Contains(header.Key))
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var response = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead); 
            return response;
        }

        public async Task<ApiResponse<T>> PostFormAsync<T>(
            string serviceName,
            string endpoint,
            MultipartFormDataContent formData,
            Dictionary<string, string>? queryParams = null,
            Dictionary<string, string>? headers = null)
        {
            ServiceConfig config = _apiHubService.GetServiceConfig(serviceName);
            TenantConfig tenantConfig = _apiHubService.GetTenantConfig();
            string url = BuildUrl(config, endpoint, queryParams);

            if (headers.IsNull())
            {
                headers = [];
            }

            headers.Add("Api-Key", config.ApiKey);
            headers.Add("TenantId", tenantConfig.Id);

            using HttpRequestMessage request = new(HttpMethod.Post, url)
            {
                Content = formData
            };

            AddHeaders(request, headers);

            HttpClient client = CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            _ = response.EnsureSuccessStatusCode();

            ApiResponse<T> apiResponse = new()
            {
                Status = ExtractJsonHeader<ResponseStatus>(response, HeadersConstants.Status),
                Pagination = ExtractJsonHeader<PaginationMetaData>(response, HeadersConstants.Pagination),
                RefreshToken = ExtractJsonHeader<TokenResponse>(response, HeadersConstants.SetRefresh),
                Authorization = ExtractHeader(response, HeadersConstants.Authorization),
                Expires = ExtractHeader(response, HeadersConstants.Expires)
            };

            string? content = await response.Content.ReadAsStringAsync();
            if (content.IsExisting())
            {
                apiResponse.Data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return apiResponse;
        }

        public static MultipartFormDataContent ToMultipartContent(UploadFilesDto dto)
        {
            var content = new MultipartFormDataContent();

            // Files
            if (dto.Files != null && dto.Files.Any())
            {
                foreach (var file in dto.Files)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    // 👇 Important: field name must be "Files", not "Files[i].File"
                    content.Add(streamContent, "Files", file.FileName);
                }
            }

            // Metadata (one copy for all files)
            content.Add(new StringContent(dto.FileType ?? ""), "FileType");
            content.Add(new StringContent(dto.Fk_Entity.ToString()), "Fk_Entity");
            content.Add(new StringContent(dto.Description ?? ""), "Description");
            content.Add(new StringContent(dto.EntityId.ToString()), "EntityId");
            content.Add(new StringContent(dto.EntityType.ToString()), "EntityType");

            return content;
        }

        public static MultipartFormDataContent ToMultipartContent(UploadFileDto dto)
        {
            var content = new MultipartFormDataContent();

            if (dto.File != null)
            {
                var streamContent = new StreamContent(dto.File.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.File.ContentType);
                content.Add(streamContent, nameof(dto.File), dto.File.FileName);
            }

            content.Add(new StringContent(dto.FileType ?? ""), nameof(dto.FileType));
            content.Add(new StringContent(dto.Fk_Entity.ToString()), nameof(dto.Fk_Entity));
            content.Add(new StringContent(dto.Description ?? ""), nameof(dto.Description));
            content.Add(new StringContent(dto.EntityType.ToString()), nameof(dto.EntityType));
            content.Add(new StringContent(dto.EntityId.ToString()), nameof(dto.EntityId));

            return content;
        }


        public async Task<ApiResponse<T>> PostAsync<T>(
            string serviceName,
            string endpoint,
            object data,
            Dictionary<string, string>? queryParams = null,
            Dictionary<string, string>? headers = null)
        {
            ServiceConfig config = _apiHubService.GetServiceConfig(serviceName);
            TenantConfig tenantConfig = _apiHubService.GetTenantConfig();

            string url = BuildUrl(config, endpoint, queryParams);
            if (headers.IsNull())
            {
                headers = [];
            }
            headers.Add("Api-Key", config.ApiKey);
            headers.Add("TenantId", tenantConfig.Id);

            using HttpRequestMessage request = new(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
            };

            AddHeaders(request, headers);

            HttpClient client = CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            _ = response.EnsureSuccessStatusCode();

            ApiResponse<T> apiResponse = new()
            {
                Status = ExtractJsonHeader<ResponseStatus>(response, HeadersConstants.Status),
                Pagination = ExtractJsonHeader<PaginationMetaData>(response, HeadersConstants.Pagination),

                RefreshToken = ExtractJsonHeader<TokenResponse>(response, HeadersConstants.SetRefresh),

                Authorization = ExtractHeader(response, HeadersConstants.Authorization),
                Expires = ExtractHeader(response, HeadersConstants.Expires),
            };

            string? content = await response.Content.ReadAsStringAsync();
            if (content.IsExisting())
            {
                apiResponse.Data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return apiResponse;
        }
    }
}
