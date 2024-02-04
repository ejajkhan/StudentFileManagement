using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StudentStore.Data;
using StudentStore.Models;
using System.Diagnostics;
using System;
using System.IO;

namespace StudentStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment,ILogger<HomeController> logger,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var documents = _context.Documents.Where(x => x.UserId == user.Id);
            FileUploadViewModel fv = new FileUploadViewModel
            {
                File = null,
                Documents = documents,
            };
            
            return View(fv);
        }



        public async Task<IActionResult> Download(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var document = _context.Documents.Find(id);
            if (document != null && document.UserId==user.Id)
            {
                var fileBytes= System.IO.File.ReadAllBytes(document.FilePath);
                return File(fileBytes, "application/octet-stream", document.FileName);
            }
            return NotFound();
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var document = _context.Documents.Find(id);
            if (document != null && document.UserId == user.Id)
            {
                var filepath=document.FilePath;
                System.IO.File.Delete(filepath);
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");

            }
            return NotFound();
        }








        [HttpPost]
        public async Task<IActionResult> Upload(FileUploadViewModel model)
        {
            if (model.File != null && model.File.Count > 0)
            {
                foreach(var file in model.File)
                {

                
                    var user = await _userManager.GetUserAsync(User);
                    string usermail= user.Email;
                
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads",usermail);
                    if(!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString()+"_"+ file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var userFile = new Document
                    {
                        FileName = file.FileName,
                        FilePath = filePath,
                        UserId = user.Id
                    };

                    _context.Documents.Add(userFile);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }

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
