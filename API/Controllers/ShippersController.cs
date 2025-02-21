using Microsoft.AspNetCore.Mvc;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippersController : ControllerBase
{
    private readonly IShipperService _shipperService;
    private readonly ILogger<ShippersController> _logger;

    public ShippersController(
        IShipperService shipperService,
        ILogger<ShippersController> logger)
    {
        _shipperService = shipperService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Shipper>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Shipper>>> GetAll()
    {
        try
        {
            var shippers = await _shipperService.GetAllAsync();
            return Ok(shippers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling request");
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }
}
