// ViewModel for AuditLog
using System;
using System.ComponentModel.DataAnnotations;

public class AuditLogViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Action { get; set; }

    [Required, MaxLength(100)]
    public string PerformedBy { get; set; }

    public DateTime Timestamp { get; set; }

    public string Description { get; set; }
}