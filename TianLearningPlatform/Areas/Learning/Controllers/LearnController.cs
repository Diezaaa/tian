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
            /* 
             Note: the following two tasks (A task is asynchronous 
             instruction) is excuted asynchronously
            */

            // Getting the current user
            var userTask = _userManager.GetUserAsync(User);

            // Getting the list of all lessons
            var lessonsListTask = _db.Lessons.ToListAsync();

            // Waiting for the completion of the two tasks above
            await Task.WhenAll(userTask, lessonsListTask);


            // Extracting the values of the completed two tasks above
            var user = await userTask;
            var lessonsList = await lessonsListTask;

            // Passing the sorted lesosns list to the view
            ViewBag.lessons = lessonsList.OrderBy(x => x.Order)
                              .ToList();

            // Getting the completed lessons of the user
            var completedLessons = _userDb.CompletedLessons
                                   .Where(x => x.UserId == user.Id)
                                   .Any() ? await _userDb.CompletedLessons.Where(x => x.UserId == user.Id).ToListAsync() : new List<CompletedLesson>();

            // Default values for users that haven't completed any lesson
            ViewBag.progress = 0;
            ViewBag.lastLessonNumber = 0;

            /* 
             Calculating the progress and finding the last lesson of a user that
             HAS completed at least one lesson
            */
            if (completedLessons.Any())
            {
                // Getting the number of the last completed lesson
                int lastLessonNumber = completedLessons.Count;
                ViewBag.lastLessonNumber = lastLessonNumber;
                ViewBag.progress = (int)(100 * ((double)lastLessonNumber / lessonsList.Count));
                return View();
            }

            /*
             This return statement is excuted only if a user have NOT completed any lesson
            */
            return View();
        }
    }
}
