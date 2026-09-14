namespace Identity.Entities
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
     
        public bool IsAdmin { get; set; }
        public int? TenantId { get; set; }

    }

}
