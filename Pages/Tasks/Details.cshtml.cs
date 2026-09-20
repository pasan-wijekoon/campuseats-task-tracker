using CampusEatsTaskTracker.Data;
using CampusEatsTaskTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CampusEatsTaskTracker.Pages.Tasks
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
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
    }
}
