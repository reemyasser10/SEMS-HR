namespace Shared.DTOs
{
    public class DataDto
    {
        public object? Value { get; set; }
    }
    public class DataBoolenDto : DataDto
    {
        public new bool? Value { get; set; }
    }
    public class DataIntDto : DataDto
    {
        public new int? Value { get; set; }
    }
    public class UserInfoDto
    {
        public string FullName { get; set; }
        public bool IsEmailVerified { get; set; }
    }

    public class ValidationResultDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int? Value { get; set; }
    }
}
