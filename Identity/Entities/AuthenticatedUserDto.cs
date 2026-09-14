using Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity.Entities
{
    public class AuthenticatedUserDto
    {
        public UserDto? User { get; set; }
        public TokenResponse? Token { get; set; }
        public TokenResponse? RefreshToken { get; set; }
    }

    public class UserRegistrationDto
    {
        public required string FullName { get; set; }

        public required string UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
        public string? PhonePrefix { get; set; }
        public string? WorkAddress_Governorate { get; set; }
        public string? WorkAddress_City { get; set; }
        public string? WorkAddress_District { get; set; }
        public int? FK_Image { get; set; }
        public required string Password { get; set; }
    }

    public class UserLoginDto : CheckUserExistDto
    {
        public required string Password { get; set; }
    }

    public class ExternalLoginProviderDto
    {
        public ExternalLoginProviderEnum Provider { get; set; }
        public required string Token { get; set; }
    }

    public class CheckUserExistDto
    {
        public required string UserName { get; set; }

    }

    public class UserRefreshTokenDto
    {
        public required string Token { get; set; }
    }
    public class EmailVerificationDto
    {
        public  string Email { get; set; }
    }

    public class UserVerificationDto
    {
        public required string Code { get; set; }
        public required string Email { get; set; }
    }

    public class UserShortLinkDto
    {
        public required string Role { get; set; }
    }

    public class TokenValidationRequestDto
    {
        public required string Token { get; set; }
        public required string Portal { get; set; }
    }

    public class ForgetPasswordDto
    {
        public required string UserName { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress]
        public required string Email { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirm password does not match.")]
        public required string ConfirmPassword { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "{0} is required")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
