using System;
using System.ComponentModel.DataAnnotations;

public class BurnRecordViewModel
{
    public Guid Id { get; set; }

    public Guid TokenId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateTime BurnedAt { get; set; }

    [MaxLength(250)]
    public string Reason { get; set; }
}