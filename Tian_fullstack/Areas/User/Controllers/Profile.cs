using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Tian_fullstack.Data;

namespace Tian_fullstack.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class Profile : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Account.Models.User> _userManager;
        private readonly UserDbContext _userDb;
        private readonly SignInManager<Areas.Account.Models.User> _signInManager;
        public Profile(ApplicationDbContext db, UserManager<Account.Models.User> userManager, UserDbContext userdb, SignInManager<Areas.Account.Models.User> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _userDb = userdb;
            _signInManager = signInManager;
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentUserId = user.Id.ToString();
            var lessonsList = _db.Lessons.ToList();
            var userCompletedLessons = await _userDb.CompletedLessons.Where(p => p.UserId == currentUserId).ToListAsync();
            var lastLessonNumber = userCompletedLessons.Any() ? (userCompletedLessons.OrderByDescending(p => p.LastLessonNumber).ToList()[0]).LastLessonNumber : 0;
            ViewBag.iSFirstLesson = lastLessonNumber >= 1;
            ViewBag.percents = (int)(100 * ((double)lastLessonNumber / lessonsList.Count));
            ViewBag.user = user;

            // Calculating days streak
            if (lastLessonNumber == 0)
            {
                ViewBag.streak = 0;
                return View();
            }
            int streak = 1;
            var userCompletedLessonsSorted = userCompletedLessons.OrderByDescending(p => p.UpdatedAt).ToList();
            var today = DateTime.Now;
            var oneDay = new TimeSpan(days: 1, 0, 0, 0);
            var lastLesson = userCompletedLessonsSorted[0];
            if (today.Date < lastLesson.UpdatedAt.Date - oneDay)
            {
                ViewBag.streak = 0;
                return View();
            }
            int j = 1;
            for (int i = 0; i < userCompletedLessonsSorted.Count; i++)
            {
                for (; j < userCompletedLessonsSorted.Count;)
                {
                    if (userCompletedLessonsSorted[i].UpdatedAt.Date == userCompletedLessonsSorted[j].UpdatedAt.Date + oneDay )
                    {
                        streak++;
                        j = j + 1;
                        break;
                    }
                    else
                    {
                        ViewBag.streak = streak;
                        return View();
                    }                
                }
            }
            ViewBag.streak = streak;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
        public async Task<IActionResult> DeleteAcc ()
        {
            await _signInManager.SignOutAsync();
            var currentUser = await _userManager.GetUserAsync(User);
            _userDb.Users.Remove(currentUser);
            _userDb.SaveChanges();
            return Redirect("/");

        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}
