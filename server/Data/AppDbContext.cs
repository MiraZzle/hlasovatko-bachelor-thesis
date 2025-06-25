using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models;
using server.Models.Activities;
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
        public DbSet<User> Users { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateSettings> TemplateSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ApiKey)
                .WithOne(a => a.User)
                .HasForeignKey<ApiKey>(a => a.UserId);

            modelBuilder.Entity<ApiKey>()
                .HasIndex(a => a.UserId)
                .IsUnique();

            modelBuilder.Entity<Template>()
                .HasMany(t => t.Definition)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
                .HasIndex(s => s.JoinCode)
                .IsUnique();

            modelBuilder.Entity<Template>()
                .HasOne(t => t.Settings)
                .WithOne(s => s.Template)
                .HasForeignKey<TemplateSettings>(s => s.TemplateId);
        }
    }
}
