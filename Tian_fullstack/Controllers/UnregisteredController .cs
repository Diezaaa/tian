using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tian_fullstack.Models;

namespace Tian_fullstack.Controllers
{
    public class UnregisteredController : Controller
    {
        private readonly ILogger<UnregisteredController> _logger;

        public UnregisteredController(ILogger<UnregisteredController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Registeration()
        { 
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
    }
}
