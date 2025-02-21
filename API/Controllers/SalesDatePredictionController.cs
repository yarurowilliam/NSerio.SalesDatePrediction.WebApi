using Microsoft.AspNetCore.Mvc;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesDatePredictionController : ControllerBase
{
    private readonly ISalesDatePredictionService _predictionService;
    private readonly ILogger<SalesDatePredictionController> _logger;

    public SalesDatePredictionController(
        ISalesDatePredictionService predictionService,
        ILogger<SalesDatePredictionController> logger)
    {
        _predictionService = predictionService;
        _logger = logger;
    }

    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(SalesPrediction), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SalesPrediction>> GetPrediction(int customerId)
    {
        try
        {
            var prediction = await _predictionService.GetPredictionAsync(customerId);
            return Ok(prediction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing prediction request for customer {CustomerId}", customerId);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SalesPrediction>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SalesPrediction>>> GetAllPredictions()
    {
        try
        {
            var predictions = await _predictionService.GetAllPredictionsAsync();
            return Ok(predictions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request for all predictions");
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }
}
