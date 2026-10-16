
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;

using TaskTracker.Data;

namespace TaskTracker.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET Tasks
        public IActionResult Index(string? priority, DateOnly? dueDate)
        {
            var tasks = _context.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(priority))
                tasks = tasks.Where(task => task.Priority == priority);

            if (dueDate.HasValue)
                tasks = tasks.Where(task => task.DueDate == dueDate);

            ViewData["SelectedPriority"] = priority;
            ViewData["SelectedDueDate"] = dueDate?.ToString("yyyy-MM-dd");

            return View(tasks.OrderBy(task => task.DueDate == null).ThenBy(task => task.DueDate).ToList());
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
            ValidateDueDate(task.DueDate);

            if (!ModelState.IsValid)
                return View(task);

            try
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, "Unable to create a task.");
                ModelState.AddModelError(string.Empty,
                    "Your task could not be saved. Please try again.");
                return View(task);
            }

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
            ValidateDueDate(updated.DueDate);

            if (!ModelState.IsValid)
                return View(updated);

            var task = _context.Tasks.Find(id);

            if (task == null)
                return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.Priority = updated.Priority;
            task.DueDate = updated.DueDate;
            task.IsCompleted = updated.IsCompleted;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, "Unable to update task {TaskId}.", id);
                ModelState.AddModelError(string.Empty,
                    "Your task could not be updated. Please try again.");
                return View(updated);
            }

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

            try
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, "Unable to delete task {TaskId}.", id);
                TempData["ErrorMessage"] = "The task could not be deleted. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void ValidateDueDate(DateOnly? dueDate)
        {
            if (dueDate.HasValue && dueDate.Value < DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError(nameof(TaskItem.DueDate),
                    "The due date cannot be in the past. Please select today or a future date.");
            }
        }
    }
}
