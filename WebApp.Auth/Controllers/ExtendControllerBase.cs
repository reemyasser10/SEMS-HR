using ApiHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Auth.Controllers
{
    public class ExtendControllerBase : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ApiHubService _apiHub;

        protected readonly ApiBase _apiBase;

        public ExtendControllerBase(IHttpClientFactory httpClient, ApiHubService apiHub)
        {
            _httpClient = httpClient;
            _apiHub = apiHub;

            _apiBase = new ApiBase(_httpClient, _apiHub);
        }
    }
}
