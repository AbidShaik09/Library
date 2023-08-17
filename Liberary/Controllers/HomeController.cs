using Liberary.Data;
using Liberary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Liberary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ApplicationDbContext db, ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Liberary");
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserModel obj)
        {
            var validuser=_db.Users.Any(x=>x.UserName == obj.UserName && x.Password==obj.Password);
            if (validuser)
            {
                HttpContext.Session.SetString("UserName",obj.UserName);
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index","Liberary");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel obj, IFormFile postedImage)
        {
            obj.profilepic = "";
            if (ModelState.IsValid)
            {
                var userexist=_db.Users.Any(x=>x.UserName==obj.UserName);
                if(userexist)
                {
                    return View();
                }

                var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "DisplayPictures");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                if (postedImage != null && postedImage.Length > 0)
                {

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedImage.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "DisplayPictures", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await postedImage.CopyToAsync(fileStream);
                    }
                    obj.profilepic = "/DisplayPictures/" + fileName;

                }
                _db.Users.Add(obj);
                _db.SaveChanges();
                HttpContext.Session.SetString("UserName", obj.UserName);
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
    }
}