using Entities.Shared;
using Shared.DTOs.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.HR.PermissionModels
{
    public class AdministratorDto : TenantBaseEntityDto
    {
        public string? EncryptedId { get; set; }
        public int Fk_User { get; set; }
        public string? JobTitle { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public List<RoleAssigneeDto>? Roles { get; set; }

    }

    public class AdministratorCreateDto : AdministratorEditDto
    {
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
    }
    public class AdministratorEditDto
    {
        public AdministratorEditDto()
        {
            Fk_Roles = new List<int>();
        }
        [RegularExpression(@"^[\p{L}0-9\s]*[\p{L}][\p{L}0-9\s]*$", ErrorMessage = "Job title contains invalid characters or cannot be numbers only.")]
        public string? JobTitle { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        [RegularExpression(@"^.*[\p{L}].*$", ErrorMessage = "Full name must contain at least one letter.")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid phone number (10-15 digits allowed)")]
        public string? PhoneNumber { get; set; }
        public List<int>? Fk_Roles { get; set; }
      
        public bool IsActive { get; set; }
    }
    public class AdminAccessDto
    {
        public bool IsAdmin { get; set; }
    }
}
