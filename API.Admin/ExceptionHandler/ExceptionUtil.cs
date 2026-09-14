using API.Admin.Localization;
using Data;
using Repositories;
using Utilities.Extensions;
using Utilities.RequestHandler;
using Utilities.ResponseHandler;

namespace API.Admin.ExceptionHandler
{
    public class ExceptionUtil
    {
        private readonly TenantConnectionResolver _tenantService;
        private readonly CryptoService _crypto;
        public ExceptionUtil(TenantConnectionResolver tenantService, CryptoService crypto)
        {
            _tenantService = tenantService;
            _crypto = crypto;
        }

        public async Task<ResponseStatus> Error(int? tenantId, string culture, Exception ex)
        {
            ResponseStatus response = new()
            {
                PlainErrorMessage = ex.Message,
                ExceptionMessage = ex.InnerException.IsNull() ? "" : ex.InnerException.Message
            };

            string? translatedMessage = null;

            if (tenantId > 0)
            {
                UnitOfWork unitOfWork = new(_tenantService.CreateDbContextForTenant(tenantId.Value),_crypto);
                LocalizationService _localizer = new(unitOfWork, tenantId.Value, culture);

                translatedMessage = _localizer[ex.Message];
            }

            response.ErrorMessage = translatedMessage ?? ex.Message;

            return response;
        }
    }

}
