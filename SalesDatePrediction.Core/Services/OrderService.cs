using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ClientOrder>> GetCustomerOrdersAsync(int customerId)
    {
        try
        {
            var orders = await _orderRepository.GetCustomerOrdersAsync(customerId);

            if (!orders.Any())
            {
                _logger.LogWarning("No orders found for customer {CustomerId}", customerId);
            }

            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<int> CreateOrderAsync(NewOrder order)
    {
        try
        {
            ValidateOrder(order);

            order.OrderDate = order.OrderDate == default ? DateTime.UtcNow : order.OrderDate;

            if (order.RequiredDate == default)
            {
                order.RequiredDate = order.OrderDate.AddDays(7);
            }

            return await _orderRepository.CreateOrderAsync(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            throw;
        }
    }

    private void ValidateOrder(NewOrder order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (order.CustomerId <= 0)
            throw new ArgumentException("Invalid customer ID", nameof(order.CustomerId));

        if (order.EmpId <= 0)
            throw new ArgumentException("Invalid employee ID", nameof(order.EmpId));

        if (order.ShipperId <= 0)
            throw new ArgumentException("Invalid shipper ID", nameof(order.ShipperId));

        if (string.IsNullOrWhiteSpace(order.ShipName))
            throw new ArgumentException("Ship name is required", nameof(order.ShipName));

        if (!order.OrderDetails.Any())
            throw new ArgumentException("Order must have at least one detail", nameof(order.OrderDetails));

        foreach (var detail in order.OrderDetails)
        {
            if (detail.ProductId <= 0)
                throw new ArgumentException("Invalid product ID", nameof(detail.ProductId));

            if (detail.UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative", nameof(detail.UnitPrice));

            if (detail.Qty <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(detail.Qty));

            if (detail.Discount < 0 || detail.Discount > 1)
                throw new ArgumentException("Discount must be between 0 and 1", nameof(detail.Discount));
        }
    }
}
