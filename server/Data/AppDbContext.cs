using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models.Activities;
using server.Utils;

namespace server.Data
{
    // note for self: udelat tridu partial, aby slo dodefinovat externe
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<QuizActivity> QuizActivities { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<QuizActivity>().OwnsMany(q => q.Questions,
                aq => {
                    // Optionally configure the foreign key to QuizActivity
                    aq.WithOwner().HasForeignKey("QuizActivityId");
                    aq.Property<Guid>("QuestionId");
                    aq.HasKey("QuestionId");
                });

            modelBuilder.Entity<Session>()
                .HasMany(s => s.Activities)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
