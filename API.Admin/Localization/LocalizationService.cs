using Entities.Enums;
using Microsoft.Extensions.Localization;
using Repositories;

namespace API.Admin.Localization
{
    public class LocalizationService : IStringLocalizer
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly string _culture;
        private readonly int _tenantId;

        public LocalizationService(UnitOfWork unitOfWork, int tenantId, string culture)
        {
            _unitOfWork = unitOfWork;
            _culture = culture;
            _tenantId = tenantId;
        }

        public LocalizedString this[string name] => GetLocalizedTextAsync(name).GetAwaiter().GetResult();

        public LocalizedString this[string name, params object[] arguments] =>
        new(name, string.Format(this[name].Value, arguments));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return Enumerable.Empty<LocalizedString>();
        }

        private async Task<LocalizedString> GetLocalizedTextAsync(string key)
        {
            string? value = await _unitOfWork.TranslationRepository.GetLocalizedText(_tenantId, ApplicationEnum.API_Admin, _culture, key);
            return new LocalizedString(key, value ?? key, value == null);
        }
    }
}
