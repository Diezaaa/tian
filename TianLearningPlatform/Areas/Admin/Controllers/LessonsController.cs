using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Tian_fullstack.Areas.Learning.Models;
using Tian_fullstack.Data;

namespace Tian_fullstack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Index()
        {
            // Getting an ordered lessons list
            var lessonsList = await _db.Lessons.OrderBy(lesson => lesson.Order).ToListAsync();
            return View(lessonsList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Getting an ordered lessons list
            var lessonsList = _db.Lessons.ToList().OrderBy(lesson => lesson.Order).ToList();

            
            if (lessonsList.Any())
            {
                // Getting lessons' orders
                var usedOrders = new List<int>();
                foreach (var lesson in lessonsList)
                {
                    usedOrders.Add(lesson.Order);
                }

                // Finding free orders between lessons
                // and passing the options to the select input
                ViewBag.Orders = new List<SelectListItem> { };
                for (int i = 1; i <= usedOrders.Last() + 1; i++)
                {
                    if (!usedOrders.Contains(i))
                    {
                        ViewBag.Orders.Add(new SelectListItem { Value = i.ToString(), Text = "Lesson " + i.ToString() + ": " + "[no lesson]" });
                    }
                    else
                    {
                        ViewBag.Orders.Add(new SelectListItem { Value = i.ToString(), Text = "Lesson " + i.ToString() + ": " + lessonsList.First(lesson => lesson.Order == i).Title, Disabled = true });
                    }
                }
            }
            // If there's no lessons, only option for the first lesson will be available
            else
            {
                ViewBag.Orders = new List<SelectListItem> { };
                ViewBag.Orders.Add(new SelectListItem { Value = "1", Text = "Lesson " + "1" + ": " + "[no lesson]" });
            }

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
                if (Path.GetExtension(image.FileName) != ".txt")
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

        public async Task <IActionResult> Delete()
        {
            // Getting the requested lesson
            var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == int.Parse(HttpContext.Request.Query["lessonId"]));

            // Passing the lesson to the view and returning the view
            return View(lesson);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(bool isConfirmed)
        {
            // Checking if the user confirmed the deletion
            if (isConfirmed)
            {
                // FIx deletion
                var lessons = _db.Lessons.Include(b => b.Slides).First(l => l.Id == int.Parse(HttpContext.Request.Query["lessonId"]));
                _db.Slides.RemoveRange(lessons.Slides);
                _db.Lessons.Remove(lessons);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Details()
        {
            return View(_db.Lessons.Include(l => l.Slides).FirstOrDefault(l => l.Id == int.Parse(HttpContext.Request.Query["lessonId"])));
        }
    }
}
