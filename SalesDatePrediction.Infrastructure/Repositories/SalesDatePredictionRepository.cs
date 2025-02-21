using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Infrastructure.Repositories;

public class SalesDatePredictionRepository : ISalesDatePredictionRepository
{
    private readonly string _connectionString;

    public SalesDatePredictionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<SalesPrediction> GetPredictionAsync(int customerId)
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            WITH OrderDates AS (
                SELECT 
                    O.custid,
                    O.orderdate,
                    LEAD(O.orderdate) OVER (PARTITION BY O.custid ORDER BY O.orderdate) as next_order_date
                FROM Sales.Orders O
                WHERE O.custid = @CustomerId
            ),
            AverageDays AS (
                SELECT 
                    custid,
                    AVG(DATEDIFF(day, orderdate, next_order_date)) as avg_days
                FROM OrderDates
                WHERE next_order_date IS NOT NULL
                GROUP BY custid
            ),
            LastOrder AS (
                SELECT TOP 1
                    O.custid as CustomerId,
                    C.companyname as CustomerName,
                    O.orderdate as LastOrderDate,
                    COALESCE(AVG(DATEDIFF(day, O1.orderdate, O1.next_order_date)), 30) as avg_days
                FROM Sales.Orders O
                JOIN Sales.Customers C ON C.custid = O.custid
                LEFT JOIN OrderDates O1 ON O1.custid = O.custid
                WHERE O.custid = @CustomerId
                GROUP BY C.companyname, O.orderdate, O.custid
                ORDER BY O.orderdate DESC
            )
            SELECT 
                LO.CustomerName,
                FORMAT(LO.LastOrderDate, 'yyyy-MM-dd HH:mm:ss.fff') as LastOrderDate,
                FORMAT(DATEADD(day, CAST(LO.avg_days as int), LO.LastOrderDate), 'yyyy-MM-dd HH:mm:ss.fff') as NextPredictedOrder
            FROM LastOrder LO;";

        return await connection.QueryFirstOrDefaultAsync<SalesPrediction>(sql, new { CustomerId = customerId });
    }

    public async Task<IEnumerable<SalesPrediction>> GetAllPredictionsAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            WITH OrderDifferences AS (
                SELECT 
                    o.custid,
                    o.orderdate,
                    LAG(o.orderdate) OVER (PARTITION BY o.custid ORDER BY o.orderdate) AS prev_order_date
                FROM StoreSample.Sales.Orders o
            ),
            AvgOrderIntervals AS (
                SELECT 
                    custid,
                    MAX(orderdate) AS LastOrderDate,
                    AVG(DATEDIFF(DAY, prev_order_date, orderdate)) AS AvgDaysBetweenOrders
                FROM OrderDifferences
                WHERE prev_order_date IS NOT NULL
                GROUP BY custid
            )
            SELECT
                a.custId as CustomerId,
                c.companyname AS CustomerName,
                a.LastOrderDate,
                DATEADD(DAY, a.AvgDaysBetweenOrders, a.LastOrderDate) AS NextPredictedOrder
            FROM AvgOrderIntervals a
            JOIN StoreSample.Sales.Customers c ON a.custid = c.custid
            ORDER BY CustomerName;";

        return await connection.QueryAsync<SalesPrediction>(sql);
    }
}