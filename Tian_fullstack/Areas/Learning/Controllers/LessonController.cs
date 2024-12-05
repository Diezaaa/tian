using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Tian_fullstack.Areas.Account.Models;
using Tian_fullstack.Areas.Learning.Models;
using Tian_fullstack.Data;


namespace Tian_fullstack.Areas.Learning.Controllers
{
    [Area("Learning")]
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserDbContext _userDb;
        private readonly UserManager<Account.Models.User> _userManager;
        public LessonController(ApplicationDbContext db, UserDbContext userdb, UserManager<Account.Models.User> userManager)
        {
            _db = db;
            _userManager = userManager;
            _userDb = userdb;
        }
        public async Task <IActionResult> Lesson()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentUserId = user.Id.ToString();
            var userCompletedLessons = await _userDb.CompletedLessons.Where(p => p.UserId == currentUserId).ToListAsync();
            var lastLessonNumber = userCompletedLessons.OrderBy(p => p.LastLessonNumber).Any() ?  (userCompletedLessons.OrderBy(p => p.LastLessonNumber).ToList()[0]).LastLessonNumber : -1;
            // If a user enters a lesson that is two lessons ahead, he will be returned to learn page 
            if (lastLessonNumber > Int32.Parse(HttpContext.Request.Query["lessonOrder"]) && Int32.Parse(HttpContext.Request.Query["lessonOrder"]) != 1)
            {
                return RedirectToAction("Index", "Learn", new { area = "Learning" });
            }
            var slides = _db.Slides.ToList();
            ViewBag.numberOfSlides = 0;
            foreach (var slide in slides)
            {
                if (slide.LessonNumber.ToString() == HttpContext.Request.Query["lessonOrder"])
                {
                    ViewBag.numberOfSlides++;
                }
            }
            foreach (var slide in slides)
            {
                if (slide.LessonNumber.ToString() == HttpContext.Request.Query["lessonOrder"] && slide.Order.ToString() == HttpContext.Request.Query["slideOrder"])
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

            // Update user's progress
            if (Int32.Parse(HttpContext.Request.Query["slideOrder"]) == ViewBag.numberOfSlides && Int32.Parse(HttpContext.Request.Query["lessonOrder"]) > lastLessonNumber)
            {
                var newUserProgress = new CompletedLesson { LastLessonNumber = Int32.Parse(HttpContext.Request.Query["lessonOrder"]), UpdatedAt = DateTime.Now, UserId = currentUserId };
                _userDb.CompletedLessons.Add(newUserProgress);
                _userDb.SaveChanges();
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

            var lessonID = queryCollection["lessonOrder"];
            var slideOrder = queryCollection["slideOrder"];
            for (int i = 0; i < slides.Count; i++)
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
            if (option == correctAnswer)
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
    }
}
