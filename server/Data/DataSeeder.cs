using Microsoft.EntityFrameworkCore;
using server.Models.Auth.DTOs;
using server.Services;

namespace server.Data
{

    public static class DataSeeder
    {
        /// <summary>
        /// Seeds an initial admin user from environment variables if they do not already exist.
        /// </summary>
        /// <param name="app">The host application.</param>
        public static async Task SeedAdminUserAsync(IHost app) {
            // need to create a scope to resolve services
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try {
                var authService = serviceProvider.GetRequiredService<IAuthService>();
                var context = serviceProvider.GetRequiredService<AppDbContext>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var email = configuration["A_USER_EMAIL"];
                var password = configuration["A_USER_PASSWORD"];
                var name = configuration["A_USER_NAME"];

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name)) {
                    logger.LogWarning("Admin user seeding skipped: Not all required variables are set.");
                    return;
                }

                // check for existence
                if (!await context.Users.AnyAsync(u => u.Email == email)) {
                    var registerDto = new RegisterRequestDto {
                        Email = email,
                        Password = password,
                        Name = name
                    };

                    await authService.RegisterAsync(registerDto);
                    logger.LogInformation("Admin user seeded successfully.");
                }
                else {
                    logger.LogInformation("Admin user already exists, seeding skipped.");
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, "Erro during admin seeding!");
            }
        }
    }
}
