namespace StudentStore.Models
{
    public class FileUploadViewModel
    {
        public List<IFormFile>? File { get; set; }
        public IEnumerable<Document>? Documents { get; set; }
    }
}
