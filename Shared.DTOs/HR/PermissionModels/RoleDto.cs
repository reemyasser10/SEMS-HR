using Shared.DTOs.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.RequestHandler;

namespace Shared.DTOs.HR.PermissionModels
{
    public class RoleParameters : RequestParameters
    {
    }
    public class RoleDto : TenantBaseEntityDto
    {
        public string? EncryptedId { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }
        public string? ColorCode { get; set; }
    }

    public class RoleEditDto
    {
        [Required(ErrorMessage = "Role name is required.")]
        [RegularExpression(@"^[\p{L}0-9\s]*[\p{L}][\p{L}0-9\s]*$", ErrorMessage = "Role name contains invalid characters or cannot be numbers only.")]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ColorCode { get; set; }
        public bool IsActive { get; set; } = true;
        [MinLength(1, ErrorMessage = "Please select at least one permission.")]
        public List<int> Fk_Functionalities { get; set; } = new List<int>();
    }

    public class RoleAssigneeDto
    {
        public string? RoleName { get; set; }
        public string? ColorCode { get; set; }
        public int Fk_Role { get; set; }
    }

    public class RoleCreateDto
    {

        [Required(ErrorMessage = "Role name is required.")]
        [RegularExpression(@"^[\p{L}0-9\s]*[\p{L}][\p{L}0-9\s]*$", ErrorMessage = "Role name contains invalid characters or cannot be numbers only.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ColorCode { get; set; }

        public bool IsActive { get; set; }
        [MinLength(1, ErrorMessage = "Please select at least one permission.")]
        public List<int> Fk_Functionalities { get; set; } = new List<int>();

        public List<RoleResourceCreateDto> Resources { get; set; } = new List<RoleResourceCreateDto>();
    }
    public class RoleResourceCreateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<RoleFunctionalityCreateDto> Functionalities { get; set; } = new List<RoleFunctionalityCreateDto>();
    }
    public class RoleFunctionalityCreateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UpdateResourcePermissionsDto : UpdateRolePermissionsDto
    {
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public string? ColorCode { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UpdateRolePermissionsDto
    {
        public int Fk_Resource { get; set; }
        public int Fk_Role { get; set; }
        public List<int> Fk_Functionalities { get; set; } = new List<int>();
    }
}
