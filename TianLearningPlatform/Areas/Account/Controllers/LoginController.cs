using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Tian_fullstack.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        private readonly SignInManager<Models.User> _signInManager;
        public LoginController(SignInManager<Models.User> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            // The condition redirects a logged in user to learn page
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Learn", new { area = "Learning" });
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            // Tries to sign in the user
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Learn", new { area = "Learning" });
            }
            return View(result);
        }
    }
}
