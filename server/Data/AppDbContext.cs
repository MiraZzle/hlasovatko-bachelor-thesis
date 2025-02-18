using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models.Activities;

namespace server.Data
{
    // note for self: udelat tridu partial, aby slo dodefinovat externe
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<QuizActivity> QuizActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Session>()
                .HasMany(s => s.Activities)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
