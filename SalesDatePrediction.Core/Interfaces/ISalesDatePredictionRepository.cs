using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface ISalesDatePredictionRepository
{
    Task<SalesPrediction> GetPredictionAsync(int customerId);
    Task<IEnumerable<SalesPrediction>> GetAllPredictionsAsync();
}
