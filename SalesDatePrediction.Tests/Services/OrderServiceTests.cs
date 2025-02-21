using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Services;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepository;
    private readonly Mock<ILogger<OrderService>> _mockLogger;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _mockLogger = new Mock<ILogger<OrderService>>();
        _orderService = new OrderService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_WithValidOrder_ReturnsOrderId()
    {
        // Arrange
        var newOrder = new NewOrder
        {
            CustomerId = 1,
            EmpId = 1,
            ShipperId = 1,
            ShipName = "Test Customer",
            ShipAddress = "Test Address",
            ShipCity = "Test City",
            ShipCountry = "Test Country",
            OrderDate = DateTime.UtcNow,
            RequiredDate = DateTime.UtcNow.AddDays(7),
            Freight = 10.00m,
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    ProductId = 1,
                    UnitPrice = 10.00m,
                    Qty = 1,
                    Discount = 0
                }
            }
        };

        var expectedOrderId = 1;
        _mockRepository.Setup(x => x.CreateOrderAsync(newOrder))
                      .ReturnsAsync(expectedOrderId);

        // Act
        var result = await _orderService.CreateOrderAsync(newOrder);

        // Assert
        result.Should().Be(expectedOrderId);
        _mockRepository.Verify(x => x.CreateOrderAsync(newOrder), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_WithInvalidCustomerId_ThrowsArgumentException()
    {
        // Arrange
        var newOrder = new NewOrder
        {
            CustomerId = 0, // Invalid CustomerId
            EmpId = 1,
            ShipperId = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(newOrder));
    }

    [Fact]
    public async Task CreateOrderAsync_WithNoOrderDetails_ThrowsArgumentException()
    {
        // Arrange
        var newOrder = new NewOrder
        {
            CustomerId = 1,
            EmpId = 1,
            ShipperId = 1,
            OrderDetails = new List<OrderDetail>() // Empty order details
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(newOrder));
    }

    [Fact]
    public async Task CreateOrderAsync_WithNegativeUnitPrice_ThrowsArgumentException()
    {
        // Arrange
        var newOrder = new NewOrder
        {
            CustomerId = 1,
            EmpId = 1,
            ShipperId = 1,
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    ProductId = 1,
                    UnitPrice = -10.00m, // Negative unit price
                    Qty = 1,
                    Discount = 0
                }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(newOrder));
    }

    [Fact]
    public async Task CreateOrderAsync_WithInvalidDiscount_ThrowsArgumentException()
    {
        // Arrange
        var newOrder = new NewOrder
        {
            CustomerId = 1,
            EmpId = 1,
            ShipperId = 1,
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    ProductId = 1,
                    UnitPrice = 10.00m,
                    Qty = 1,
                    Discount = 1.5m // Invalid discount > 1
                }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(newOrder));
    }
}