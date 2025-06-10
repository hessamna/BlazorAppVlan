using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; }

    [Required, MaxLength(20)]
    public string Role { get; set; }

    public Guid? CompanyId { get; set; }
    public Company Company { get; set; }

    public ICollection<MultiSigApproval> Approvals { get; set; }
}