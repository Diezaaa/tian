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
            // Getting the current user
            var user =  await _userManager.GetUserAsync(User);

            // Getting the completed lessons of the user
            var completedLessons = new List<CompletedLesson>();
            if (_userDb.CompletedLessons
                .Where(p => p.UserId == user.Id)
                .Any())
            {
                 completedLessons = await _userDb.CompletedLessons
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            }

            /*
            Getting the number of the lesson by counting the completed lessons,
            if there're no completed lessons 0 is assigned
            */
            var lastLessonNumber = completedLessons.Count;

            // Getting the requested lesson
            var requestedLessonNumber = Int32.Parse(HttpContext.Request.Query["lessonOrder"]);

            /* 
            Redirecing the user if he tries to access a locked lesson
            (a locked lesson is a lesson that is after the last unlocked
            lesson)
            */
            if (lastLessonNumber < requestedLessonNumber
                && !(requestedLessonNumber == 1)
                && !(requestedLessonNumber == lastLessonNumber + 1))
            {
                return RedirectToAction("Index", "Learn", new { area = "Learning" });
            }

            // Getting lesson's slides
            var slides = await _db.Slides
                         .Where(x => x.LessonNumber == requestedLessonNumber)
                         .ToListAsync();

            // Passing the number of slides to the view
            ViewBag.numberOfSlides = slides.Where(x => x.LessonNumber == requestedLessonNumber)
                                     .Count();

            // Getting the requested slide
            var requestedSlideNumber = Int32.Parse(HttpContext.Request.Query["slideOrder"]);
            ViewBag.slide = slides.FirstOrDefault(x => x.Order == requestedSlideNumber);

            /*
            Deciding if a slide has a quiz by cheking if it has a correct answer
            (a correcet answer must presence if a slide has a quiz
            */
            ViewBag.isAQuiz = ViewBag.slide.CorrectOptionIndex == null ? false : true;

            // Adding a new completed lesson to a user
            if (requestedSlideNumber == ViewBag.numberOfSlides 
                && requestedLessonNumber > lastLessonNumber)
            {
                var completedLesson = new CompletedLesson 
                { 
                    LessonNumber = requestedLessonNumber,
                    UpdatedAt = DateTime.Now,
                    UserId = user.Id 
                };
                _userDb.CompletedLessons.Add(completedLesson);
                await _userDb.SaveChangesAsync();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Lesson(int option)
        {
            /* Redirects the user to the quiz if the user submitted illegal data (legal data is */
            if (option != 1 &&
                option != 2 &&
                option != 3 &&
                option != 4)
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
