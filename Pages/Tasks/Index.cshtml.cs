using CampusEatsTaskTracker.Data;
using CampusEatsTaskTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CampusEatsTaskTracker.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<TaskItem> Tasks { get; set; } = new();
        public string Search { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;

        public async System.Threading.Tasks.Task OnGetAsync(string? search, string? filter)
        {
            Search = search ?? string.Empty;
            Filter = filter ?? string.Empty;

            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(t =>
                    t.Title.ToLower().Contains(Search.ToLower()) ||
                    (t.Description != null && t.Description.ToLower().Contains(Search.ToLower())));
            }

            if (Filter == "pending")
                query = query.Where(t => !t.IsCompleted);
            else if (Filter == "completed")
                query = query.Where(t => t.IsCompleted);

            Tasks = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }
    }
}
