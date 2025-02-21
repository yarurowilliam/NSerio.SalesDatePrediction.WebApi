using Microsoft.AspNetCore.Mvc;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderService orderService,
        ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(IEnumerable<ClientOrder>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ClientOrder>>> GetCustomerOrders(int customerId)
    {
        try
        {
            var orders = await _orderService.GetCustomerOrdersAsync(customerId);
            if (!orders.Any())
                return NotFound($"No orders found for customer {customerId}");

            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders for customer {CustomerId}", customerId);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> CreateOrder([FromBody] NewOrder order)
    {
        try
        {
            if (order == null)
            {
                return BadRequest(new ErrorResponse("Order cannot be null"));
            }

            var orderId = await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(CreateOrder), new { id = orderId }, orderId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new ErrorResponse("An error occurred while processing your request"));
        }
    }
}
