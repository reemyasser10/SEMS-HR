namespace HR.Domain.Entities.System;

using HR.Domain.Common;

public class AuditLog : Entity
{
    public string UserId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Create, Update, Delete
    public string TableName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string OldValues { get; set; } = string.Empty; // JSON
    public string NewValues { get; set; } = string.Empty; // JSON
    public string AffectedColumns { get; set; } = string.Empty; // JSON
    public string PrimaryKey { get; set; } = string.Empty; // JSON
}
