using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using server.Data;
using server.Models.Auth;
using server.Models.Auth.DTOs;
using server.Services;
using System.Threading.Tasks;
using Xunit;

namespace server.Tests
{
    public class AuthServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        private readonly IConfiguration _configuration;

        public AuthServiceTests() {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAuthDb")
                .Options;

            // Create in-mem db for configuration
            var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "YourSuperSecretKeyForTesting123!"},
            {"Jwt:Issuer", "TestIssuer"},
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser() {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var authService = new AuthService(context, _configuration);
            var registerDto = new RegisterRequestDto { Email = "test@example.com", Name = "Uzivatel Skvely", Password = "password123" };

            // Act
            var result = await authService.RegisterAsync(registerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(userInDb);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse() {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var authService = new AuthService(context, _configuration);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("password123");
            var user = new User { Email = "login@example.com", Name = "Uzivatel Skvely", PasswordHash = passwordHash };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var loginDto = new LoginRequestDto { Email = "login@example.com", Password = "password123" };

            // Act
            var result = await authService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_ShouldThrowException() {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var authService = new AuthService(context, _configuration);
            var loginDto = new LoginRequestDto { Email = "neexistuju@example.com", Password = "wrongpassword" };

            // AA: Act + Assert
            await Assert.ThrowsAsync<Exception>(() => authService.LoginAsync(loginDto));
        }
    }
}
