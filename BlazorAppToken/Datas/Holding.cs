using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Holding : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; }

    public ICollection<HoldingDetail> Details { get; set; }
}