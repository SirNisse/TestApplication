using TestApplication.ViewModels;

namespace TestApplication.ViewModels
{
    public class HomeViewModel
    {
        //public ICollection<Attachments> Attchments { get; set; } = new List<Attachments>();
        //public Attachments? Attachment { get; set; }
        public IEnumerable<AttachmentsViewModel> Attachments { get; set; } = Enumerable.Empty<AttachmentsViewModel>();
    }
}