using ApiHub.Services;
using ApiHub.Services.HR;
using Entities.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace WebApp.Auth.Service
{
    public class HRTranslationService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClient;
        private readonly ApiHubService _apiHub;

        private readonly HRServices _HRServices;
        public HRTranslationService(IMemoryCache cache, IHttpClientFactory httpClient, ApiHubService apiHub)
        {
            _cache = cache;
            _httpClient = httpClient;
            _apiHub = apiHub;

            _HRServices = new HRServices(new ApiBase(_httpClient, _apiHub));
        }

        public async Task<Dictionary<string, string>> GetTranslationsAsync()
        {
            return await _cache.GetOrCreateAsync("Translations", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return (await _HRServices.TranslationServices.GetTranslations(ApplicationEnum.WebApp_Auth)).Data;
            });
        }
    }

    public class HRLocalizationService : IStringLocalizer
    {
        private readonly HRTranslationService _translationCacheService;

        public HRLocalizationService(HRTranslationService translationCacheService)
        {
            _translationCacheService = translationCacheService;
        }

        public LocalizedString this[string name]
        {
            get
            {
                Dictionary<string, string> translations = _translationCacheService.GetTranslationsAsync().Result;
                string value = translations != null ? translations.TryGetValue(name, out string? translation) ? translation : name : name;
                return new LocalizedString(name, value);
            }
        }

        public LocalizedString this[string name, params object[] arguments] =>
            new(name, string.Format(this[name].Value, arguments));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return Enumerable.Empty<LocalizedString>();
        }
    }

}
