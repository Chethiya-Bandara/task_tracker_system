
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

        [Display(Name = "Due date")]
        [DataType(DataType.Date)]
        public DateOnly? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
