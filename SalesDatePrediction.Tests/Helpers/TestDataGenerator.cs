using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Tests.Helpers;

public static class TestDataGenerator
{
    public static List<Employee> CreateTestEmployees()
    {
        return new List<Employee>
        {
            new() { EmpId = 1, FullName = "John Doe" },
            new() { EmpId = 2, FullName = "Jane Smith" }
        };
    }

    public static List<Shipper> CreateTestShippers()
    {
        return new List<Shipper>
        {
            new() { ShipperId = 1, CompanyName = "Fast Delivery" },
            new() { ShipperId = 2, CompanyName = "Quick Ship" }
        };
    }

    public static List<Product> CreateTestProducts()
    {
        return new List<Product>
        {
            new() { ProductId = 1, ProductName = "Product 1" },
            new() { ProductId = 2, ProductName = "Product 2" }
        };
    }

    public static NewOrder CreateValidOrder()
    {
        return new NewOrder
        {
            CustomerId = 1,
            EmpId = 1,
            ShipperId = 1,
            ShipName = "Test Customer",
            ShipAddress = "Test Address",
            ShipCity = "Test City",
            ShipCountry = "Test Country",
            OrderDate = DateTime.UtcNow,
            RequiredDate = DateTime.UtcNow.AddDays(7),
            Freight = 10.00m,
            OrderDetails = new List<OrderDetail>
            {
                new()
                {
                    ProductId = 1,
                    UnitPrice = 10.00m,
                    Qty = 1,
                    Discount = 0
                }
            }
        };
    }
}