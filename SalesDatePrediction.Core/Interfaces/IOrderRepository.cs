using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<ClientOrder>> GetCustomerOrdersAsync(int customerId);
    Task<int> CreateOrderAsync(NewOrder order);
}
