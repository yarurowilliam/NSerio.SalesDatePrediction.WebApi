using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository repository,
        ILogger<EmployeeService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        try
        {
            var employees = await _repository.GetAllAsync();

            if (!employees.Any())
            {
                _logger.LogWarning("No employees found");
            }

            return employees;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            throw;
        }
    }
}
