using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tian_fullstack.Data;
using Tian_fullstack.Models;


namespace Tian_fullstack.Controllers
{
    public class RegisteredController : Controller
    {
        // Views for non admin users
        private readonly ApplicationDbContext _db;
        public RegisteredController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Learn()
        {
            var lessonsList = _db.Lessons.ToList().OrderBy(lesson => lesson.Order).ToList();
            return View(lessonsList);
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
        public IActionResult Lesson()
        {
            var slides = _db.Slides.ToList();
            ViewBag.orderOfTheLastSlide = 0;

            foreach(var slide in slides)
            {
                if (slide.LessonId.ToString() == HttpContext.Request.Query["lessonId"])
                {
                    ViewBag.orderOfTheLastSlide++;
                }
            }
            foreach (var slide in slides)
            {
                if (slide.LessonId.ToString() == HttpContext.Request.Query["lessonId"] && slide.Order.ToString() == HttpContext.Request.Query["slideOrder"])
                {
                    ViewBag.slide = slide;
                }
            }
            return View();
        }

        // Views for admins
        public IActionResult AdminPanel()
        {
            ViewBag.tables = _db.Model.GetEntityTypes().Select(t => t.GetTableName()).ToList();
            return View();
        }
    }
}
