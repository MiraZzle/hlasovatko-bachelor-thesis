using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using server.Data;
using server.Models.Activities.DTOs;
using server.Services;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace server.Tests
{
    public class ActivityServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        private readonly Mock<ILogger<ActivityService>> _loggerMock;

        public ActivityServiceTests() {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestActivityDb")
                .Options;
            _loggerMock = new Mock<ILogger<ActivityService>>();
        }

        [Fact]
        public async Task AddToBankAsync_ShouldAddActivity() {
            // Arrange
            await using var context = new AppDbContext(_dbOptions);
            var activityService = new ActivityService(context, _loggerMock.Object);
            var ownerId = Guid.NewGuid();
            var activityDto = new ActivityRequestDto {
                Title = "Test activity",
                ActivityType = "poll",
                Definition = JsonDocument.Parse("{\"options\":[{\"id\":\"1\",\"text\":\"Option 1\"}]}")
                    .RootElement,
                Tags = new List<string> { "test" }
            };

            // Act
            var result = await activityService.AddToBankAsync(activityDto, ownerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test activity", result.Title);
            var activityInDb = await context.BankActivities.FirstOrDefaultAsync(a => a.Title == "Test activity");
            Assert.NotNull(activityInDb);
        }

        [Fact]
        public async Task ValidateActivityDefinitionAsync_WithInvalidJson_ShouldThrowJsonException() {
            // Arrange
            await using var context = new AppDbContext(_dbOptions);
            var activityService = new ActivityService(context, _loggerMock.Object);
            var invalidJson = "{\"options\":[{id:1}]}";

            // Act and Assert
            await Assert.ThrowsAsync<JsonException>(() =>
                activityService.ValidateActivityDefinitionAsync("poll", invalidJson));
        }
    }
}