CREATE TYPE OrderDetailsTableType AS TABLE
(
    ProductId INT,
    UnitPrice MONEY,
    Qty SMALLINT,
    Discount NUMERIC(4,3)
);

CREATE OR ALTER PROCEDURE Sales.sp_CreateOrder
    @CustomerId INT,         
    @EmpId INT,
    @ShipperId INT,
    @ShipName NVARCHAR(40),
    @ShipAddress NVARCHAR(60),
    @ShipCity NVARCHAR(15),
    @ShipCountry NVARCHAR(15),
    @OrderDate DATETIME2,
    @RequiredDate DATETIME2,
    @ShippedDate DATETIME2 = NULL,
    @Freight MONEY,
    @OrderDetails OrderDetailsTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Sales.Customers WHERE custid = @CustomerId)
        BEGIN
            THROW 50000, 'Customer not found', 1;
        END

        DECLARE @OrderId INT;
        
        INSERT INTO Sales.Orders (
            custid,             
            empid, 
            shipperid, 
            shipname, 
            shipaddress, 
            shipcity, 
            shipcountry, 
            orderdate, 
            requireddate, 
            shippeddate, 
            freight)
        VALUES (
            @CustomerId,       
            @EmpId, 
            @ShipperId, 
            @ShipName, 
            @ShipAddress, 
            @ShipCity,
            @ShipCountry, 
            @OrderDate, 
            @RequiredDate, 
            @ShippedDate, 
            @Freight);
        
        SET @OrderId = SCOPE_IDENTITY();
        
        INSERT INTO Sales.OrderDetails (
            orderid, 
            productid, 
            unitprice, 
            qty, 
            discount)
        SELECT 
            @OrderId,
            ProductId,
            UnitPrice,
            Qty,
            Discount
        FROM @OrderDetails;
        
        COMMIT TRANSACTION;
        SELECT @OrderId AS OrderId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END