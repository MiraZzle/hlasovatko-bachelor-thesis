
using Microsoft.EntityFrameworkCore;
using server.Data;

namespace server
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontendClients",
                    policy => {
                        policy.WithOrigins("http://localhost:3000", "http://localhost:3001") // allow frontends
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });


            builder.Services.AddControllers();
            // POC -> user in mem db
            builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Hlasovatko"));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowFrontendClients");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
