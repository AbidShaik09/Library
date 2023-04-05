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
            IEnumerable<Liberaries> objList=_db.Books;
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Create(Liberaries model, IFormFile postedImage)
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
					string? formattedDescription = "<p>" + model.Discription.Replace("\n", "</p><p>") + "</p>";
					_db.Add(model);
					model.Discription= formattedDescription;
					await _db.SaveChangesAsync();

					return RedirectToAction("Index");
				}
			}
			return View(model);
		}

        public IActionResult ViewContent(int ? id)
        {

			{
				if (id == null ||id<0)
				{
					return NotFound();
				}
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
