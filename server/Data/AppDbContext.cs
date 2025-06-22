using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models.Activities;

namespace server.Data
{
    public partial class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
