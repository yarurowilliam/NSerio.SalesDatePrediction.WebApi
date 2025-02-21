namespace SalesDatePrediction.Core.Models;

public class NewOrder
{
    public int CustomerId { get; set; } 
    public int EmpId { get; set; }
    public int ShipperId { get; set; }
    public string ShipName { get; set; }
    public string ShipAddress { get; set; }
    public string ShipCity { get; set; }
    public string ShipCountry { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public decimal Freight { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}