using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesDatePrediction.API.Controllers;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;
using Microsoft.AspNetCore.Http;
using SalesDatePrediction.Tests.Helpers;

namespace SalesDatePrediction.Tests.Controllers;

public class OrdersControllerTests
{
    private readonly Mock<IOrderService> _mockService;
    private readonly Mock<ILogger<OrdersController>> _mockLogger;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _mockService = new Mock<IOrderService>();
        _mockLogger = new Mock<ILogger<OrdersController>>();
        _controller = new OrdersController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateOrder_WithValidOrder_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var newOrder = TestDataGenerator.CreateValidOrder();
        var expectedOrderId = 1;

        _mockService.Setup(x => x.CreateOrderAsync(It.IsAny<NewOrder>()))
                   .ReturnsAsync(expectedOrderId);

        // Act
        var actionResult = await _controller.CreateOrder(newOrder);

        // Assert
        var result = actionResult.Result as CreatedAtActionResult;
        result.Should().NotBeNull();
        result!.Value.Should().Be(expectedOrderId);
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task CreateOrder_WhenServiceThrowsArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var newOrder = TestDataGenerator.CreateValidOrder();
        var errorMessage = "Invalid order";

        _mockService.Setup(x => x.CreateOrderAsync(It.IsAny<NewOrder>()))
                   .ThrowsAsync(new ArgumentException(errorMessage));

        // Act
        var actionResult = await _controller.CreateOrder(newOrder);

        // Assert
        var result = actionResult.Result as BadRequestObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var error = result.Value as ErrorResponse;
        error.Should().NotBeNull();
        error!.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task CreateOrder_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var newOrder = TestDataGenerator.CreateValidOrder();

        _mockService.Setup(x => x.CreateOrderAsync(It.IsAny<NewOrder>()))
                   .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var actionResult = await _controller.CreateOrder(newOrder);

        // Assert
        var result = actionResult.Result as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var error = result.Value as ErrorResponse;
        error.Should().NotBeNull();
        error!.Message.Should().Be("An error occurred while processing your request");
    }

    [Fact]
    public async Task CreateOrder_WithNullOrder_ReturnsBadRequest()
    {
        // Arrange
        NewOrder order = null;

        // Act
        var actionResult = await _controller.CreateOrder(order);

        // Assert
        var result = actionResult.Result as BadRequestObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        var error = result.Value as ErrorResponse;
        error.Should().NotBeNull();
        error!.Message.Should().Be("Order cannot be null");
    }
}
