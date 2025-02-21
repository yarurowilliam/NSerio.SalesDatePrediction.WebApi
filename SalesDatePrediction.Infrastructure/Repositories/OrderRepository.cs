using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;
using System.Data;

namespace SalesDatePrediction.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly string _connectionString;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(
        IConfiguration configuration,
        ILogger<OrderRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<IEnumerable<ClientOrder>> GetCustomerOrdersAsync(int customerId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                    SELECT 
                        orderid as OrderId,
                        requireddate as RequiredDate,
                        shippeddate as ShippedDate,
                        shipname as ShipName,
                        shipaddress as ShipAddress,
                        shipcity as ShipCity
                    FROM Sales.Orders
                    WHERE custid = @CustomerId
                    ORDER BY orderid DESC";

            return await connection.QueryAsync<ClientOrder>(sql, new { CustomerId = customerId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error accessing database for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<int> CreateOrderAsync(NewOrder order)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var detailsTable = new DataTable();
            detailsTable.Columns.Add("ProductId", typeof(int));
            detailsTable.Columns.Add("UnitPrice", typeof(decimal));
            detailsTable.Columns.Add("Qty", typeof(short));
            detailsTable.Columns.Add("Discount", typeof(decimal));

            foreach (var detail in order.OrderDetails)
            {
                detailsTable.Rows.Add(
                    detail.ProductId,
                    detail.UnitPrice,
                    detail.Qty,
                    detail.Discount);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", order.CustomerId);   
            parameters.Add("@EmpId", order.EmpId);
            parameters.Add("@ShipperId", order.ShipperId);
            parameters.Add("@ShipName", order.ShipName);
            parameters.Add("@ShipAddress", order.ShipAddress);
            parameters.Add("@ShipCity", order.ShipCity);
            parameters.Add("@ShipCountry", order.ShipCountry);
            parameters.Add("@OrderDate", order.OrderDate);
            parameters.Add("@RequiredDate", order.RequiredDate);
            parameters.Add("@ShippedDate", order.ShippedDate);
            parameters.Add("@Freight", order.Freight);
            parameters.Add("@OrderDetails", detailsTable.AsTableValuedParameter("OrderDetailsTableType"));

            var result = await connection.QuerySingleAsync<int>(
                "Sales.sp_CreateOrder",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order for customer {CustomerId}", order.CustomerId);
            throw;
        }
    }
}