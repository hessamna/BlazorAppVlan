// Example of Full EF Core Data Models with ViewModel Support

using Microsoft.AspNetCore.Identity;

namespace BalzorAppVlan.Datas
{
    public class ApplicationUser : IdentityUser<Guid>
        {
            public string? FullName { get; set; }
            public int? CompanyId { get; set; }
        }
    
}