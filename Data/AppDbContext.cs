using Microsoft.EntityFrameworkCore;
using CampusEatsTaskTracker.Models;

namespace CampusEatsTaskTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Use Fluent API as single source of truth for constraints
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                // Changed to integer storage for performance and maintainability
                entity.Property(e => e.Priority).HasConversion<int>();

                // Use standard SQL for portability
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Index for performance on frequently filtered columns
                entity.HasIndex(e => e.Priority);
                entity.HasIndex(e => e.DueDate);

                // Global query filter for soft delete
                entity.HasQueryFilter(t => !t.IsDeleted);
            });
        }
    }
}
