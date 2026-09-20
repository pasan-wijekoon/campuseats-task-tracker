using CampusEatsTaskTracker.Data;
using CampusEatsTaskTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CampusEatsTaskTracker.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskItem Task { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            Task = task;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Task.UpdatedAt = DateTime.UtcNow;

            _context.Attach(Task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tasks.AnyAsync(e => e.Id == Task.Id))
                    return NotFound();
                throw;
            }

            TempData["SuccessMessage"] = $"Task \"{Task.Title}\" updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
