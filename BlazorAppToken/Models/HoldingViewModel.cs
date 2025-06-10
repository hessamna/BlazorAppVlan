using System;
using System.ComponentModel.DataAnnotations;

public class HoldingViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; }

    [Required]
    public bool IsActive { get; set; }
}