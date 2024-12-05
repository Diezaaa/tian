using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Web;
using Tian_fullstack.Areas.Account.Models;
using Tian_fullstack.Data;
using Tian_fullstack.Models;


namespace Tian_fullstack.Areas.Learning.Controllers
{
    [Authorize]
    [Area("Learning")]
    public class LearnController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Account.Models.User> _userManager;
        private readonly UserDbContext _userDb;
        public LearnController(ApplicationDbContext db, UserManager<Account.Models.User> userManager, UserDbContext userdb)
        {
            _db = db;
            _userManager = userManager;
            _userDb = userdb;
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index()
        {
            // Getting user's recrods about completed lessons
            var user = await _userManager.GetUserAsync(User);
            var currentUserId = user.Id.ToString();
            var completedLessons =  await _userDb.CompletedLessons.Where(p => p.UserId == currentUserId)
                                    .ToListAsync();

            // Getting and then passing the list of lessons to the view
            var lessonsList = await _db.Lessons.ToListAsync();
            var lessonsListSorted = lessonsList.OrderBy(lesson => lesson.Order).ToList();
            ViewBag.lessons = lessonsListSorted;

            // Default values for users that haven't completed any lesson
            ViewBag.progress = 0;
            ViewBag.lastLessonNumber = 0;

            /* 
             Calculating the progress and finding the last lesson of a user that
             HAS completed at least one lesson
            */
            if (completedLessons.Any())
            {
                var lastLessonNumber = completedLessons.OrderBy(completedLesson => completedLesson.LastLessonNumber)
                                       .ToList()[0].LastLessonNumber;
                ViewBag.progress = (int)(100 * ((double)lastLessonNumber / lessonsList.Count));
                ViewBag.lastLessonNumber = lastLessonNumber;
                return View();
            }

            /* 
             This return statement is excuted only if a user have NOT completed any lesson
            */
            return View();
        }
    }
}
