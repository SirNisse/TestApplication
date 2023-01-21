using TestApplication.Models;

namespace TestApplication.ViewModels
{
   
    public class AttachmentsViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DateCreated { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }

    public class AttachmentsRecordViewModel : AttachmentsViewModel
    {
        public int? ParentEntityID { get; set; }
    }

}