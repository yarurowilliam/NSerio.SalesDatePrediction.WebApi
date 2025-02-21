using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SalesDatePrediction.API.Controllers;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_WhenProductsExist_ReturnsOkWithProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
        {
            new() { ProductId = 1, ProductName = "Chai" },
            new() { ProductId = 2, ProductName = "Chang" }
        };

            _mockService.Setup(x => x.GetAllAsync())
                       .ReturnsAsync(expectedProducts);

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task GetAll_WhenNoProducts_ReturnsOkWithEmptyList()
        {
            // Arrange
            _mockService.Setup(x => x.GetAllAsync())
                       .ReturnsAsync(new List<Product>());

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var products = result!.Value as IEnumerable<Product>;
            products.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_WhenServiceThrows_ReturnsInternalServerError()
        {
            // Arrange
            _mockService.Setup(x => x.GetAllAsync())
                       .ThrowsAsync(new Exception("Database error"));

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var result = actionResult.Result as ObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var errorResponse = result.Value as ErrorResponse;
            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Be("An error occurred while processing your request");

            // Verify logging
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
