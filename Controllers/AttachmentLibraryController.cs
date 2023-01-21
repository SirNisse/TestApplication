using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TestApplication.DAL;
using TestApplication.Models;

namespace TestApplication.Controllers
{
    public class AttachmentLibraryController : ControllerBase
    {

        [HttpPost]
        public IActionResult UploadFile(IFormFile postedFile, int id, int type)
        {
            string fileName = Path.GetFileName(postedFile.FileName);
            string contentType = postedFile.ContentType;

            using (MemoryStream ms = new())
            {
                postedFile.CopyTo(ms);

                // I don't want to make another db is we already got one up
                using (ApplicationDbContext db = new())
                {
                    try 
                    {
                        AttachmentLibrary? library = db?.AttachmentLibraries.Where(x => x.Type == type)
                            .Include(x => x.Attachments)
                            .Include(x => x.Project).FirstOrDefault(x => x.Project.Id == id);

                        Attachment nF = new()
                        {
                            FileName = fileName,
                            Description = "Some description",
                            DateCreated = DateTime.Now,
                            ContentType = contentType,
                            Data = ms.ToArray(),
                        };

                        library?.Attachments.Add(nF);
                    }
                    catch(Exception ex) 
                    {
                        return Ok();
                        //TODO: Setup logging for the application
                    }

                    db?.SaveChanges();
                }
            }
            return RedirectToAction("EditProject", "Project", new { id });
        }

        //[HttpPost]
        //public IActionResult DownloadFile(int fileId)
        //{
        //    byte[] bytes;
        //    string fileName, contentType;
        //    string constr = this.Configuration.GetConnectionString("MyConn");
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandText = "SELECT Name, Data, ContentType FROM tblFiles WHERE Id=@Id";
        //            cmd.Parameters.AddWithValue("@Id", fileId);
        //            cmd.Connection = con;
        //            con.Open();
        //            using (SqlDataReader sdr = cmd.ExecuteReader())
        //            {
        //                sdr.Read();
        //                bytes = (byte[])sdr["Data"];
        //                contentType = sdr["ContentType"].ToString();
        //                fileName = sdr["Name"].ToString();
        //            }
        //            con.Close();
        //        }
        //    }

        //    return File(bytes, contentType, fileName);
        //}

        //private List<FileModel> GetFiles()
        //{
        //    List<FileModel> files = new List<FileModel>();
        //    string constr = this.Configuration.GetConnectionString("MyConn");
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM tblFiles"))
        //        {
        //            cmd.Connection = con;
        //            con.Open();
        //            using (SqlDataReader sdr = cmd.ExecuteReader())
        //            {
        //                while (sdr.Read())
        //                {
        //                    files.Add(new FileModel
        //                    {
        //                        Id = Convert.ToInt32(sdr["Id"]),
        //                        Name = sdr["Name"].ToString()
        //                    });
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    return files;
        //}




    }
}
