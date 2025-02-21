namespace SalesDatePrediction.Core.Models;

public class OrderDetail
{
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public short Qty { get; set; }
    public decimal Discount { get; set; }
}
