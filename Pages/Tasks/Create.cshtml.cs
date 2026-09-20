using CampusEatsTaskTracker.Data;
using CampusEatsTaskTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CampusEatsTaskTracker.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        private const string SuccessMessageKey = "SuccessMessage";

        public CreateModel(AppDbContext context, ILogger<CreateModel> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [BindProperty]
        public TaskCreateViewModel TaskInput { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var task = new TaskItem
                {
                    Title = TaskInput.Title,
                    Description = TaskInput.Description,
                    Priority = TaskInput.Priority,
                    DueDate = TaskInput.DueDate,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Task {TaskId} with title {Title} created successfully.", task.Id, task.Title);

                TempData[SuccessMessageKey] = $"Task \"{task.Title}\" created successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a task with title {Title}.", TaskInput.Title);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving the task. Please try again.");
                return Page();
            }
        }
    }
}
