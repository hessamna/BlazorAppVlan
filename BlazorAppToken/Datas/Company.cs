using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Company : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; }

    public bool IsVerified { get; set; }

    [MaxLength(50)]
    public string KycStatus { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<Wallet> Wallets { get; set; }
}