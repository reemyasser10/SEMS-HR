using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.SharedModels
{
    public class CreateUserDto : EditUserDto
    {
    }

    public class EditUserDto 
    {
        public required string FullName { get; set; }

        public required string UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
        public string? PhonePrefix { get; set; }
        public int? FK_Image { get; set; }
        public string? Password { get; set; }

    }

}
