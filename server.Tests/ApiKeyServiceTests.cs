using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models.Auth;
using server.Services;
using Xunit;

namespace server.Tests
{
    public class ApiKeyServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public ApiKeyServiceTests() {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestApiKeyDb")
                .Options;
        }

        [Fact]
        public async Task RegenerateKeyAsync_ShouldCreateNewApiKey() {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var apiKeyService = new ApiKeyService(context);
            var user = new User { Id = Guid.NewGuid(), Email = "apikey@example.com", Name = "API User", PasswordHash = "somehash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var newApiKey = await apiKeyService.RegenerateKeyAsync(user.Id);

            // Assert
            Assert.NotNull(newApiKey);
            Assert.NotEmpty(newApiKey.RawApiKey);
            Assert.NotNull(newApiKey.KeyInfo);
        }

        [Fact]
        public async Task GetUserFromApiKeyAsync_WithValidKey_ShouldReturnUser() {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var apiKeyService = new ApiKeyService(context);
            var user = new User { Id = Guid.NewGuid(), Email = "getuser@example.com", Name = "GetUser", PasswordHash = "anotherhash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var newApiKey = await apiKeyService.RegenerateKeyAsync(user.Id);

            // Act
            var resultUser = await apiKeyService.GetUserFromApiKeyAsync(newApiKey.RawApiKey);

            // Assert
            Assert.NotNull(resultUser);
            Assert.Equal(user.Id, resultUser.Id);
        }
    }
}
