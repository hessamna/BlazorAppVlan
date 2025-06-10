using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BurnRecord : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid TokenId { get; set; }
    public Token Token { get; set; }

    [Column(TypeName = "decimal(18,6)")]
    public decimal Amount { get; set; }

    public DateTime BurnedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(250)]
    public string Reason { get; set; }
}