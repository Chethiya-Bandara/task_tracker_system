
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Models;

using TaskTracker.Data;

namespace TaskTracker.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET Tasks
        public IActionResult Index()
        {
            return View(_context.Tasks.ToList());
        }

        // GET Tasks Create
        public IActionResult Create()
        {
            return View();
        }

        // POST Tasks Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskItem task)
        {
            if (!ModelState.IsValid)
                return View(task);

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET Tasks Edit
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.Find(id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST Tasks Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TaskItem updated)
        {
            if (!ModelState.IsValid)
                return View(updated);

            var task = _context.Tasks.Find(id);

            if (task == null)
                return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.Priority = updated.Priority;
            task.IsCompleted = updated.IsCompleted;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // POST Tasks Delete 1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Find(id);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
