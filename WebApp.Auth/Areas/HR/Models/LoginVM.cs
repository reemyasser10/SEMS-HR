using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Auth.Areas.HR.Models
{
    public class CheckUserVM
    {
        [Required(ErrorMessage = "{0} is required")]
        public required string UserName { get; set; }
    }

    public class UserLoginVM : CheckUserVM
    {
        [Required(ErrorMessage = "{0} is required")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }

   

    public class SelectRoleVM
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
        public required string Role { get; set; }

        public bool IsParent { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsAdmin { get; set; }
    }
}
