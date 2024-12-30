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
        public async Task<int> CalculateProgress()
        {
            // Getting asynchronously two essential pieces of info for the calculation
            var userTask = _userManager.GetUserAsync(User);
            var lessonsTask = _db.Lessons.ToListAsync();

            // Waiting to the tasks above to complete
            await Task.WhenAll(userTask, lessonsTask);

            // Retrieving the results of the tasks above
            var user = await userTask;
            var lessonsList = await lessonsTask;

            // Getting more details about the user for further calculations
            var userCompletedLessons = await _userDb.CompletedLessons.Where(p => p.UserId == user.Id).ToListAsync();
            var lastCompletedLessonNumber = userCompletedLessons.Any() ? (userCompletedLessons.OrderByDescending(p => p.LessonNumber).ToList()[0]).LessonNumber : 0;


            // Calculating progress
            if (lessonsList.Any())
            {
                var lastDbLesson = lessonsList.OrderByDescending(l => l.Order)
                                 .ToList()[0].Order;

                // If there're no lessons in the DB the progress will be 0
                if (lastDbLesson == 0)
                {
                    return 0;
                }

                var progress = ((double)lastCompletedLessonNumber / lastDbLesson) * 100;
                return Math.Min((int)progress, 100); // Ensuring the value doesn't exceed 100
            }
            return 0;
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

            // Passing the sorted lessons list to the view
            ViewBag.lessons = lessonsList.OrderBy(x => x.Order)
                              .ToList();

            // Calculating progress for displaying
            ViewBag.progressInPercentage = await CalculateProgress();

            // Getting the number of the last completed lesson
            if (_userDb.CompletedLessons.Where(x => x.UserId == user.Id).Any())
            {
                var completedLesson = await _userDb.CompletedLessons.Where(x => x.UserId == user.Id).ToListAsync();
                ViewBag.lastLessonNumber = completedLesson.OrderBy(l => l.LessonNumber).ToList().Last().LessonNumber;
                return View();
            }
            ViewBag.lastLessonNumber = 0;
            return View();
        }
    }
}
