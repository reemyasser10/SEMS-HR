using AutoMapper;
using Azure.Storage.Blobs;
using Data;
using Entities.DBModels.Common;
using Entities.Enums;
using static Entities.Enums.SeedDataEnum;
using Entities.Shared;
using Identity;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories;
using Shared.DTOs.SharedModels;
using Shared.DTOs.Helpers;
using Microsoft.EntityFrameworkCore;
using Utilities.ActionFilters;
using Utilities.Constants;
using Utilities.PaginationHelper;
using Utilities.RequestHandler;
using Utilities.Settings;
using Utilities.EmailServices;

namespace API.Admin.Controllers
{
    [ApiAuthorize]
    [ApiController]
    public class ExtendControllerBase : ControllerBase
    {
        protected readonly LinkGenerator _linkGenerator;
        private readonly JwtUtil _jwtUtil;
        private readonly TenantConnectionResolver _tenantService;
        protected readonly IMapper _mapper;
        private ApplicationDbContext _dbContext;
        private UnitOfWork _unitOfWork;
        private AuthenticationUtil _authenticationUtil;
        protected readonly AppSettings _appSettings;
        protected CryptoService _crypto;

        private int _tenantId = -1;
        public ExtendControllerBase(TenantConnectionResolver tenantService,
                                   JwtUtil jwtUtil,
                                   LinkGenerator linkGenerator,
                                   IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto)
        {
            _linkGenerator = linkGenerator;
            _jwtUtil = jwtUtil;
            _tenantService = tenantService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _crypto = crypto;
        }
    
        private ApplicationDbContext DbContext
        {
            get
            {
                _dbContext ??= _tenantService.CreateDbContextForTenant(TenantId);
                return _dbContext;
            }
        }
        protected int TenantId => _tenantId == -1 ? SetTenantId() : _tenantId;
        protected UnitOfWork UnitOfWork
        {
            get
            {
                _unitOfWork ??= new UnitOfWork(DbContext,_crypto);
                return _unitOfWork;
            }
        }
        protected AuthenticationUtil AuthenticationUtil
        {
            get
            {
                _authenticationUtil ??= new AuthenticationUtil(UnitOfWork, _jwtUtil);
                return _authenticationUtil;
            }
        }

     

        // Logic Helper Methods
        protected async Task<T> SetAuditFieldsForCreation<T>(T entity) where T : TenantBaseEntity
        {
            entity.Fk_Tenant = TenantId;
            entity.CreatedByIp = IpAddress();

            if (GetUserId() > 0)
            {
                entity.CreatedByUserID = GetUserId();
                entity.CreatedByName = await GetUserName(entity.CreatedByUserID);
            }

            if (GetDeviceId() > 0)
            {
                entity.CreatedByDeviceID = GetDeviceId();
            }

            return entity;
        }

        protected async Task<IEnumerable<T>> SetAuditFieldsForCreation<T>(IEnumerable<T> entities) where T : TenantBaseEntity
        {
            foreach (var entity in entities)
            {
                entity.Fk_Tenant = TenantId;
                entity.CreatedByIp = IpAddress();

                if (GetUserId() > 0)
                {
                    entity.CreatedByUserID = GetUserId();
                    entity.CreatedByName = await GetUserName(entity.CreatedByUserID);
                }

                if (GetDeviceId() > 0)
                {
                    entity.CreatedByDeviceID = GetDeviceId();
                }
            }

            return entities;
        }

        protected async Task<T> SetAuditFieldsForUpdate<T>(T entity) where T : BaseEntity
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedByIp = IpAddress();

            if (GetUserId() > 0)
            {
                entity.UpdatedByUserID = GetUserId();
                entity.UpdatedByName = await GetUserName(entity.UpdatedByUserID);
            }

            if (GetDeviceId() > 0)
            {
                entity.UpdatedByDeviceID = GetDeviceId();
            }

            return entity;
        }
        protected static TenantBaseEntityDto MapAuditFields(TenantBaseEntity entity)
        {
            return new TenantBaseEntityDto
            {
                Id = entity.Id,
                Fk_Tenant = entity.Fk_Tenant,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedByUserID = entity.CreatedByUserID,
                CreatedByName = entity.CreatedByName,
                CreatedByDeviceID = entity.CreatedByDeviceID,
                CreatedByIp = entity.CreatedByIp,
                UpdatedAt = entity.UpdatedAt,
                UpdatedByUserID = entity.UpdatedByUserID,
                UpdatedByName = entity.UpdatedByName,
                UpdatedByDeviceID = entity.UpdatedByDeviceID,
                UpdatedByIp = entity.UpdatedByIp,
                SoftDelete = entity.SoftDelete,
                Sort = entity.Sort
            };
        }

        // Auth Helper Methods
        protected int GetUserId()
        {
            return (int?)Request.HttpContext.Items[ApiConstants.UserId] ?? -1;
        }

     
        protected async Task<UserDto> GetUser(int? id = null)
        {
            return await AuthenticationUtil.UserDto(TenantId, id ?? GetUserId());
        }
        protected async Task<string> GetUserName(int? id = null)
        {
            return await AuthenticationUtil.UserName(TenantId, id ?? GetUserId());
        }
        protected async Task<string> GetRefreshToken()
        {
            return await AuthenticationUtil.GetRefreshToken(TenantId, GetUserId());
        }
        protected async Task<bool> IsAuth(bool throwException = true)
        {
            int userID = GetUserId();

            if (userID is (-1) or 0)
                return throwException ? throw new Exception("Unauthorized") : false;

            if (!await AuthenticationUtil.IsExist(TenantId, userID))
                return throwException ? throw new Exception("User not found") : false;

            if (!await AuthenticationUtil.IsActive(TenantId, userID))
                return throwException ? throw new Exception("Account is inactive") : false;

            return true;
        }

        protected List<int> GetClaimValues(string claimType)
        {
            var claim = User.FindFirst(claimType)?.Value;
            if (string.IsNullOrEmpty(claim)) return new List<int>();
            return claim.Split(',').Select(int.Parse).ToList();
        }

        // HttpContext Helper Methods
        protected int SetTenantId(int? id = null)
        {
            _tenantId = id ?? (int?)Request.HttpContext.Items[ApiConstants.TenantId] ?? -1;
            return _tenantId;
        }
        protected int GetDeviceId()
        {
            return (int?)Request.HttpContext.Items[ApiConstants.DeviceId] ?? -1;
        }
        protected string? GetCulture()
        {
            return (string?)Request.HttpContext.Items[ApiConstants.Culture]??"en";
        }

        // Helper Methods
        protected void SetPagination(MetaData metaData, RequestParameters requestParameters)
        {
            string? actionUri = _linkGenerator.GetUriByAction(HttpContext);

            Response.Headers.Append(key: HeadersConstants.Pagination, value: MetaData.PaginationMetaData(metaData, requestParameters, actionUri));
        }
        protected string GetBaseUri()
        {
            return _linkGenerator.GetUriByAction(HttpContext).GetBaseUri(HttpContext.Request.RouteValues["area"].ToString());
        }
        protected string? IpAddress()
        {
            // get source ip address for the current request
            return Request.Headers.ContainsKey("x-Forwarded-For")
                ? (string?)Request.Headers["x-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        protected string GetStorageAccount()
        {
            int tenantId = TenantId;

            Entities.DBModels.Common.Configuration? configDB = UnitOfWork.ConfigurationRepository
                                                                         .Find(a => (a.Fk_Tenant == tenantId || a.Fk_Tenant == null) &&
                                                                                     a.Module == ModuleEnum.StorageAccount &&
                                                                                     a.Key == "AzureStorage")
                                                                         .OrderByDescending(a => a.Fk_Tenant != null)
                                                                         .FirstOrDefault();

            return configDB?.Value ?? "";
        }

       
        protected EmailConfiguration GetEmailConfiguration()
        {
            int tenantId = TenantId;

            EmailConfiguration emailConfig = new();

            IQueryable<Entities.DBModels.Common.Configuration> configDB = UnitOfWork.ConfigurationRepository
                                                                                    .Find(a => (a.Fk_Tenant == tenantId || a.Fk_Tenant == null) &&
                                                                                                a.Module == ModuleEnum.EmailConfiguration)
                                                                                    .OrderByDescending(a => a.Fk_Tenant != null);
            if (configDB != null)
            {
                emailConfig = new EmailConfiguration
                {
                    From = configDB.FirstOrDefault(c => c.Key == "From")?.Value ?? "",
                    FromName = configDB.FirstOrDefault(c => c.Key == "FromName")?.Value ?? "",
                    SmtpServer = configDB.FirstOrDefault(c => c.Key == "SmtpServer")?.Value ?? "",
                    Port = int.TryParse(configDB.FirstOrDefault(c => c.Key == "Port")?.Value, out int port) ? port : 587,
                    UserName = configDB.FirstOrDefault(c => c.Key == "Username")?.Value ?? "",
                    Password = configDB.FirstOrDefault(c => c.Key == "Password")?.Value ?? ""
                };
            }
            return emailConfig;
        }



        protected async Task<Entities.DBModels.Common.Attachment> UploadFileToStorageAsync(IFormFile file, EntityTypeEnum entityType, int entityId = 0, string? description = null)
        {
            using (Stream stream = file.OpenReadStream())
            {
                return await UploadFileToStorageAsync(stream, file.FileName, file.ContentType, file.Length, entityType, entityId, description);
            }
        }

        protected async Task<Entities.DBModels.Common.Attachment> UploadFileToStorageAsync(byte[] fileBytes, string fileName, string contentType, EntityTypeEnum entityType, int entityId = 0, string? description = null)
        {
            using (MemoryStream stream = new MemoryStream(fileBytes))
            {
                return await UploadFileToStorageAsync(stream, fileName, contentType, fileBytes.Length, entityType, entityId, description);
            }
        }

        protected async Task<Entities.DBModels.Common.Attachment> UploadFileToStorageAsync(Stream stream, string fileName, string contentType, long fileSize, EntityTypeEnum entityType, int entityId = 0, string? description = null)
        {
            string storageAccount = GetStorageAccount();
            string containerName = entityType.ToString().ToLower();

            BlobServiceClient blobServiceClient = new BlobServiceClient(storageAccount);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            string dateFolder = DateTime.UtcNow.ToString("yyyy/MM/dd");
            string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            string fullPath = $"{dateFolder}/{uniqueFileName}";

            BlobClient blobClient = containerClient.GetBlobClient(fullPath);
            await blobClient.UploadAsync(stream, overwrite: true);

            string accountName = storageAccount.Split(';').FirstOrDefault(s => s.Contains("AccountName"))?.Split('=')[1] ?? "";
            string storagePath = $"https://{accountName}.blob.core.windows.net/";

            var attachment = new Entities.DBModels.Common.Attachment
            {
                Fk_Tenant = TenantId,
                EntityType = entityType,
                EntityId = entityId,
                Description = description,
                FilePath = fullPath,
                ContainerName = containerName,
                StoragePath = storagePath,
                FileName = fileName,
                FileType = contentType,
                FileSize = (int)fileSize,
            };

            await SetAuditFieldsForCreation(attachment);
            return attachment;
        }

    }
}
