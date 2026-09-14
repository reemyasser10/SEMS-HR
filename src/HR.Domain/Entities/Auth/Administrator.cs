namespace HR.Domain.Entities.Auth;

using HR.Domain.Common;

public class Administrator : AuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
