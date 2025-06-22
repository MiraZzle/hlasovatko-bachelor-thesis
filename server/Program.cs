
using Microsoft.EntityFrameworkCore;
using server.Data;

namespace server
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Get connection string from env or fallback
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                ?? builder.Configuration.GetConnectionString("Default");

            // Register DbContext with PostgreSQL
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Setup CORS to allow frontend clients
            var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:3000";
            var managerUrl = Environment.GetEnvironmentVariable("MANAGER_URL") ?? "http://localhost:3001";

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(clientUrl, managerUrl)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Middleware pipeline
            app.UseCors();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
