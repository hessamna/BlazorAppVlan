using System;
using System.ComponentModel.DataAnnotations;

public class SmartContractViewModel
{
    public Guid Id { get; set; }

    [Required, MaxLength(150)]
    public string Address { get; set; }

    [MaxLength(50)]
    public string Version { get; set; }

    public DateTime DeployedAt { get; set; }

    [MaxLength(20)]
    public string Status { get; set; }
}