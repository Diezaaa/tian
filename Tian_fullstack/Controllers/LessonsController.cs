using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Tian_fullstack.Data;
using Tian_fullstack.Models;

namespace Tian_fullstack.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment _hostEnvironment;

        public LessonsController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            var lessonsList = _db.Lessons.ToList().OrderBy(lesson => lesson.Order).ToList();
            return View(lessonsList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Lesson obj, List<IFormFile> imageFiles)
        {
            for (int j = 0; j < obj.Slides.Count; j++)
            {
                obj.Slides[j].Order = j + 1;
                obj.Slides[j].LessonNumber = obj.Order;
            }
            int i = 0;
            foreach (var image in imageFiles)
            {
                if (image.FileName != " ") { 
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string extension = Path.GetExtension(image.FileName);
                    obj.Slides[i].ImagePath = obj.Slides[i].Order.ToString() + obj.Slides[i].LessonNumber.ToString() + extension;
                    string path = Path.Combine(wwwRootPath + "/images/imagesForSlides", obj.Slides[i].ImagePath);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFiles[i].CopyToAsync(fileStream);
                    }
                    }
                i++;
            }

            ModelState.Clear();
            if (ModelState.IsValid) 
            {

                _db.Lessons.Add(obj);
                _db.SaveChanges();
            }


            return View();
        }

    }
}
