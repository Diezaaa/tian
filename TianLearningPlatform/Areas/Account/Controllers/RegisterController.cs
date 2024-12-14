using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tian_fullstack.Areas.Account.Models;
using Tian_fullstack.Data;

namespace Tian_fullstack.Areas.Account.Controllers
{
    [Area("Account")]
    public class RegisterController : Controller
    {
        private readonly UserManager<Models.User> _userManager;
        private readonly SignInManager<Models.User> _signInManager;
        private readonly UserDbContext _userDb;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterController(UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, UserDbContext userDb, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userDb = userDb;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Learn", new { area = "Learning" });
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index (Models.User newUser, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new Models.User
                {
                    Email = newUser.Email,
                    UserName = newUser.UserName,
                    FirstName = newUser.FirstName,
                    Surname = newUser.Surname,
                    PhoneNumber = newUser.PhoneNumber,
                    BirthDay = newUser.BirthDay
                };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Learn", new { area = "Learning" });
                }
                AddErrors(result);
            }
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
