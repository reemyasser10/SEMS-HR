using Data;
using Entities.DBModels.Common;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.CommonModels;
using Shared.DTOs.Helpers;
using Utilities.Extensions;
using Utilities.RequestHandler;

namespace Repositories.Repositories.Common
{
    public class TranslationRepository : GenericRepository<Translation>
    {
        private readonly CryptoService _crypto;
        public TranslationRepository(ApplicationDbContext context, CryptoService crypto) : base(context)
        {
            _crypto = crypto;
        }

        public IQueryable<Translation> GetTranslations(TranslationParameters parameters)
        {
            IQueryable<Translation> query = GetAll();

            if (parameters.Id > 0)
            {
                query = query.Where(x => x.Id == parameters.Id);
            }

            if (parameters.LanguageCode.IsNotNull())
            {
                query = query.Where(x => x.LanguageCode == parameters.LanguageCode);
            }


            if (parameters.ApplicationEnum.IsNotNull())
            {
                query = query.Where(x => (int)x.ApplicationEnum == parameters.ApplicationEnum);
            }

            return query.Sort(parameters.OrderBy);
        }

        public IQueryable<TranslationDto> GetTranslationsDto(TranslationParameters parameters)
        {
            return GetTranslations(parameters)
                   .Select(e => new TranslationDto
                   {
                       EncryptedId = _crypto.EncryptObject(e.Id),
                       BaseEntity = TenantEntityMapper.MapAuditFields(e),
                       LanguageCode = e.LanguageCode,
                       ApplicationEnum = e.ApplicationEnum,
                       RowText = e.RowText,
                       TranslatedText = e.TranslatedText,
                       Application = e.ApplicationEnum.ToString()
                   })
                   .Search(parameters.SearchColumns, parameters.SearchTerm);
        }

        public async Task<string> GetLocalizedText(int tenantId, ApplicationEnum applicationEnum, string culture, string key)
        {
            string? translatedMessage = await Find(e => e.ApplicationEnum == applicationEnum &&
                                                        e.LanguageCode == culture &&
                                                        e.RowText == key &&
                                                        (e.Fk_Tenant == null || e.Fk_Tenant == tenantId))
                                             .OrderByDescending(e => e.Fk_Tenant != null)
                                             .Select(e => e.TranslatedText)
                                             .FirstOrDefaultAsync();

            return translatedMessage.IsExisting() ? translatedMessage : key;
        }

        public async Task<Dictionary<string, string>> GetTranslations(int tenantId, ApplicationEnum applicationEnum, string culture)
        {
            Dictionary<string, string> translations = await Find(t => t.ApplicationEnum == applicationEnum &&
                                                                      (t.Fk_Tenant == null || t.Fk_Tenant == tenantId) &&
                                                                       t.LanguageCode == culture)
                                                            .ToDictionaryAsync(t => t.RowText, t => t.TranslatedText);

            return translations;
        }


        public async Task DeleteTranslation(int id)
        {
            Translation entity = await GetByIdAsync(id);

            _context.Remove(entity);
        }
    }
}
