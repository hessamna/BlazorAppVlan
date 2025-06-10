
using System;
using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }

    [MaxLength(100)]
    public string CreatedBy { get; set; }

    [MaxLength(100)]
    public string ModifiedBy { get; set; }

    [MaxLength(45)]
    public string CreatorIp { get; set; }

    [MaxLength(100)]
    public string CreatorMachine { get; set; }
}
