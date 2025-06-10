using System;
using System.ComponentModel.DataAnnotations;

public class TokenViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalSupply { get; set; }
}