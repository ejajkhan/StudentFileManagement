using Microsoft.AspNetCore.Identity;

namespace StudentStore.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
