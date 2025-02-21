using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;
using SalesDatePrediction.Core.Services;

namespace SalesDatePrediction.Tests.Services
{
    public class ShipperServiceTests
    {
        private readonly Mock<IShipperRepository> _mockRepository;
        private readonly Mock<ILogger<ShipperService>> _mockLogger;
        private readonly ShipperService _service;

        public ShipperServiceTests()
        {
            _mockRepository = new Mock<IShipperRepository>();
            _mockLogger = new Mock<ILogger<ShipperService>>();
            _service = new ShipperService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenShippersExist_ReturnsShippers()
        {
            // Arrange
            var expectedShippers = new List<Shipper>
        {
            new() { ShipperId = 1, CompanyName = "Fast Delivery" },
            new() { ShipperId = 2, CompanyName = "Quick Ship" }
        };

            _mockRepository.Setup(x => x.GetAllAsync())
                          .ReturnsAsync(expectedShippers);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedShippers);
            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoShippers_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetAllAsync())
                          .ReturnsAsync(new List<Shipper>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryThrows_LogsAndRethrows()
        {
            // Arrange
            var expectedException = new Exception("Database error");
            _mockRepository.Setup(x => x.GetAllAsync())
                          .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _service.GetAllAsync());

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
