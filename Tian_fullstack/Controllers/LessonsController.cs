using Microsoft.AspNetCore.Mvc;
using Tian_fullstack.Data;
using Tian_fullstack.Models;

namespace Tian_fullstack.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LessonsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var lessonsList = _db.Lessons.ToList().OrderBy(lesson => lesson.Order).ToList();
            return View(lessonsList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Lesson obj)
        {

            if (ModelState.IsValid)
            {
                _db.Lessons.Add(obj);
                _db.SaveChanges();
            }
            return View();
        }

    }
}
