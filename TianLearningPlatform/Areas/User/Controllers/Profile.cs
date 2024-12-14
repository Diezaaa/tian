using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;
using System.IO;
using Tian_fullstack.Areas.Account.Models;
using Tian_fullstack.Data;
using static System.Net.Mime.MediaTypeNames;

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
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentUserId = user.Id.ToString();
            var lessonsList = _db.Lessons.ToList();
            var userCompletedLessons = await _userDb.CompletedLessons.Where(p => p.UserId == currentUserId).ToListAsync();
            var lastLessonNumber = userCompletedLessons.Any() ? (userCompletedLessons.OrderByDescending(p => p.LessonNumber).ToList()[0]).LessonNumber : 0;
            ViewBag.iSFirstLesson = lastLessonNumber >= 1;
            ViewBag.percents = (int)(100 * ((double)lastLessonNumber / lessonsList.Count));

            // Calculating days streak
            if (lastLessonNumber == 0)
            {
                ViewBag.streak = 0;
                return View(user);
            }
            int streak = 0;
            var userCompletedLessonsSorted = userCompletedLessons.OrderByDescending(p => p.UpdatedAt).ToList();
            var today = DateTime.Now;
            var oneDay = new TimeSpan(days: 1, 0, 0, 0);
            var lastLesson = userCompletedLessonsSorted[0];
            // FIX ALL THE CONTOLLER
            if (today.Date < lastLesson.UpdatedAt.Date)
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
        // Add valdiation to the form
        //
        //
        ///
        [HttpPost]
        public async Task<IActionResult> Settings(Account.Models.User editeduser,
                                                  IFormFile avatar)
        {
            // Getting current user
            var existingUser = await _userManager.GetUserAsync(User);

            if (avatar!= null)
            {
                // Deleting if an avatar exist
                if (existingUser.ImagePath is not null)
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

            return View();
        }
    }
}
