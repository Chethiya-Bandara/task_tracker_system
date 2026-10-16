
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        [Required]
        public string Priority { get; set; } = "Medium";

        public bool IsCompleted { get; set; } = false;
    }
}
