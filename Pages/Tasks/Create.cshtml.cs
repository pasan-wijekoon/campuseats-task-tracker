using CampusEatsTaskTracker.Data;
using CampusEatsTaskTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CampusEatsTaskTracker.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskItem Task { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async System.Threading.Tasks.Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Task.CreatedAt = DateTime.UtcNow;
            Task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(Task);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Task \"{Task.Title}\" created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
