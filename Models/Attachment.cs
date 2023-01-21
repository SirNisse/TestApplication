using System.ComponentModel.DataAnnotations.Schema;

namespace TestApplication.Models
{
    public class Attachment
    {
        // Attachment info
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }

        public string ContentType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = new byte[0];

        public int AttachmentLibraryId { get; internal set; }
        public AttachmentLibrary AttachmentLibrary { get; set; }
        
    }
}
