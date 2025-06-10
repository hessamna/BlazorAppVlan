using System;
using System.ComponentModel.DataAnnotations;

public class SupportTicketViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(150)]
    public string Subject { get; set; }

    [Required, MaxLength(1000)]
    public string Message { get; set; }

    [Required, MaxLength(100)]
    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    [MaxLength(20)]
    public string Status { get; set; }
}