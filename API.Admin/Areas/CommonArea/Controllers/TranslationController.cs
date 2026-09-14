using API.Admin.Controllers;
using AutoMapper;
using Data;
using Entities.DBModels.Common;
using Entities.Enums;
using Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories;
using Shared.DTOs;
using Shared.DTOs.CommonModels;
using Utilities.ActionFilters;
using Utilities.Extensions;
using Utilities.PaginationHelper;
using Utilities.RequestHandler;
using Utilities.Settings;

namespace API.Admin.Areas.CommonArea.Controllers
{
    [Area("Common")]
    [ApiExplorerSettings(GroupName = "Common")]
    [Route("api/[area]/[controller]")]
    public class TranslationController : ExtendControllerBase
    {
        public TranslationController(TenantConnectionResolver tenantService,
                                JwtUtil jwtUtil, LinkGenerator linkGenerator,
                                IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetTranslations))]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTranslations([FromQuery, BindRequired] ApplicationEnum applicationEnum)
        {
            string? culture = GetCulture();

            if (culture.IsEmpty())
            {
                //throw new Exception("Culture is required");
                // fall back to english
                culture = "en";
            }

            Dictionary<string, string> translations = await UnitOfWork.TranslationRepository.GetTranslations(TenantId, applicationEnum, culture);
            return Ok(translations);
        }

        [HttpGet]
        [Route(nameof(GetTranslationsPaged))]
        [ProducesResponseType(typeof(IEnumerable<TranslationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTranslationsPaged([FromQuery] TranslationParameters parameters)
        {
            IQueryable<TranslationDto> query = UnitOfWork.TranslationRepository.GetTranslationsDto(parameters);

            PagedList<TranslationDto> paged = await PagedList<TranslationDto>.ToPagedListAsync(query, parameters.PageNumber, parameters.PageSize);

            SetPagination(paged.MetaData, parameters);

            return Ok(paged);
        }

        [HttpGet]
        [Route(nameof(GetTranslationById))]
        [ProducesResponseType(typeof(TranslationDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTranslationById([FromQuery, BindRequired] int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid ID");
            }

            TranslationDto dto = await UnitOfWork.TranslationRepository.GetTranslationsDto(new TranslationParameters
            {
                Id = id
            }).FirstOrDefaultAsync();

            return dto == null ? throw new Exception("Item not found") : Ok(dto);
        }



        [HttpPost]
        [Route(nameof(CreateTranslation))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTranslation([FromBody] TranslationCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Model State");
            }

            Translation entity = new Translation
            {
                LanguageCode = model.LanguageCode,
                ApplicationEnum = model.ApplicationEnum.Value,
                RowText = model.RowText,
                TranslatedText = model.TranslatedText
            };

            _ = await SetAuditFieldsForCreation(entity);

            await UnitOfWork.TranslationRepository.AddAsync(entity);
            _ = await UnitOfWork.SaveChangesAsync();

            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }

        [HttpPost]
        [Route(nameof(EditTranslation))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditTranslation([FromQuery, BindRequired] int id, [FromBody] TranslationEditDto model)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Model State");
            }

            Translation? entity = await UnitOfWork.TranslationRepository.GetByIdAsync(id, trackChanges: true);
            if (entity.IsNull())
            {
                throw new Exception("Translation not found");
            }

            entity.TranslatedText = model.TranslatedText;
            entity.RowText = model.RowText;
            if(model.ApplicationEnum != null)
            {
                entity.ApplicationEnum = model.ApplicationEnum ;
            }

            _ = await SetAuditFieldsForUpdate(entity);

            _ = await UnitOfWork.SaveChangesAsync();

            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }

        [HttpPost]
        [Route(nameof(DeleteTranslation))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTranslation([FromQuery, BindRequired] int id)
        {
            await UnitOfWork.TranslationRepository.DeleteTranslation(id);

            _ = await UnitOfWork.SaveChangesAsync();

            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }



    }
}
