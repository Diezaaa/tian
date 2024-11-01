using Microsoft.AspNetCore.Mvc;

namespace Tian_fullstack.Areas.User.Controllers
{
    [Area("User")]

    public class Profile : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}
