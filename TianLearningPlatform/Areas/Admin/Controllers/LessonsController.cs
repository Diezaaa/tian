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

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Edit()
        {
            // Getting the requested lesson and its slides
            var lesson = _db.Lessons.Include(l => l.Slides).FirstOrDefault(l => l.Id == int.Parse(HttpContext.Request.Query["lessonId"]));

            // Getting an ordered lessons list
            var lessonsList = _db.Lessons.ToList().OrderBy(lesson => lesson.Order).ToList();

            /* 
             * Identifying free lessons for displaying them in the drop
             * down list in the view while creating a new lesson
            */

            // Getting the order of the requested lesson for displaying it in the dropdown list as default
            var currentLessonOrder = lesson.Order;
            ViewBag.CurrentLesson = currentLessonOrder;

            // Getting lessons' orders
            var usedOrders = new List<int>();
            foreach (var les in lessonsList)
            {
                usedOrders.Add(les.Order);
            }

            // Finding free orders between lessons
            // and passing the options to the select input
            ViewBag.Orders = new List<SelectListItem> { };
            for (int i = 1; i <= usedOrders.Last() + 1; i++)
            {
                if (lesson.Order == i)
                {
                    ViewBag.Orders.Add(new SelectListItem { Value = i.ToString(), Text = "Lesson " + i.ToString() + ": " + lessonsList.First(lesson => lesson.Order == i).Title });
                }
                else if (!usedOrders.Contains(i))
                {
                    ViewBag.Orders.Add(new SelectListItem { Value = i.ToString(), Text = "Lesson " + i.ToString() + ": " + "[no lesson]" });
                }
                else
                {
                    ViewBag.Orders.Add(new SelectListItem { Value = i.ToString(), Text = "Lesson " + i.ToString() + ": " + lessonsList.First(lesson => lesson.Order == i).Title, Disabled = true });
                }
            }

            return View(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Lesson obj, List<IFormFile> imageFiles)
        {
            // Deleting slide's image by slide order number
            void deleteSlideImageIfExist(int orderNumber)
            {
                if (_db.Slides.FirstOrDefault(p => p.LessonNumber == obj.Slides[0].LessonNumber && p.Order == obj.Slides[orderNumber].Order) != null)
                {
                    // Deleting the old image if it exists
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

            // Giving for all slides the lesson number and numbering lesson's slides
            for (int j = 0; j < obj.Slides.Count; j++)
            {
                obj.Slides[j].Order = j + 1;
                obj.Slides[j].LessonNumber = obj.Order;
            }

            // Copying the id of the lesson that was edited to the edited lesson
            obj.Id = int.Parse(HttpContext.Request.Query["lessonId"]);

            // Manipulation of slides' images
            int i = 0;
            foreach (var image in imageFiles)
            {
                // Deleting the slide's image if it was requested
                if (Request.Form["deleteOriginal " + i] == "on")
                {
                    deleteSlideImageIfExist(i);
                    obj.Slides[i].ImagePath = null;
                }
                //  A txt file is sent if no image has been set.
                //  If an image has been sent to a slide, this condition will be excuted
                else if (Path.GetExtension(image.FileName) != ".txt")
                {
                    // Deleting slide's image if it exist
                    deleteSlideImageIfExist(i);

                    // Getting the folder where the images will be stored
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    // Getting the extension of the desired image to store
                    string extension = Path.GetExtension(image.FileName);

                    // Making a unique identifier for the slide's image path using lesson number and slide number
                    // and then assigning it to the slide object

                    obj.Slides[i].ImagePath = obj.Slides[i].Order.ToString() + obj.Slides[i].LessonNumber.ToString() + extension;

                    // Making a path where the slide will be stored
                    string path = Path.Combine(wwwRootPath + "/images/imagesForSlides", obj.Slides[i].ImagePath);

                    // Saving the slide in the path above
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFiles[i].CopyToAsync(fileStream);
                    }
                }

                // Assigning the image path of the past slides to the new slides object
                else
                {
                    if (await _db.Slides.FirstOrDefaultAsync(s => s.Order == i + 1 && s.LessonNumber == obj.Order) != null)
                    {
                        obj.Slides[i].ImagePath = (await _db.Slides.FirstAsync(s => s.Order == i + 1 && s.LessonNumber == obj.Order)).ImagePath;
                    }
                }
                i++;
            }

            // Deleting the old lesson's slides
            _db.Slides.RemoveRange(_db.Slides.Where(s => s.LessonNumber == _db.Lessons.First(x => x.Id == obj.Id).Order));
            _db.Lessons.Remove(_db.Lessons.First(l => l.Id == int.Parse(HttpContext.Request.Query["lessonId"])));

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                _db.Lessons.Update(obj);
                await _db.SaveChangesAsync();
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

            // Getting the requested lesson
            var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == int.Parse(HttpContext.Request.Query["lessonId"]));

            // Passing the lesson to the view and returning the view
            return View(lesson);
        }
        public IActionResult Details()
        {
            return View(_db.Lessons.Include(l => l.Slides).FirstOrDefault(l => l.Id == int.Parse(HttpContext.Request.Query["lessonId"])));
        }
    }
}
