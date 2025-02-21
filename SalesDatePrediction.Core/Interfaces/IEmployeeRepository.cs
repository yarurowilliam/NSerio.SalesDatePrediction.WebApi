using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
}
