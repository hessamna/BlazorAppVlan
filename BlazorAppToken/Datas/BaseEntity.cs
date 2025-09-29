
using System;
using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }

    [MaxLength(450)]
    public string CreatedBy { get; set; } = "System";

    [MaxLength(450)]
    public string ModifiedBy { get; set; }

    [MaxLength(450)]
    public string CreatorIp { get; set; } = "127.0.0.1";

    [MaxLength(450)]
    public string CreatorMachine { get; set; } = "This";
}
