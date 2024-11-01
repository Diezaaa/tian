using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Web;
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
        [HttpGet]
        public IActionResult Lesson()
        {
            var slides = _db.Slides.ToList();
            ViewBag.orderOfTheLastSlide = 0;
            ViewBag.numberOfSlides = 0;
            foreach(var slide in slides)
            {
                if (slide.LessonNumber.ToString() == HttpContext.Request.Query["lessonId"])
                {
                    ViewBag.orderOfTheLastSlide++;
                    ViewBag.numberOfSlides++;
                }
            }
            foreach (var slide in slides)
            {
                if (slide.LessonNumber.ToString() == HttpContext.Request.Query["lessonId"] && slide.Order.ToString() == HttpContext.Request.Query["slideOrder"])
                {
                    ViewBag.slide = slide;
                }
            }
            int numberOfNulloptionsOfASlide = 0;
            foreach (string option in ViewBag.slide.Options)
            {
                if (option == null)
                {
                    numberOfNulloptionsOfASlide++;
                }
            }
            ViewBag.isAQuiz = true;
            if (numberOfNulloptionsOfASlide == 4)
            {
                ViewBag.isAQuiz = false;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Lesson(int option)
        {
            if (option == 0)
            {
                return Redirect(HttpContext.Request.Headers["Referer"].ToString());
            }    
            var slides = _db.Slides.ToList();
            int? correctAnswer = 0;
            // Previous url
            var referer = Request.Headers["Referer"].ToString();

            // Creattin uri from the previous url
            var uri = new Uri(referer);

            // Get the full query string
            var queryString = uri.Query; // This will give you the full query string

            // Parse the query string to extract specific parameters
            var queryCollection = HttpUtility.ParseQueryString(uri.Query);

            var lessonID = queryCollection["lessonID"];
            var slideOrder = queryCollection["slideOrder"];
            for (int i = 0; i < slides.Count;i++)
            {
                if (slides[i].LessonNumber.ToString() == lessonID)
                {
                    if (slides[i].Order.ToString() == slideOrder)
                    {
                        correctAnswer = slides[i].CorrectOptionIndex;
                    }
                }
            }

            TempData["isCorrect"] = "";
            TempData["isInCorrect"] = "";
            if ((int)option == correctAnswer)
            {
                TempData["isCorrect"] = "It's correct!";
            }
            else 
            {
                TempData["isInCorrect"] = "It's not correct!";
            }
            var refererUrl = HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(refererUrl);
        }

        // Views for admins
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
