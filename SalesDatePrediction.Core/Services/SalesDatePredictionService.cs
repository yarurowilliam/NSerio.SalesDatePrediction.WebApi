using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Services;

public class SalesDatePredictionService : ISalesDatePredictionService
{
    private readonly ISalesDatePredictionRepository _repository;
    private readonly ILogger<SalesDatePredictionService> _logger;

    public SalesDatePredictionService(
        ISalesDatePredictionRepository repository,
        ILogger<SalesDatePredictionService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<SalesPrediction> GetPredictionAsync(int customerId)
    {
        try
        {
            var prediction = await _repository.GetPredictionAsync(customerId);
            if (prediction == null)
            {
                _logger.LogWarning("No prediction found for customer {CustomerId}", customerId);
                throw new KeyNotFoundException($"No prediction found for customer {customerId}");
            }

            ValidatePrediction(prediction);

            return prediction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting prediction for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<IEnumerable<SalesPrediction>> GetAllPredictionsAsync()
    {
        try
        {
            var predictions = await _repository.GetAllPredictionsAsync();

            foreach (var prediction in predictions)
            {
                ValidatePrediction(prediction);
            }

            return predictions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all predictions");
            throw;
        }
    }

    private void ValidatePrediction(SalesPrediction prediction)
    {
        if (prediction.NextPredictedOrder < DateTime.Now)
        {
            _logger.LogWarning("Prediction date is in the past for customer {CustomerName}", prediction.CustomerName);
        }
    }
}
