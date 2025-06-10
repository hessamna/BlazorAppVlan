using System;
using System.ComponentModel.DataAnnotations;

public class MultiSigApprovalViewModel
{
    public Guid Id { get; set; }

    public Guid TransferId { get; set; }

    public Guid UserId { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? ActionAt { get; set; }
}