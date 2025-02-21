using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Core.Interfaces;

public interface IShipperService
{
    Task<IEnumerable<Shipper>> GetAllAsync();
}
