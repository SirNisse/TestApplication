using System.ComponentModel.DataAnnotations.Schema;
using TestApplication.Models;

namespace TestApplication.ViewModels
{
    public class AttachmentLibraryViewModel : AttachmentsViewModel
    {
        public int Type { get; set; }
        public int ProjectID { get; set; }
    
        public virtual ICollection<AttachmentsViewModel> Attachments { get; set; }
    }
}
