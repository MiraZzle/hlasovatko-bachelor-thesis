
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
                options.AddPolicy("AllowManagerFE",
                    policy => {
                        policy.WithOrigins("http://localhost:5173") // allow teacher
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

            app.UseCors("AllowManagerFE");

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
