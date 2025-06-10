using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Wallet : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Address { get; set; }

    [ForeignKey("Company")]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    public ICollection<TokenAllocation> Allocations { get; set; }
    public ICollection<TokenTransfer> SentTransfers { get; set; }
    public ICollection<TokenTransfer> ReceivedTransfers { get; set; }
}