using System;
using System.ComponentModel.DataAnnotations;

public class SystemSetting : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Key { get; set; }

    [Required, MaxLength(500)]
    public string Value { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}