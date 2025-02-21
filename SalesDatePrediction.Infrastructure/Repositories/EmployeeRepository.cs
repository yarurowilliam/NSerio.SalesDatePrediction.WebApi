using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(IConfiguration configuration, ILogger<EmployeeRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                SELECT 
                    empid as EmpId,
                    firstname + ' ' + lastname as FullName
                FROM HR.Employees
                ORDER BY empid";

            return await connection.QueryAsync<Employee>(sql);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employees");
            throw;
        }
    }
}
