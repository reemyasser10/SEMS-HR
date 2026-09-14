using Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.CommonModels
{
    public class UploadFileDto
    {
        public IFormFile File { get; set; }
        public string FileType { get; set; }
        public int Fk_Entity { get; set; }
        public string Description { get; set; }
        public int EntityType { get; set; }
        public int EntityId { get; set; }
    }

    public class UploadFilesDto
    {
        public List<IFormFile> Files { get; set; }
        public string FileType { get; set; }
        public int Fk_Entity { get; set; }
        public string Description { get; set; }
        public int EntityType { get; set; }
        public int EntityId { get; set; }
    }



}
