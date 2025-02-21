using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<ClientOrder>> GetCustomerOrdersAsync(int customerId);
    Task<int> CreateOrderAsync(NewOrder order);
}
