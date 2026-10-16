
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Models;

namespace TaskTracker.Controllers
{
    public class TasksController : Controller
    {
        private static List<TaskItem> tasks = new();
        private static int nextId = 1;

        // GET Tasks
        public IActionResult Index()
        {
            return View(tasks);
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

            task.Id = nextId++;
            tasks.Add(task);

            return RedirectToAction(nameof(Index));
        }

        // GET Tasks Edit
        public IActionResult Edit(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

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

            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.Priority = updated.Priority;
            task.IsCompleted = updated.IsCompleted;

            return RedirectToAction(nameof(Index));
        }

        // POST Tasks Delete 1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            tasks.Remove(task);

            return RedirectToAction(nameof(Index));
        }
    }
}
