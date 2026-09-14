using Shared.DTOs.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.RequestHandler;

namespace Shared.DTOs.HR.PermissionModels
{
    public class FunctionalityParameters : RequestParameters
    {
        public int Fk_Resource { get; set; }
    }
    public class FunctionalityDto : TenantBaseEntityDto
    {
        public string? EncryptedId { get; set; }

        public int Fk_Resource { get; set; }

        public string? ResourceName { get; set; }

        public string? Name { get; set; }

        public string? FunctionalityCode { get; set; }

        public string? Description { get; set; }

        public List<RoleAssigneeDto>? Assignees { get; set; }
    }
    public class FunctionalityCreateDto : FunctionalityEditDto
    { }
    public class FunctionalityEditDto
    {
        public int Fk_Resource { get; set; }
        public required string Name { get; set; }
        public required string? FunctionalityCode { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
