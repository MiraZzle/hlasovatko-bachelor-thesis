using Microsoft.EntityFrameworkCore;
using server.Models.Auth.DTOs;
using server.Services;
using Newtonsoft.Json.Linq;
using server.Models.Activities.DTOs;
using System.Collections.Generic;
using System.Text.Json;

namespace server.Data
{

    public static class DataSeeder
    {
        /// <summary>
        /// Seeding service for demo data.
        /// </summary>
        /// <param name="app">The host app.</param>
        public static async Task SeedAdminUserAsync(IHost app) {
            // Need to create a scope to resolve services
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try {
                var authService = serviceProvider.GetRequiredService<IAuthService>();
                var context = serviceProvider.GetRequiredService<AppDbContext>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                // Get the credentials from env variables
                var email = configuration["A_USER_EMAIL"];
                var password = configuration["A_USER_PASSWORD"];
                var name = configuration["A_USER_NAME"];

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name)) {
                    logger.LogWarning("Admin user seeding skipped: Not all required variables are set.");
                    return;
                }

                // Check for existence
                if (!await context.Users.AnyAsync(u => u.Email == email)) {
                    var registerDto = new RegisterRequestDto {
                        Email = email,
                        Password = password,
                        Name = name
                    };

                    var authResponse = await authService.RegisterAsync(registerDto);
                    var adminId = await context.Users.FindAsync(authResponse.Id);

                    if (adminId != null) { await SeedAdminActivityBankAsync(serviceProvider, adminId.Id, logger); }

                    logger.LogInformation("Admin user seeded successfully.");
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, "Erro during admin seeding!");
            }
        }

        private static async Task SeedAdminActivityBankAsync(IServiceProvider serviceProvider, Guid adminId, ILogger logger) {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var activityService = serviceProvider.GetRequiredService<IActivityService>();
            var activitiesToSeed = GetInitialActivities();

            foreach (var activityDto in activitiesToSeed) {
                await activityService.AddToBankAsync(activityDto, adminId);
            }
        }

        /// <summary>
        /// Returns a list of sample activities to be added to the activity bank.
        /// </summary>
        /// <returns>A list of ActivityRequestDtos.</returns>
        private static List<ActivityRequestDto> GetInitialActivities() {

            // Helper method for creating jsonelem from an object
            JsonElement ToJsonElement(object obj) {
                var jsonString = JObject.FromObject(obj).ToString();
                return JsonDocument.Parse(jsonString).RootElement;
            }

            return new List<ActivityRequestDto>
            {
                new ActivityRequestDto
                {
                    Title = "Which of these is most likely to be used to write a Matfyz bachelor thesis?",
                    ActivityType = "multiple_choice",
                    Tags = new List<string> { "matfyz", "suffering", "sample" },
                    Definition = ToJsonElement(new
                    {
                        options = new[]
                        {
                            new { id = "1", text = "LaTeX" },
                            new { id = "2", text = "JavaScript" },
                            new { id = "3", text = "Excel macros" },
                            new { id = "4", text = "Blood, sweat, and tears" }
                        },
                        correctAnswer = new[] { "1" },
                        allowMultiple = false
                    })
                },

                new ActivityRequestDto
                {
                    Title = "Which of these is a Turing-complete language?",
                    ActivityType = "multiple_choice",
                    Tags = new List<string> { "programming", "cs-theory", "sample" },
                    Definition = ToJsonElement(new
                    {
                        options = new[]
                        {
                            new { id = "1", text = "HTML" },
                            new { id = "2", text = "Python" },
                            new { id = "3", text = "CSS" },
                            new { id = "4", text = "Markdown" }
                        },
                        correctAnswer = new[] { "2" },
                        allowMultiple = false
                    })
                },
                new ActivityRequestDto
                {
                    Title = "What is your favorite course at Matfyz and why?",
                    ActivityType = "open_ended",
                    Tags = new List<string> { "matfyz", "opinion", "sample" },
                    Definition = ToJsonElement(new
                    {
                        placeholder = "Share your thoughts here...",
                    })
                },
                new ActivityRequestDto
                {
                    Title = "What is your preferred way to study for exams?",
                    ActivityType = "poll",
                    Tags = new List<string> { "study", "habits", "sample" },
                    Definition = ToJsonElement(new
                    {
                        options = new[]
                        {
                            new { id = "1", text = "Reading slides and lecture notes" },
                            new { id = "2", text = "Solving past exams" },
                            new { id = "3", text = "Group study sessions" }
                        },
                        allowMultiple = false
                    })
                },
                new ActivityRequestDto
                {
                    Title = "On a scale of 1 to 10, how confident are you in writing SQL queries?",
                    ActivityType = "scale_rating",
                    Tags = new List<string> { "db", "confidence", "sample" },
                    Definition = ToJsonElement(new
                    {
                        min = 1,
                        max = 10,
                        minLabel = "Not Confident At All",
                        maxLabel = "SQL Wizard"
                    })
                }
            };
        }
    }
}
