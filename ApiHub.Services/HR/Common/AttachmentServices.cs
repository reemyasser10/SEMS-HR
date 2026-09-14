using Azure.Core;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Extensions;

namespace ApiHub.Services.HR.Common
{
    public class AttachmentServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _group = "Common/Attachment";
        private readonly string _serviceName;
        private readonly string _accessToken;

        public AttachmentServices(ApiBase apiHub, string serviceName,string accessToken)
        {
            _apiHub = apiHub;
            _serviceName = serviceName;
            _accessToken = accessToken;
        }

        public async Task<ApiResponse<AttachmentDto>> GetFileById(int id)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.GetAsync<AttachmentDto>(_serviceName, $"{_group}/GetFileById", queryParams, headers);
        }

        public async Task<ApiResponse<IEnumerable<AttachmentDto>>> GetFilesByIds(IEnumerable<int> ids)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            var queryParams = new Dictionary<string, string>
            {
                { "ids", string.Join(",", ids) }
            };

            return await _apiHub.GetAsync<IEnumerable<AttachmentDto>>(_serviceName, $"{_group}/GetFilesByIds", queryParams, headers);
        }

        public async Task<ApiResponse<AttachmentDto>> DownloadFile(int id)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.GetAsync<AttachmentDto>(_serviceName, $"{_group}/DownloadFile", queryParams, headers);
        }

        public async Task<ApiResponse<List<AttachmentDto>>> UploadFiles(UploadFilesDto content)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };

            var formData = ApiBase.ToMultipartContent(content);

            return await _apiHub.PostFormAsync<List<AttachmentDto>>(_serviceName, $"{_group}/{nameof(UploadFiles)}", formData, queryParams: null, headers);
        }

        public async Task<ApiResponse<AttachmentDto>> UploadFile(UploadFileDto content)
        {
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };

            var formData = ApiBase.ToMultipartContent(content);

            return await _apiHub.PostFormAsync<AttachmentDto>(_serviceName, $"{_group}/UploadFile", formData, queryParams: null, headers);
        }

        public async Task<ApiResponse<bool>> DeleteFile(int id)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "id", id.ToString() }
            };
            Dictionary<string, string> headers = new()
            {
                { ApiConstants.AccessToken, _accessToken }
            };
            return await _apiHub.PostAsync<bool>(_serviceName, $"{_group}/DeleteFile", data: null, queryParams, headers);
        }

        public async Task<string> GetAttachmentURL(int? fk_Attachment)
        {
            if (fk_Attachment.IsNull())
            {
                return string.Empty;
            }
            var result = await GetFileById(fk_Attachment.Value);

            return result.Data.FullPath;
        }

        public async Task<int?> HandelUploadsAsync(IFormFile imageFile, int? existingFileId,
            EntityTypeEnum entityType)
        {
            if (imageFile.IsNull()) return null;

            if (existingFileId.IsNotNull())
            {
                await DeleteFile(existingFileId.Value);
            }
            // Upload file 
            var newFile = await UploadFile(new UploadFileDto
            {
                File = imageFile,
                EntityType = (int)entityType
            });

            return newFile.Data.BaseEntity.Id;
        }

        
    }
}
