using System;
using System.ComponentModel.DataAnnotations;

public class CompanyViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; }

    public bool IsVerified { get; set; }

    [MaxLength(50)]
    public string KycStatus { get; set; }
}