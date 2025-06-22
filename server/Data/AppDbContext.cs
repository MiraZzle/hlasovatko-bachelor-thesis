using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models.Activities;
using server.Models.Auth.server.Models.Auth;
using server.Models.Auth;

namespace server.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-one relationship between User and ApiKey
            modelBuilder.Entity<User>()
                .HasOne(u => u.ApiKey)
                .WithOne(a => a.User)
                .HasForeignKey<ApiKey>(a => a.UserId);

            // Enforce that each user can only have one key by making UserId unique
            modelBuilder.Entity<ApiKey>()
                .HasIndex(a => a.UserId)
                .IsUnique();
        }
    }
}
