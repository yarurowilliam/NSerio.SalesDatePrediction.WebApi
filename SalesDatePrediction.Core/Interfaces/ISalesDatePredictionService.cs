using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface ISalesDatePredictionService
{
    Task<SalesPrediction> GetPredictionAsync(int customerId);
    Task<IEnumerable<SalesPrediction>> GetAllPredictionsAsync();
}
