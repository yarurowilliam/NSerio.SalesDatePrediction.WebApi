using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Services;

public class ShipperService : IShipperService
{
    private readonly IShipperRepository _repository;
    private readonly ILogger<ShipperService> _logger;

    public ShipperService(
        IShipperRepository repository,
        ILogger<ShipperService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Shipper>> GetAllAsync()
    {
        try
        {
            var shippers = await _repository.GetAllAsync();

            if (!shippers.Any())
            {
                _logger.LogWarning("No shippers found");
            }

            return shippers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving shippers");
            throw;
        }
    }
}
