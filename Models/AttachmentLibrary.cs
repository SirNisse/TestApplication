using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace TestApplication.Models
{
    public enum AttachmentType
    {
        ActiveFiles
    }


    public class AttachmentLibrary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }

        public int ProjectId { get; internal set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }

        public AttachmentLibrary() => Attachments = new List<Attachment>();
    }
}
