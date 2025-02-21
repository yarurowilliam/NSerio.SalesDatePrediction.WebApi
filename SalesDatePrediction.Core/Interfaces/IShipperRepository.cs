using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface IShipperRepository
{
    Task<IEnumerable<Shipper>> GetAllAsync();
}
