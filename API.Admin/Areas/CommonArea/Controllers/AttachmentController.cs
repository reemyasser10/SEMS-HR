using API.Admin.Controllers;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Data;
using Entities.Enums;
using Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.DTOs.CommonModels;
using Utilities.ActionFilters;
using Utilities.Extensions;
using Utilities.RequestHandler;
using Utilities.Settings;

namespace API.Admin.Areas.CommonArea.Controllers
{
    [Area("Common")]
    [ApiExplorerSettings(GroupName = "Common")]
    [Route("api/[area]/[controller]")]
    public class AttachmentController : ExtendControllerBase
    {
        public AttachmentController(TenantConnectionResolver tenantService,
                                JwtUtil jwtUtil, LinkGenerator linkGenerator, IOptions<AppSettings> appSettings,
                                 IMapper mapper, CryptoService crypto) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto)
        {
        }

        private BlobServiceClient _blobServiceClient;
        private string _storageAccount;

        private BlobServiceClient BlobServiceClient
        {
            get
            {
                _storageAccount = GetStorageAccount();
                _blobServiceClient ??= new(_storageAccount);
                return _blobServiceClient;
            }
        }

      


        [HttpGet]
        [Route(nameof(GetFileById))]
        [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFileById([FromQuery, BindRequired] int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid ID");
            }
            AttachmentDto dto = await GetAttachment(id);

            return dto.IsNull() ? throw new Exception("File not found") : (IActionResult)Ok(dto);
        }

        [HttpGet]
        [Route(nameof(GetFilesByIds))]
        [ProducesResponseType(typeof(IEnumerable<AttachmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFilesByIds([FromQuery(Name = "ids")] string ids)
        {
            if (string.IsNullOrEmpty(ids))
                throw new Exception("ids is required");

            var idList = ids.Split(',').Select(int.Parse).ToList();

            IList<AttachmentDto> dtos = [];
            foreach (int id in idList)
            {
                AttachmentDto dto = await GetAttachment(id);
                dtos.Add(dto);
            }

            return Ok(dtos);
        }


        [HttpGet]
        [Route(nameof(DownloadFile))]
        public async Task<IActionResult> DownloadFile([FromQuery, BindRequired] int id)
        {
            AttachmentDto attachment = await GetAttachmentDto(id) ?? throw new Exception("File not found.");

            BlobContainerClient containerClient = BlobServiceClient.GetBlobContainerClient(attachment.ContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(attachment.FilePath);

            Stream stream = await blobClient.OpenReadAsync();
            attachment.FileType = attachment.FileType?.TrimEnd('/');
            return File(stream, attachment.FileType, attachment.FileName);
        }

      

        [HttpPost]
        [Route(nameof(DeleteFile))]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ApiAuthorize]
        public async Task<IActionResult> DeleteFile([FromQuery, BindRequired] int id)
        {
            if (id <= 0)
            {
                throw new Exception("Invalid ID");
            }

            Entities.DBModels.Common.Attachment entity = await UnitOfWork.AttachmentRepository.GetByIdAsync(id, trackChanges: true);

            if (entity.IsNull())
            {
                throw new Exception("File not found");
            }

            _ = await SetAuditFieldsForUpdate(entity);
            await UnitOfWork.AttachmentRepository.SoftDeleteAsync(id);
            _ = await UnitOfWork.SaveChangesAsync();
            return Ok(true);
        }

        private async Task<AttachmentDto> GetAttachment(int id)
        {
            AttachmentDto attachment = await GetAttachmentDto(id) ?? throw new Exception("File not found.");

            BlobContainerClient containerClient = BlobServiceClient.GetBlobContainerClient(attachment.ContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(attachment.FilePath);

            BlobSasBuilder sasBuilder = new()
            {
                BlobName = blobClient.Name,
                Resource = "b", // 'b' = blob
                ExpiresOn = DateTime.UtcNow.AddMinutes(10), // Set expiration
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

            string cleanUrl = $"{sasUri}&response-content-disposition=inline; filename={attachment.FileName}";

            attachment.FullPath = cleanUrl;


            return attachment;
        }

        private async Task<AttachmentDto> GetAttachmentDto(int id)
        {
            return await UnitOfWork.AttachmentRepository
                                   .Find(c => c.Id == id)
                                   .Select(a => new AttachmentDto
                                   {
                                       Id=a.Id,
                                       Description = a.Description,
                                       EntityId = a.EntityId,
                                       EntityType = a.EntityType,
                                       FileName = a.FileName,
                                       FileType = a.FileType,
                                       FileSize = a.FileSize,
                                       FilePath = a.FilePath,
                                       ContainerName = a.ContainerName,
                                       StoragePath = a.StoragePath,
                                       BaseEntity = MapAuditFields(a)
                                   }).FirstOrDefaultAsync();
        }


        [HttpPost]
        [Route(nameof(UploadFile))]
        [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileDto file)
        {
            var attachment = await UploadFileToStorageAsync(file.File, (EntityTypeEnum)file.EntityType, file.EntityId, file.Description);

            await UnitOfWork.AttachmentRepository.AddAsync(attachment);
            await UnitOfWork.SaveChangesAsync();

            return Ok(await GetAttachment(attachment.Id));
        }
        [HttpPost]
        [Route(nameof(UploadFiles))]
        [ProducesResponseType(typeof(IEnumerable<AttachmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFiles([FromForm] UploadFilesDto model)
        {
            if (model.Files == null || !model.Files.Any())
                return BadRequest("No files uploaded.");

            var attachments = new List<Entities.DBModels.Common.Attachment>();

            foreach (var file in model.Files)
            {
                var attachment = await UploadFileToStorageAsync(file, (EntityTypeEnum)model.EntityType, model.EntityId, model.Description);
                attachments.Add(attachment);
            }

            await UnitOfWork.AttachmentRepository.AddRangeAsync(attachments);
            await UnitOfWork.SaveChangesAsync();

            var result = new List<AttachmentDto>();
            foreach (var att in attachments)
            {
                result.Add(await GetAttachment(att.Id));
            }

            return Ok(result);
        }


    }
}
