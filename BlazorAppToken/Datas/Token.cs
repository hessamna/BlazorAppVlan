using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Token : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Column(TypeName = "decimal(18,6)")]
    public decimal TotalSupply { get; set; }

    public ICollection<TokenAllocation> Allocations { get; set; }
}