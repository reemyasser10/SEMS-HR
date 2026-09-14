using Entities.Enums;
using Shared.DTOs.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.RequestHandler;

namespace Shared.DTOs.CommonModels
{
    public class TranslationParameters : RequestParameters
    {
        public string? LanguageCode { get; set; }

    }
    public class TranslationDto 
    {
        public TenantBaseEntityDto? BaseEntity { get; set; }

        public string? EncryptedId { get; set; }
        public ApplicationEnum ApplicationEnum { get; set; }
        public string? Application { get; set; }

        public string? LanguageCode { get; set; }

        public string? RowText { get; set; }

        public string? TranslatedText { get; set; }
    }

    public class TranslationCreateDto
    {
        public ApplicationEnum? ApplicationEnum { get; set; }

        public required string LanguageCode { get; set; }

        public required string RowText { get; set; }
        public required string TranslatedText { get; set; }

    }
    public class TranslationEditDto
    {
        public ApplicationEnum ApplicationEnum { get; set; }

        public string? TranslatedText { get; set; }
        public string? LanguageCode { get; set; }

        public string? RowText { get; set; }
    }
}
