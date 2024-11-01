using Microsoft.AspNetCore.Mvc;

namespace Tian_fullstack.Areas.Account.Controllers
{
    [Area("Account")]
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
