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
            // Deleting the ImagePath form model state (Use view models for simpler backend validation)
            newUser.ImagePath = "";
            ModelState.Remove("ImagePath");
            var result = await _userManager.CreateAsync(newUser, password);

            // Checking if whether or not th user was created successfully
            if (result.Succeeded)
            {
                // Creating roles 
                var roleNames = new[] { "Admin", "User", "Manager" };

                foreach (var roleName in roleNames)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        var role = new IdentityRole(roleName);
                        await _roleManager.CreateAsync(role);
                    }
                }

                // Username "Dieza" is the head admin
                var createdUser = await _userManager.FindByNameAsync(newUser.UserName);
                if (createdUser.UserName == "Dieza")
                {
                    await _userManager.AddToRoleAsync(createdUser, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(createdUser, "User");
                }

                await _signInManager.SignInAsync(newUser, isPersistent: false);

                return RedirectToAction("Index", "Learn", new { area = "Learning" });
            }

            AddErrors(result);
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
