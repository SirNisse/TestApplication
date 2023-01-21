using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
using TestApplication.DAL;
using TestApplication.Models;
using TestApplication.ViewModels;

namespace TestApplication.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly ILogger<HomeController> _logger;

        public IActionResult Index()
        {
            return View();
        }

       
        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditProjectViewModel))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EditProject(int id)
        {
            EditProjectViewModel vm;
            using (ApplicationDbContext db = new())
            {
                Project? m = db.Projects
                            .Include(x => x.AttachmentLibraries)
                            .ThenInclude(y => y.Attachments)
                            .First(x => x.Id == id);

                if (m is null)
                return NotFound();


                Attachment? activeDocument = m.AttachmentLibraries
                        .First(x => x.Type == (int)AttachmentType.ActiveFiles).Attachments
                        .MaxBy(x => x.DateCreated);

                vm = new() 
                { 
                    Id = m.Id,
                    Title= m.Title,
                    Description= m.Description,
                    AttachmentLibrary = m.AttachmentLibraries
                    .Where(x => x.Type != (int)AttachmentType.ActiveFiles)
                        .Select(x => new AttachmentLibraryViewModel()
                        {
                            Attachments = x.Attachments.Select(y => new AttachmentsViewModel()
                            {
                                FileName = y.FileName,
                                ContentType= y.ContentType,
                                DateCreated = y.DateCreated.ToShortDateString(),
                                Description= y.Description, 
                                Id = y.Id
                            }).ToList(),
                            Type= x.Type
                        }).ToList(),
                    AttachmentCurrentlyActive = activeDocument is null ? new AttachmentsViewModel() : new AttachmentsViewModel() 
                    {
                        FileName = activeDocument.FileName,
                        ContentType = activeDocument.ContentType,
                        DateCreated = activeDocument.DateCreated.ToShortDateString(),
                        Description = activeDocument.Description,
                        Id  = activeDocument.Id
                    },
                    AttachmentsRecord = m.AttachmentLibraries.First(x => x.Type == (int)AttachmentType.ActiveFiles).Attachments
                                            .Where(x => x != activeDocument).OrderByDescending(x => x.DateCreated)
                                            .Select(x => new AttachmentsViewModel()
                                            {
                                                FileName = x.FileName,
                                                ContentType = x.ContentType,
                                                DateCreated = x.DateCreated.ToShortDateString(),
                                                Description = x.Description,
                                                Id = x.Id
                                            }).ToList()
                };
            }
            return vm is null ? NotFound() : View("Views/Project/Edit.cshtml", vm);
        }

        [HttpPost]        
        public IActionResult EditProject(EditProjectViewModel vm)
        {
            using ApplicationDbContext db = new();
            Project? project = db.Projects
                .Include(x => x.AttachmentLibraries)
                .ThenInclude(y => y.Attachments)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == vm.Id);

            // Guard statements
            if (object.Equals(project, null)) { return NotFound(); }
     
            if (ModelState.IsValid)
            {
                project = project.FromVM(vm);
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(project).GetDatabaseValues();
            }

            vm = EditProjectViewModel.FromProject(project);

            return View("Views/Project/Edit.cshtml", vm);
        }




        [HttpGet]
        public IActionResult CreateProject()
        {
            return View("Views/Project/Create.cshtml", new ProjectViewModel { });
        }


        [HttpPost]
        public IActionResult CreateProject(ProjectViewModel vm)
        {
            #region Guards
            // Guard statement
            //if (vm.UploadFiles is null)
            //    ModelState.AddModelError("UploadFiles", "You need a file to create a Project");
                    
            if (!ModelState.IsValid)
            {
                return View("Views/Project/Create.cshtml", vm);
            }
            #endregion

            Project Project = new()
            {
                Title = vm.Title,
                Description = vm.Description
            };
            using (ApplicationDbContext db = new())
            {
                db.Projects.Add(Project);
                db.SaveChanges();
                db.Entry(Project).GetDatabaseValues();

                Project.AttachmentLibraries.Add(new AttachmentLibrary()
                {
                    Type = (int)AttachmentType.ActiveFiles,                 
                    Attachments = new List<Attachment>()
                });
                db.Entry(Project).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(Project).GetDatabaseValues();

                if (vm.UploadFiles is not null)
                {
                    string fileName = Path.GetFileName(vm.UploadFiles.FileName); // Already checked if file is null in guard
                    string contentType = vm.UploadFiles.ContentType;
                    using (MemoryStream ms = new())
                    {
                        vm.UploadFiles.CopyTo(ms);

                        Attachment nF = new()
                        {
                            FileName = fileName,
                            Description = "Initial file at Project create",
                            DateCreated = DateTime.Now,
                            ContentType = contentType,
                            Data = ms.ToArray()
                        };

                        Project.AttachmentLibraries.FirstOrDefault(x => x.Type == (int)AttachmentType.ActiveFiles)?.Attachments.Add(nF);
                        db.Entry(Project).State = EntityState.Modified;
                        db.SaveChanges();
                        db.Entry(Project).GetDatabaseValues();
                    }
                }           
            }

            return RedirectToAction("EditProject", new { id = Project.Id });
            //return View("Views/Project/Edit.cshtml", new { id = Project.Id });
        }



        

        //public ActionResult AttachmentListSorting(string sortOrder, int id)
        //{
        //    ViewBag.CreatedSortParm = String.IsNullOrEmpty(sortOrder) ? "created_desc" : "";
        //    ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
        //    ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";
        //    ViewBag.ContentTypeSortParm = sortOrder == "ContentType" ? "contenttype_desc" : "Title";

        //    IEnumerable<AttachmentsViewModel>? attachments;
        //    using (var db = new ApplicationDbContext())
        //    {
        //        //Project = (ProjectViewModel)db.Projects.Where(x => x.Id == id).FirstOrDefault()
        //        //    .Select(x => new ProjectViewModel
        //        //    {
        //        //        Title = x.Title,
        //        //        Description = x.Description,
        //        //        AttachmentLibrary = x.AttachmentLibrary                         
        //        //    });             


        //        // db.AttachmentLibraries.Where(x => x.ProjectID == id).FirstOrDefault()?.Attachments
        //        attachments = db.Projects.Find(id)?.AttachmentLibrary.First().Attachments
        //                            .Select(x => new AttachmentsRecordViewModel
        //                            {
        //                                Id = x.Id,
        //                                Title = x.Title,
        //                                DateCreated = x.DateCreated.ToShortDateString(),
        //                                Description = x.Description,
        //                                ContentType = x.ContentType,

        //                                ParentEntityID = id
        //                            });
        //    }


        //    if (attachments is null)
        //        return View("NotFound.cshtml");


        //    switch (sortOrder)
        //    {
        //        case "created_desc":
        //            attachments = attachments.OrderByDescending(a => a.DateCreated);
        //            break;
        //        case "Title":
        //            attachments = attachments.OrderBy(a => a.Title);
        //            break;
        //        case "title_desc":
        //            attachments = attachments.OrderByDescending(a => a.Title);
        //            break;
        //        case "Description":
        //            attachments = attachments.OrderBy(a => a.Description);
        //            break;
        //        case "description_desc":
        //            attachments = attachments.OrderByDescending(a => a.Description);
        //            break;
        //        case "ContentType":
        //            attachments = attachments.OrderBy(a => a.ContentType);
        //            break;
        //        case "contentType_desc":
        //            attachments = attachments.OrderByDescending(a => a.ContentType);
        //            break;
        //        default:
        //            attachments = attachments.OrderBy(a => a.DateCreated);
        //            break;
        //    }

        //    return PartialView("/Views/Shared/Partials/AttachmentList.cshtml", attachments);
        //}
    }
}
