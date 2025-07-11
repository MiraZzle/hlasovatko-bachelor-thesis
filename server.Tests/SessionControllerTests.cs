using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using server.Controllers;
using server.Data;
using server.Models.Enums;
using server.Models.Sessions.DTOs;
using server.Services;
using System;
using Xunit;

namespace server.Tests
{
    public class SessionControllerTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public SessionControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSessionDb")
                .Options;
        }

        [Fact]
        public async Task GetSessionByJoinCode_WithValidCode_ReturnsOk()
        {
            // Arrange
            var mockService = new Mock<ISessionService>();
            var expectedSession = new SessionResponseDto { Id = Guid.NewGuid(), Title = "Test Session",
                JoinCode = "ABCDEF", Mode = SessionMode.StudentPaced };

            await using var context = new AppDbContext(_dbOptions);
            mockService.Setup(service => service.GetSessionByJoinCodeAsync("ABCDEF"))
                .ReturnsAsync(expectedSession);

            var controller = new SessionController(mockService.Object, context);

            // Act
            var result = await controller.GetSessionByJoinCode("ABCDEF");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value;
            Assert.NotNull(returnValue);
            Assert.Equal(expectedSession.Id, returnValue.GetType().GetProperty("id")?.GetValue(returnValue, null));
            Assert.Equal(expectedSession.Title, returnValue.GetType().GetProperty("title")?.GetValue(returnValue, null));
            Assert.Equal(expectedSession.Mode, returnValue.GetType().GetProperty("mode")?.GetValue(returnValue, null));
        }

        [Fact]
        public async Task GetSessionByJoinCode_WithInvalidCode_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<ISessionService>();
            await using var context = new AppDbContext(_dbOptions);

            mockService.Setup(service => service.GetSessionByJoinCodeAsync("INVALID"))
                .ReturnsAsync((SessionResponseDto)null!);
            var controller = new SessionController(mockService.Object, context);

            // Act
            var result = await controller.GetSessionByJoinCode("INVALID");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}