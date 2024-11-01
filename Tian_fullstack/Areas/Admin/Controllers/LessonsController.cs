using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Tian_fullstack.Areas.Learning.Models;
using Tian_fullstack.Data;

namespace Tian_fullstack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment _hostEnvironment;

        public LessonsController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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
                if (image.FileName != " ")
                {
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


            return RedirectToAction("Index");
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Edit()
        {
            var lesson = _db.Lessons.Include(l => l.Slides).FirstOrDefault(l => l.Id == int.Parse(HttpContext.Request.Query["lessonId"]));
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Lesson obj, List<IFormFile> imageFiles)
        {
            void deleteSlideImageIfExist(int orderNumber)
            {
                if (_db.Slides.FirstOrDefault(p => p.LessonNumber == obj.Slides[0].LessonNumber && p.Order == obj.Slides[orderNumber].Order) != null)
                {
                    if (_db.Slides.FirstOrDefault(p => p.LessonNumber == obj.Slides[0].LessonNumber && p.Order == obj.Slides[orderNumber].Order).ImagePath != null)
                    {
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images/imagesForSlides", _db.Slides.FirstOrDefault(p => p.LessonNumber == obj.Slides[0].LessonNumber && p.Order == obj.Slides[orderNumber].Order).ImagePath);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
            }
            for (int j = 0; j < obj.Slides.Count; j++)
            {
                obj.Slides[j].Order = j + 1;
                obj.Slides[j].LessonNumber = obj.Order;
            }

            obj.Id = int.Parse(HttpContext.Request.Query["lessonId"]);

            int i = 0;
            foreach (var image in imageFiles)
            {
                if (image.FileName != " ")
                {
                    deleteSlideImageIfExist(i);
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string extension = Path.GetExtension(image.FileName);
                    obj.Slides[i].ImagePath = obj.Slides[i].Order.ToString() + obj.Slides[i].LessonNumber.ToString() + extension;
                    string path = Path.Combine(wwwRootPath + "/images/imagesForSlides", obj.Slides[i].ImagePath);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFiles[i].CopyToAsync(fileStream);
                    }
                }
                else if (Request.Form["deleteOriginal " + i] == "on")
                {
                    deleteSlideImageIfExist(i);
                    obj.Slides[i].ImagePath = null;
                }
                else
                {
                    foreach (var slide in _db.Slides)
                    {
                        if (slide.Order == obj.Slides[i].Order && slide.LessonNumber == obj.Slides[i].LessonNumber)
                        {
                            obj.Slides[i].ImagePath = slide.ImagePath;
                        }
                    }
                }
                i++;
            }

            _db.Slides.RemoveRange(_db.Slides.Where(s => s.LessonNumber == obj.Order));

            ModelState.Clear();
            if (ModelState.IsValid)
            {

                _db.Lessons.Update(obj);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Delete()
        {
            return View(_db.Lessons.Find(int.Parse(HttpContext.Request.Query["lessonId"])));
        }
        [HttpPost]
        public IActionResult Delete(int lessonId)
        {
            _db.Lessons.Remove(_db.Lessons.Find(int.Parse(HttpContext.Request.Query["lessonId"])));
            var slidesToDelete = _db.Slides.Where(p => p.LessonNumber == _db.Lessons.Find(int.Parse(HttpContext.Request.Query["lessonId"])).Order).ToList();
            _db.Slides.RemoveRange(slidesToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detailes()
        {
            return View(_db.Lessons.Include(l => l.Slides).FirstOrDefault(l => l.Id == int.Parse(HttpContext.Request.Query["lessonId"])));
        }
    }
}
