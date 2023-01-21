using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System;
using TestApplication.Models;
using TestApplication.DAL;
using TestApplication.ViewModels;

namespace TestApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;      

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

   
        public IActionResult Index()
        {      
            HomeViewModel vm = new HomeViewModel();
            //var att = _context.Attachments
            //    .Select(x => new AttachmentsViewModel() { Id = x.Id, Title = x.Title, Description = x.Description, ContentType = x.ContentType });

            //vm.Attachments = att; 

            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}