using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
}
