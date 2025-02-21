using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;
using SalesDatePrediction.Core.Services;

namespace SalesDatePrediction.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _service = new ProductService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenProductsExist_ReturnsProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
        {
            new() { ProductId = 1, ProductName = "Product 1" },
            new() { ProductId = 2, ProductName = "Product 2" }
        };

            _mockRepository.Setup(x => x.GetAllAsync())
                          .ReturnsAsync(expectedProducts);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoProducts_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetAllAsync())
                          .ReturnsAsync(new List<Product>());

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
