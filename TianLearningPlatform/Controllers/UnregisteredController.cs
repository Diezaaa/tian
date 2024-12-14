using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tian_fullstack.Models;

namespace Tian_fullstack.Controllers
{
    public class UnregisteredController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Learn", new { area="Learning"});
            }
            return View();
        }

        public IActionResult About()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Learn", new { area = "Learning" });
            }
            return View();
        }
    }
}
