using System;
using System.ComponentModel.DataAnnotations;

public class TokenTransferViewModel
{
    public Guid Id { get; set; }

    public Guid FromWalletId { get; set; }

    public Guid ToWalletId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateTime RequestedAt { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; }

    public bool IsMultiSig { get; set; }
}