using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync();
}
