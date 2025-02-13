using Microsoft.EntityFrameworkCore;
using server.Entities;

namespace server.Data
{
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Session> Sessions { get; set; }
    }
}
