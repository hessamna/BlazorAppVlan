using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MultiSigApproval : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("Transfer")]
    public Guid TransferId { get; set; }
    public TokenTransfer Transfer { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; }

    public bool IsApproved { get; set; }
    public DateTime? ActionAt { get; set; }
}