using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SalesDatePrediction.API.Controllers;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Tests.Controllers;

public class ShippersControllerTests
{
    private readonly Mock<IShipperService> _mockService;
    private readonly Mock<ILogger<ShippersController>> _mockLogger;
    private readonly ShippersController _controller;

    public ShippersControllerTests()
    {
        _mockService = new Mock<IShipperService>();
        _mockLogger = new Mock<ILogger<ShippersController>>();
        _controller = new ShippersController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_WhenShippersExist_ReturnsOkWithShippers()
    {
        // Arrange
        var expectedShippers = new List<Shipper>
    {
        new() { ShipperId = 1, CompanyName = "Speedy Express" },
        new() { ShipperId = 2, CompanyName = "United Package" }
    };

        _mockService.Setup(x => x.GetAllAsync())
                   .ReturnsAsync(expectedShippers);

        // Act
        var actionResult = await _controller.GetAll();

        // Assert
        var result = actionResult.Result as OkObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeEquivalentTo(expectedShippers);
    }

    [Fact]
    public async Task GetAll_WhenNoShippers_ReturnsOkWithEmptyList()
    {
        // Arrange
        _mockService.Setup(x => x.GetAllAsync())
                   .ReturnsAsync(new List<Shipper>());

        // Act
        var actionResult = await _controller.GetAll();

        // Assert
        var result = actionResult.Result as OkObjectResult;
        result.Should().NotBeNull();
        var shippers = result!.Value as IEnumerable<Shipper>;
        shippers.Should().BeEmpty();
    }
}
