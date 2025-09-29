using System;
using System.ComponentModel.DataAnnotations;

public class AuditLog : BaseEntity
{


    [Required, MaxLength(200)]
    public string Action { get; set; }

    [Required, MaxLength(100)]
    public string PerformedBy { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string Description { get; set; }
}