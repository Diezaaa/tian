using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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
        private IWebHostEnvironment _hostEnvironment;

        public Profile(ApplicationDbContext db, UserManager<Account.Models.User> userManager, UserDbContext userdb, SignInManager<Areas.Account.Models.User> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _userManager = userManager;
            _userDb = userdb;
            _signInManager = signInManager;
            _hostEnvironment = webHostEnvironment;

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
            var user = await _userManager.GetUserAsync(User);
            var userCompletedLessons = await _userDb.CompletedLessons.Where(p => p.UserId == user.Id).ToListAsync();
            ViewBag.iSFirstLesson = userCompletedLessons.Any();
            ViewBag.progressInPercentage = await CalculateProgress();

            // If the user hasn't completed any lesson the days streak will be 0
            if (!userCompletedLessons.Any())
            {
                ViewBag.streak = 0;
                return View(user);
            }
            
            // Getting essential info for calculating a day streak
            var userCompletedLessonsDescendingByDate = userCompletedLessons.OrderByDescending(lc => lc.UpdatedAt).ToList();
            var lastCompletedLessonDate = userCompletedLessonsDescendingByDate[0].UpdatedAt.Date;

            // Check if streak is still valid
            if (lastCompletedLessonDate < DateTime.Now.Date.AddDays(-1))
            {
                ViewBag.streak = 0;
                return View(user);
            }

            // Calculating day streak
            int dayStreak = 1;
            for (int i = 1; i < userCompletedLessonsDescendingByDate.Count; i++)
            {
                var currentLessonDate = userCompletedLessonsDescendingByDate[i - 1].UpdatedAt.Date;
                var previousLessonDate = userCompletedLessonsDescendingByDate[i].UpdatedAt.Date;

                if (currentLessonDate == previousLessonDate)
                {
                    // Same day, continue
                    continue;
                }
                else if (currentLessonDate.AddDays(-1) == previousLessonDate)
                {
                    // Consecutive day
                    dayStreak++;
                }
                else
                {
                    // Break in the streak
                    break;
                }
            }
            ViewBag.streak = dayStreak;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete (bool isConfirmed)
        {
            // Checking if the user confirmed the deletion
            if (isConfirmed)
            {
                // Getting the current user
                var user = await _userManager.GetUserAsync(User);

                // Signing out the current user
                await _signInManager.SignOutAsync();

                // Deleting the user's avatar and setting the path to user's avatar to null
                var avatarPath = Path.Combine(_hostEnvironment.WebRootPath, "images/imagesForAvatars", user.ImagePath);
                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }
                user.ImagePath = "";

                // Deleting the current user
                _userDb.Remove(user);
                await _userDb.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Settings()
        {
            // Getting current user
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        //
        //
        // -- Add valdiation to the form also for image
        //
        //
        ///
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Settings(Account.Models.User editeduser,
                                                  IFormFile avatar)
        {
            // Getting current user
            var existingUser = await _userManager.GetUserAsync(User);

            // Getting user's avatar path
            if (existingUser.ImagePath != null && Request.Form["deleteImageHidden"] == "true")
            {
                var avatarPath = Path.Combine(_hostEnvironment.WebRootPath, "images/imagesForAvatars", existingUser.ImagePath);
                // Deleting user's avatar from the system
                // if he pressed the delete avatar button
                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }
                existingUser.ImagePath = null;
                await _userManager.UpdateAsync(existingUser);
            }

            if (avatar != null)
            {
                // Deleting if an avatar exist
                if (existingUser.ImagePath != null)
                {
                    var avatarPath = Path.Combine(_hostEnvironment.WebRootPath, "images/imagesForAvatars", existingUser.ImagePath);
                    if (System.IO.File.Exists(avatarPath))
                    {
                        System.IO.File.Delete(avatarPath);
                    }
                    existingUser.ImagePath = null;
                    await _userManager.UpdateAsync(existingUser);
                }

                // Saving user's avatar
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string extension = Path.GetExtension(avatar.FileName);
                existingUser.ImagePath = existingUser.Id + extension;
                string path = Path.Combine(wwwRootPath + "/images/imagesForAvatars", existingUser.ImagePath);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }
            }

            // Updating user's info
            existingUser.UserName = editeduser.UserName;
            existingUser.FirstName = editeduser.FirstName;
            existingUser.Surname = editeduser.Surname;
            existingUser.Email = editeduser.Email;
            existingUser.PhoneNumber = editeduser.PhoneNumber;
            
            // Saving edits
            await _userManager.UpdateAsync(existingUser);
            return RedirectToAction("Settings");
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            // Getting the current user
            var user = await _userManager.GetUserAsync(User);

            // Updating user's password if the current password is correct
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            // If the password was updated the user will be redirected to his profile page
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            // Returning that the passwords don't match
            ViewBag.noMatch = true;
            return View();
        }
    }
}
