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

            modelBuilder.Entity<Template>()
                .HasMany<Session>() 
                .WithOne(s => s.Template)
                .HasForeignKey(s => s.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
                .HasMany(s => s.Activities)
                .WithOne() 
                .HasForeignKey("SessionId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Activity>()
                .HasMany<Answer>() 
                .WithOne(a => a.Activity)
                .HasForeignKey(a => a.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Template>()
                .HasMany(t => t.Definition)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
                .HasIndex(s => s.JoinCode)
                .IsUnique();

            modelBuilder.Entity<ApiKey>()
                .HasIndex(a => a.UserId)
                .IsUnique();
        }
    }
}