using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Tian_fullstack.Data;


namespace Tian_fullstack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserDbContext _userDb;
        private readonly UserManager<Tian_fullstack.Areas.Account.Models.User> _userManager;
        public UsersController(ApplicationDbContext db, UserDbContext userDb, UserManager<Tian_fullstack.Areas.Account.Models.User> userManager)
        {
            _db = db;
            _userDb = userDb;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var usersList = await _userDb.Users.ToListAsync();
            return View(usersList);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Detailes()
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
    } 
}