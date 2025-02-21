namespace SalesDatePrediction.Core.Models;

public class SalesPrediction
{
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime LastOrderDate { get; set; }
    public DateTime NextPredictedOrder { get; set; }
}