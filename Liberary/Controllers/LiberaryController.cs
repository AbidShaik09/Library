using Liberary.Data;
using Liberary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Liberary.Controllers
{
    public class LiberaryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LiberaryController(ApplicationDbContext db ,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
			_webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Libraries> objList=_db.Books;
            return View(objList);
        }

        public IActionResult Create()
        {
			if (HttpContext.Session.GetString("UserName") == null)
			{
				return RedirectToAction("Login","Home");
			}
			ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Home");
            }
			List<UserModel> user = _db.Users.Where(u => u.UserName == HttpContext.Session.GetString("UserName")).ToList();
            List<Libraries> Articles=_db.Books.Where(u => u.UserName == HttpContext.Session.GetString("UserName")).ToList();
            ViewBag.Articles = Articles;
			ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View(user[0]);
        }

		public IActionResult EditPage()
		{
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<UserModel> user = _db.Users.Where(u => u.UserName == HttpContext.Session.GetString("UserName")).ToList();
            List<Libraries> Articles = _db.Books.Where(u => u.UserName == HttpContext.Session.GetString("UserName")).ToList();
            ViewBag.Articles = Articles;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View(user[0]);
        }
		
        public IActionResult Edit(int Id)
		{
			var article =  _db.Books.Where(u=>u.Id == Id);

			return View(article);
		}
        public IActionResult Delete(int ? id)
		{
            if (id == null || id < 0)
            {
                return NotFound();
            }

            var bookFromDb = _db.Books.Find(id);

            if (bookFromDb == null)
            {
                return NotFound();
            }
            _db.Books.Remove(bookFromDb);
			_db.SaveChanges();
			return RedirectToAction("EditPage");
		}

        [HttpPost]
		public async Task<IActionResult> Create(Libraries model, IFormFile postedImage)
		{
			if (ModelState.IsValid)
			{
				var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}
				if (postedImage != null && postedImage.Length > 0)
				{
					var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedImage.FileName);
					var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await postedImage.CopyToAsync(fileStream);
					}

					model.ImgPath = "/images/" + fileName;
					string? formattedDescription = "<p>" + model.Description.Replace("\n", "</p><p>") + "</p>";

					model.UserName = HttpContext.Session.GetString("UserName");
					_db.Add(model);
					model.Description= formattedDescription;
					await _db.SaveChangesAsync();

					return RedirectToAction("Index");
				}
			}
			return View(model);
		}

        public IActionResult ViewContent(int ? id)
        {

			
				if (id == null ||id<0)
				{
					return NotFound();
				}
			
			var bookFromDb = _db.Books.Find(id);

			if(bookFromDb == null)
			{
				return NotFound();
			}
			return View(bookFromDb);
            
        }

    }
}
