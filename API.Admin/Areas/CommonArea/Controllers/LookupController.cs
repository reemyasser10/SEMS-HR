using API.Admin.Controllers;
using AutoMapper;
using Data;
using Entities.DBModels.Common;
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
    public class LookupController : ExtendControllerBase
    {
        public LookupController(TenantConnectionResolver tenantService,
            JwtUtil jwtUtil, LinkGenerator linkGenerator,
            IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto) : base(tenantService, jwtUtil,
            linkGenerator, mapper, appSettings, crypto)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetLookups))]
        [ProducesResponseType(typeof(IEnumerable<LookupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLookups([FromQuery] LookupParameters parameters)
        {
            IQueryable<LookupDto> lookups = GetFilteredQuery(parameters).Sort(parameters.OrderBy ?? "Id desc")
                .Select(a => new LookupDto
                {
                    Id = a.Id,
                    EncryptedId = _crypto.EncryptObject(a.Id),
                    UpdatedAt = a.UpdatedAt,
                    Name = a.Name,
                    Description = a.Description,
                    ColorCode = a.ColorCode,
                    EntityType = a.EntityType.ToString(),
                    EntityTypeId = (int)a.EntityType,
                    BaseEntity = MapAuditFields(a)
                }).Search(parameters.SearchColumns, parameters.SearchTerm);
            PagedList<LookupDto> paged =
                await PagedList<LookupDto>.ToPagedListAsync(lookups, parameters.PageNumber, parameters.PageSize);

            SetPagination(paged.MetaData, parameters);

            return Ok(paged);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetCustomLookups))]
        [ProducesResponseType(typeof(IEnumerable<CustomLookUpDto>), StatusCodes.Status200OK)]
        public IActionResult GetCustomLookups([FromQuery] LookupParameters parameters)
        {
            parameters.IsActive ??= true;

            IQueryable<CustomLookUpDto> dto = GetFilteredQuery(parameters).OrderBy(a => a.Sort).Select(a =>
                new CustomLookUpDto
                {
                    Name = a.Name,
                    Description = a.Description,
                    Id = a.Id,
                    ColorCode = a.ColorCode,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId
                });

            return Ok(dto.ToList());
        }

        [AllowAnonymous]
    
        [HttpGet]
        [Route(nameof(GetLookupById))]
        [ProducesResponseType(typeof(LookupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLookupById([FromQuery, BindRequired] int id)
        {
            LookupDto dto = await GetLookupDto(id);
            return dto.IsNull() ? throw new Exception("Lookup not found") : Ok(dto);
        }

        [HttpPost]
        [Route(nameof(CreateLookup))]
        [ProducesResponseType(typeof(LookupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateLookup([FromBody] CreateLookupDto model)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Model State");
            }

            Lookup entity = _mapper.Map<Lookup>(model);

            _ = await SetAuditFieldsForCreation(entity);
            try
            {
                await UnitOfWork.LookUpRepository.AddAsync(entity);
                _ = await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex?.InnerException.Message.Contains("duplicate")??false)
                {
                    throw new Exception("Lookup already exists", ex.InnerException);
                }
                else
                {
                    throw;
                }
            }

            LookupDto dto = await GetLookupDto(entity.Id);
            return Ok(dto);
        }

        [HttpPost]
        [Route(nameof(EditLookup))]
        [ProducesResponseType(typeof(LookupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditLookup([FromQuery, BindRequired] int id, [FromBody] EditLookupDto model)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Model State");
            }

            Lookup? entity = await UnitOfWork.LookUpRepository.GetByIdAsync(id, trackChanges: true);
            if (entity.IsNull())
            {
                throw new Exception("Lookup not found");
            }

            _ = _mapper.Map(model, entity);

            _ = await SetAuditFieldsForUpdate(entity);

            _ = await UnitOfWork.SaveChangesAsync();

            LookupDto dto = await GetLookupDto(entity.Id);
            return Ok(dto);
        }

        [HttpPost]
        [Route(nameof(DeleteLookup))]
        [ProducesResponseType(typeof(DataBoolenDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLookup([FromQuery, BindRequired] int id)
        {
            await UnitOfWork.LookUpRepository.SoftDeleteAsync(id);
            _ = await UnitOfWork.SaveChangesAsync();
            return Ok(new DataBoolenDto
            {
                Value = true
            });
        }

        private async Task<LookupDto> GetLookupDto(int id)
        {
            return await UnitOfWork.LookUpRepository.Find(l => l.Id == id)
                .Select(l => new LookupDto
                {
                    Name = l.Name,
                    Description = l.Description,
                    ColorCode = l.ColorCode,
                    EntityType = l.EntityType.ToString(),
                    EntityTypeId = (int)l.EntityType,
                    BaseEntity = MapAuditFields(l)
                }).FirstOrDefaultAsync();
        }

        private IQueryable<Lookup> GetFilteredQuery(LookupParameters parameters)
        {
            IQueryable<Lookup> lookupQuery = UnitOfWork.LookUpRepository.GetAll();


            if (!parameters.EntityType.IsNull())
            {
                lookupQuery = lookupQuery.Where(a => a.EntityType == parameters.EntityType);
            }

            if (parameters.EntityTypes != null && parameters.EntityTypes.Any())
            {
                lookupQuery = lookupQuery.Where(a => parameters.EntityTypes.Contains((int)a.EntityType));
            }

            if (parameters.IsActive.HasValue)
            {
                lookupQuery = lookupQuery.Where(a => a.IsActive == parameters.IsActive.Value);
            }

            return lookupQuery;
        }
    }
}
