using System.ComponentModel.DataAnnotations;

public class User : BaseEntity
{
  

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; }

    [Required, MaxLength(20)]
    public string Role { get; set; }

    public Guid? CompanyId { get; set; }
    public Company Company { get; set; }

  
}