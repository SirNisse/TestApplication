using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TestApplication.Models;

namespace TestApplication.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }      
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        public IFormFile? UploadFiles { get; set; }// While creating a Project        
    }

    public class EditProjectViewModel : ProjectViewModel
    {   
        public AttachmentsViewModel AttachmentCurrentlyActive { get; set; } = new AttachmentsViewModel(); // Active attachment of the Project
        public List<AttachmentsViewModel>? AttachmentsRecord { get; set; }
        public List<AttachmentLibraryViewModel>? AttachmentLibrary { get; set; }

        public EditProjectViewModel()
        {
            AttachmentLibrary = new List<AttachmentLibraryViewModel>();
            AttachmentsRecord = new List<AttachmentsViewModel>();
        }

        public static List<AttachmentLibraryViewModel> Library(ICollection<AttachmentLibrary> library)
        {
            return library.Where(x => x.Type != (int)AttachmentType.ActiveFiles)
                        .Select(x => new AttachmentLibraryViewModel()
                        {
                            Attachments = x.Attachments.Select(y => new AttachmentsViewModel()
                            {
                                FileName = y.FileName,
                                ContentType = y.ContentType,
                                DateCreated = y.DateCreated.ToShortDateString(),
                                Description = y.Description,
                                Id = y.Id
                            }).ToList(),
                            Type = x.Type
                        }).ToList();
        }

        public static AttachmentsViewModel CurrentlyActiveAttachment(ICollection<AttachmentLibrary> library)
        {
            Attachment? activeDocument = library
                        .First(x => x.Type == (int)AttachmentType.ActiveFiles).Attachments
                        .MaxBy(x => x.DateCreated);

            return new AttachmentsViewModel()
                        {
                            FileName = activeDocument?.FileName ?? "",
                            ContentType = activeDocument?.ContentType ?? "",
                            DateCreated = activeDocument?.DateCreated.ToShortDateString() ?? "",
                            Description = activeDocument?.Description ?? "",
                            Id = activeDocument?.Id ?? 0
                        };
        }

        public static List<AttachmentsViewModel> AttachmentRecord(ICollection<AttachmentLibrary> library)
        {
            Attachment? activeDocument = library
                        .First(x => x.Type == (int)AttachmentType.ActiveFiles).Attachments
                        .MaxBy(x => x.DateCreated);

            return library.First(x => x.Type == (int)AttachmentType.ActiveFiles).Attachments
                                            .Where(x => x != activeDocument).OrderByDescending(x => x.DateCreated)
                                            .Select(x => new AttachmentsViewModel()
                                            {
                                                FileName = x.FileName,
                                                ContentType = x.ContentType,
                                                DateCreated = x.DateCreated.ToShortDateString(),
                                                Description = x.Description,
                                                Id = x.Id
                                            }).ToList();
        }

        public static EditProjectViewModel FromProject(Project project)
        {
            return new EditProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                AttachmentLibrary = EditProjectViewModel.Library(project.AttachmentLibraries),
                AttachmentCurrentlyActive = EditProjectViewModel.CurrentlyActiveAttachment(project.AttachmentLibraries),
                AttachmentsRecord = EditProjectViewModel.AttachmentRecord(project.AttachmentLibraries)
            };
        }

    }
}
