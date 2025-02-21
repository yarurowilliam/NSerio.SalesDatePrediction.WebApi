using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;
using Dapper;

namespace SalesDatePrediction.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(IConfiguration configuration, ILogger<ProductRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                SELECT 
                    productid as ProductId,
                    productname as ProductName
                FROM Production.Products
                ORDER BY productid";

            return await connection.QueryAsync<Product>(sql);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products");
            throw;
        }
    }
}
