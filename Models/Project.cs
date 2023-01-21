using System.ComponentModel.DataAnnotations.Schema;
using TestApplication.ViewModels;

namespace TestApplication.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
                   
        public virtual ICollection<AttachmentLibrary> AttachmentLibraries { get; set;}

        public Project()
        {
            AttachmentLibraries = new List<AttachmentLibrary>();
        }

        public Attachment GetCurrentlyActiveAttachment(ICollection<AttachmentLibrary> attachmentLibraries)
        {
            return attachmentLibraries.First(x => x.Type == (int)AttachmentType.ActiveFiles).Attachments
                        .MaxBy(x => x.DateCreated) ?? new Attachment();
        }

        public Project FromVM(EditProjectViewModel vm)
        {
            return new Project
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                AttachmentLibraries = AttachmentLibrary(vm.AttachmentLibrary)//vm.AttachmentLibrary?.att
            };
        }

        public ICollection<AttachmentLibrary> AttachmentLibrary(List<AttachmentLibraryViewModel> library)
        {
            var AttachmentLibrartyerry = library?.Select(x => new AttachmentLibrary
            {
                Id = x.Id,
                Name = x.FileName,
                Type = x.Type,
                ProjectId = x.ProjectID,
                Attachments = (ICollection<Attachment>)x.Attachments.Select(y => new Attachment
                {
                    AttachmentLibraryId = y.Id,
                    ContentType = y.ContentType,
                    DateCreated = DateTime.Parse(x.DateCreated),
                    Description = y.Description,
                    FileName = y.FileName,
                    Id = y.Id
                })
            }) ?? new List<AttachmentLibrary>();

            return (ICollection<AttachmentLibrary>)AttachmentLibrartyerry;
        }
    }
}
