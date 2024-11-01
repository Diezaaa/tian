using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Web;
using Tian_fullstack.Data;
using Tian_fullstack.Models;


namespace Tian_fullstack.Areas.Learning.Controllers
{
    [Area("Learning")]
    public class LearnController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LearnController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var lessonsList = _db.Lessons.ToList().OrderBy(lesson => lesson.Order).ToList();
            return View(lessonsList);
        }
    }
}
