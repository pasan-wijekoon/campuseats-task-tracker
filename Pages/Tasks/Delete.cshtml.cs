using CampusEatsTaskTracker.Data;
using CampusEatsTaskTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CampusEatsTaskTracker.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        public TaskItem Task { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            Task = task;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Task \"{task.Title}\" deleted.";
            }

            return RedirectToPage("./Index");
        }
    }
}
