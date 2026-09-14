using Shared.DTOs.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.RequestHandler;

namespace Shared.DTOs.HR.PermissionModels
{
    public class ResourceParameters : RequestParameters
    {
    }
    public class ResourceDto : TenantBaseEntityDto
    {
        public string? EncryptedId { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }
        public List<RoleAssigneeDto>? Assignees { get; set; }
    }

    public class ResourceCreateDto : ResourceEditDto
    {
    }
    public class ResourceEditDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
