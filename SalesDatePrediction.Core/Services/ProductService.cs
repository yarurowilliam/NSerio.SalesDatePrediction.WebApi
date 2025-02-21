using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository repository,
        ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            var products = await _repository.GetAllAsync();

            if (!products.Any())
            {
                _logger.LogWarning("No products found");
            }

            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            throw;
        }
    }
}
