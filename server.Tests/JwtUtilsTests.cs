using server.Models.Auth;
using server.Utils;
using System.IdentityModel.Tokens.Jwt;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace server.Tests
{

    public class JwtUtilsTests
    {
        private readonly IConfiguration _configuration;

        public JwtUtilsTests() {
            var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "ThisIsAValidKeyForTesting123456!"},
            {"Jwt:Issuer", "TestIssuer"},
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public void GenerateJwtToken_ShouldCreateValidToken() {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "jwt@test.com", Name = "JwtUser" };

            // Act
            var tokenString = JwtUtils.GenerateJwtToken(user, _configuration);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            // Assert
            Assert.NotNull(token);
            Assert.Equal("TestIssuer", token.Issuer);
            Assert.Equal(user.Id.ToString(), token.Claims.First(c => c.Type == "sub").Value);
        }
    }
}
