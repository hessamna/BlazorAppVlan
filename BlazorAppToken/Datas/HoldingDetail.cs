using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HoldingDetail : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid HoldingId { get; set; }
    public Holding Holding { get; set; }

    [MaxLength(150)]
    public string Title { get; set; }

    [MaxLength(250)]
    public string Description { get; set; }
}