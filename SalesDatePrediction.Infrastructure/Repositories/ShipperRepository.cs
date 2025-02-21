using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;
using Dapper;

namespace SalesDatePrediction.Infrastructure.Repositories;

public class ShipperRepository : IShipperRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ShipperRepository> _logger;

    public ShipperRepository(IConfiguration configuration, ILogger<ShipperRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<IEnumerable<Shipper>> GetAllAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                SELECT 
                    shipperid as ShipperId,
                    companyname as CompanyName
                FROM Sales.Shippers
                ORDER BY shipperid";

            return await connection.QueryAsync<Shipper>(sql);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shippers");
            throw;
        }
    }
}
