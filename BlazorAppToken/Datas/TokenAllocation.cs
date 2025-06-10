using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TokenAllocation : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("Token")]
    public Guid TokenId { get; set; }
    public Token Token { get; set; }

    [ForeignKey("Wallet")]
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }

    [Column(TypeName = "decimal(18,6)")]
    public decimal Amount { get; set; }

    public bool IsReleased { get; set; }

    public DateTime AllocatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReleasedAt { get; set; }
}