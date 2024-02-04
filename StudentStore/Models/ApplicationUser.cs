using Microsoft.AspNetCore.Identity;

namespace StudentStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        public IEnumerable<Document>? Documents { get; set; }
    }
}
