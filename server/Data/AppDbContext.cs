using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models.Activities;
using server.Models.Answers;
using server.Utils;

namespace server.Data
{
    public partial class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<QuizActivity> QuizActivities { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<QuizActivity>().OwnsMany(q => q.Questions,
                aq => {
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
