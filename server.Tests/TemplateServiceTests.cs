using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using server.Data;
using server.Models.Enums;
using server.Models.Templates.DTOs;
using server.Services;
using System.Text.Json;
using Xunit;

namespace server.Tests
{
    public class TemplateServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        private readonly Mock<IActivityService> _activityServiceMock;

        public TemplateServiceTests() {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestTemplateDb")
                .Options;
            _activityServiceMock = new Mock<IActivityService>();
        }

        [Fact]
        public async Task CreateTemplateAsync_ShouldCreateTemplate() {
            // Arrange
            await using var context = new AppDbContext(_dbOptions);
            var templateService = new TemplateService(context, _activityServiceMock.Object);
            var ownerId = Guid.NewGuid();
            var createTemplateDto = new CreateTemplateRequestDto {
                Title = "Test temp",
                SessionPacing = SessionMode.TeacherPaced,
                Activities = new List<Models.Activities.DTOs.ActivityRequestDto>
                {
                    new Models.Activities.DTOs.ActivityRequestDto
                    {
                        Title = "Activity 1",
                        ActivityType = "poll",
                        Definition =
                            JsonDocument.Parse("{\"options\":[{\"id\":\"1\",\"text\":\"Option 1\"}]}").RootElement
                    }
                }
            };

            // Act
            var result = await templateService.CreateTemplateAsync(createTemplateDto, ownerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test temp", result.Title);
            var templateInDb = await context.Templates.Include(t => t.Definition)
                .FirstOrDefaultAsync(t => t.Settings.Title == "Test temp");
            Assert.NotNull(templateInDb);
            Assert.Single(templateInDb.Definition);
        }

        [Fact]
        public async Task UpdateTemplateSettingsAsync_ShouldUpdateSettings() {
            // Arrange
            await using var context = new AppDbContext(_dbOptions);
            var ownerId = Guid.NewGuid();

            var initialTemplate = new server.Models.Template {
                OwnerId = ownerId,
                Settings = new server.Models.Templates.TemplateSettings { Title = "OG title" }
            };

            context.Templates.Add(initialTemplate);
            await context.SaveChangesAsync();

            var templateService = new TemplateService(context, _activityServiceMock.Object);
            var updateDto = new UpdateTemplateSettingsDto {
                Title = "New title",
                SessionPacing = SessionMode.StudentPaced
            };

            // Act
            var result = await templateService.UpdateTemplateSettingsAsync(initialTemplate.Id, updateDto, ownerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New title", result.Title);
            Assert.Equal(SessionMode.StudentPaced, result.SessionPacing);
        }
    }
}