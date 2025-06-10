using System;
using System.ComponentModel.DataAnnotations;

public class WalletViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Address { get; set; }

    public Guid CompanyId { get; set; }
}