using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TokenTransfer : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("FromWallet")]
    public Guid FromWalletId { get; set; }
    public Wallet FromWallet { get; set; }

    [ForeignKey("ToWallet")]
    public Guid ToWalletId { get; set; }
    public Wallet ToWallet { get; set; }

    [Column(TypeName = "decimal(18,6)")]
    public decimal Amount { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(20)]
    public string Status { get; set; }

    public bool IsMultiSig { get; set; }

    public ICollection<MultiSigApproval> Approvals { get; set; }
}