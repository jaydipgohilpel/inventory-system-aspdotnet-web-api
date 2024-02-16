USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[add_order]    Script Date: 16-02-2024 07:13:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[add_order]
    @CustomerId INT,
    @ProductId INT,
    @OrderDetailQuantity INT,
    @OrderDetailUnitPrice DECIMAL(18, 2),
	@OrderDate DATETIME = NULL
AS
BEGIN	
	 DECLARE @ProductQuantityInStock INT;
    DECLARE @OrderTotalAmount DECIMAL(18, 2);
	 IF @OrderDate IS NULL
        SET @OrderDate = GETDATE();


    -- Get the product quantity in stock
    SELECT @ProductQuantityInStock = product_quantity_in_stock
    FROM products
    WHERE product_id = @ProductId;

    -- Check if the order quantity exceeds the available stock quantity
    IF @OrderDetailQuantity > @ProductQuantityInStock
    BEGIN
        RAISERROR ('Order quantity exceeds available stock quantity.', 16, 1);
        RETURN;
    END

    -- Calculate the order total amount
    SET @OrderTotalAmount = @OrderDetailQuantity * @OrderDetailUnitPrice;

    -- Insert a new order
    INSERT INTO orders (customer_id, order_total_amount, order_status,order_date)
    VALUES (@CustomerId, @OrderTotalAmount, 'Completed',@OrderDate);

    -- Insert the order detail record
    INSERT INTO order_details (order_id, product_id, order_detail_quantity, order_detail_unit_price)
    VALUES (SCOPE_IDENTITY(), @ProductId, @OrderDetailQuantity, @OrderDetailUnitPrice);

    -- Deduct the ordered quantity from the product stock quantity
    UPDATE products
    SET product_quantity_in_stock = product_quantity_in_stock - @OrderDetailQuantity,
	updated_at=GETDATE()
    WHERE product_id = @ProductId;
    
END
