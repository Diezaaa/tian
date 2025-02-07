using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;
using Tian_fullstack.Areas.Account.Models;
using Tian_fullstack.Data;
using System.Text.Json;

namespace Tian_fullstack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserDbContext _userDb;
        private readonly UserManager<Tian_fullstack.Areas.Account.Models.User> _userManager;
        private IWebHostEnvironment _hostEnvironment;
        public UsersController(ApplicationDbContext db, UserDbContext userDb, UserManager<Tian_fullstack.Areas.Account.Models.User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _userDb = userDb;
            _userManager = userManager;
            _hostEnvironment = webHostEnvironment;

        }
        public async Task<IActionResult> Index()
        {
            var usersList = await _userDb.Users.ToListAsync();
            return View(usersList);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Details()
        {
            // Getting the requested username
            var requstedUsername = HttpContext.Request.Query["user"].ToString();

            // Getting the requested user by username
            var user = await _userDb.Users.FirstOrDefaultAsync(x => x.UserName == requstedUsername);

            // Finding his role
            var role = (await _userManager.GetRolesAsync(user))[0];


            // Passing the info to the view
            ViewBag.user = user;
            ViewBag.role = role;

            return View();
        }
        public async Task<IActionResult> Delete()
        {
            // Getting the requested username
            var requstedUsername = HttpContext.Request.Query["user"].ToString();

            // Getting the requested user by username
            var user = await _userDb.Users.FirstOrDefaultAsync(x => x.UserName == requstedUsername);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(bool isConfirmed)
        {
            // Getting the requested user
            var user = await _userDb.Users.FirstOrDefaultAsync(x => x.UserName == HttpContext.Request.Query["username"].ToString());

            // Checking if the user confirmed the deletion
            if (isConfirmed)
            {
                _userDb.Users.Remove(user);
                _userDb.SaveChanges();
                return RedirectToAction("Index");
            }

            // Passing the user to the view and returning the view
            return View(user);
        }
        public async Task<IActionResult> Edit()
        {
            // Getting the requested username
            var requstedUsername = HttpContext.Request.Query["user"].ToString();
            // Getting the requested user by username
            var user = await _userDb.Users.FirstOrDefaultAsync(x => x.UserName == requstedUsername);
            return View(user);
        }
        [HttpPost] 
        public async Task<IActionResult> Edit(Tian_fullstack.Areas.Account.Models.User editedUser,
                                                IFormFile avatar, string password)
        {
            // Getting the requested username
            var requestedUsername = HttpContext.Request.Query["user"].ToString();

            // Getting the requested user by username
            var existingUser = await _userDb.Users.FirstOrDefaultAsync(x => x.UserName == requestedUsername);

            // Getting user's avatar path
            var avatarPath = Path.Combine(_hostEnvironment.WebRootPath, "images/imagesForAvatars", existingUser.ImagePath);

            // Deleting user's avatar from the system
            // if he pressed the delete avatar button
            if (Request.Form["deleteImageHidden"] == "true")
            {
                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }
                existingUser.ImagePath = "";
                await _userManager.UpdateAsync(existingUser);
            }

            if (avatar != null)
            {
                // Deleting if an avatar exist
                if (existingUser.ImagePath != "")
                {
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
            existingUser.UserName = editedUser.UserName;
            existingUser.FirstName = editedUser.FirstName;
            existingUser.Surname = editedUser.Surname;
            existingUser.Email = editedUser.Email;
            existingUser.PhoneNumber = editedUser.PhoneNumber;

            if (password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                await _userManager.ResetPasswordAsync(existingUser, token, password);
            }
            
            // Saving edits
            await _userManager.UpdateAsync(existingUser);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> SearchByUserName(string username)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            List<Account.Models.User> users;
            if (username != null)
            {
                users = await _userDb.Users
                    .Where(u => u.UserName.Contains(username))
                    .ToListAsync();
            }
            else
            {
                users = await _userDb.Users.ToListAsync();
            }
            
            List<string> avatars = new List<string>();
            foreach (var user in users)
            {
                if (user.ImagePath != null)
                {
                    var avatarPath = Path.Combine(wwwRootPath + "/images/imagesForAvatars", user.ImagePath);

                    byte[] avatarBytes = System.IO.File.ReadAllBytes(avatarPath);
                    string avatar64Image = Convert.ToBase64String(avatarBytes);
                    avatars.Add("data:image/" + Path.GetExtension(avatarPath).Substring(1) + ";base64," + avatar64Image);
                }
                avatars.Add("");
            }


            string json = System.Text.Json.JsonSerializer.Serialize(users);
            List<JsonObject> jsonUsersObjs = System.Text.Json.JsonSerializer.Deserialize<List<JsonObject>>(json);

            int i = 0;
            foreach (var userJson in jsonUsersObjs)
            {
                userJson.Add("Avatar", avatars[i]);
                i++;
            }

            string modifiedJson = System.Text.Json.JsonSerializer.Serialize(jsonUsersObjs);
            return Ok(modifiedJson);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (Tian_fullstack.Areas.Account.Models.User newUser,
                                                IFormFile avatar, string password)
        {
            await _userManager.CreateAsync(newUser, password);

            // Saving the uploaded avatar
            if (avatar != null) 
            {

                string extension = Path.GetExtension(avatar.FileName);
                newUser.ImagePath = newUser.Id + extension;

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string path = Path.Combine(wwwRootPath + "/images/imagesForAvatars", newUser.ImagePath);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }
                await _userManager.UpdateAsync(newUser);
            }
            return RedirectToAction("Index");
        }
    } 
}