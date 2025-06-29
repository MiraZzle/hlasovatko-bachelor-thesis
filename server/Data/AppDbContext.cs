using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models;
using server.Models.Activities;
using server.Models.Answers;
using server.Models.Auth;
using server.Models.Auth.server.Models.Auth;
using server.Models.Templates;

namespace server.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<BankActivity> BankActivities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateSettings> TemplateSettings { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Let EF Core handle most relationships by convention,
            // we will only specify the delete behavior.

            // --- Define Cascade Deletes ---

            // When a Template is deleted, its child Sessions are also deleted.
            modelBuilder.Entity<Template>()
                .HasMany<Session>() // A Template has many Sessions
                .WithOne(s => s.Template) // A Session has one Template
                .HasForeignKey(s => s.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            // When a Session is deleted, its child Activities are also deleted.
            modelBuilder.Entity<Session>()
                .HasMany(s => s.Activities)
                .WithOne() // Activity has no navigation property back to Session
                .HasForeignKey("SessionId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // When an Activity is deleted, its child Answers are also deleted.
            modelBuilder.Entity<Activity>()
                .HasMany<Answer>() // Activity has no navigation property to Answer
                .WithOne(a => a.Activity) // Answer has a navigation property to Activity
                .HasForeignKey(a => a.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            // When a Template is deleted, its Definition (Activities) are also deleted.
            modelBuilder.Entity<Template>()
                .HasMany(t => t.Definition)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // --- Unique Indexes ---
            modelBuilder.Entity<Session>()
                .HasIndex(s => s.JoinCode)
                .IsUnique();

            modelBuilder.Entity<ApiKey>()
                .HasIndex(a => a.UserId)
                .IsUnique();
        }
    }
}