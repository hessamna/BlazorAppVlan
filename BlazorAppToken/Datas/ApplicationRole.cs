// Example of Full EF Core Data Models with ViewModel Support

using Microsoft.AspNetCore.Identity;

namespace BalzorAppVlan.Datas
{
    public class ApplicationRole : IdentityRole<Guid>
        {
            public string? Description { get; set; }
        public string? Title { get; set; }
    }
    
}