USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[update_order]    Script Date: 16-02-2024 07:11:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[update_order]
    @OrderId INT,
    @CustomerId INT,
    @ProductId INT,
    @OrderDetailQuantity INT,
    @OrderDetailUnitPrice DECIMAL(18, 2),
	@OrderDate DATETIME = NULL
AS
BEGIN
	DECLARE @OldOrderDetailQuantity INT;
	DECLARE @ProductQuantityInStock INT;
    DECLARE @OrderTotalAmount DECIMAL(18, 2);
	 IF @OrderDate IS NULL
        SET @OrderDate = GETDATE();
		DECLARE @newQty INT;


    BEGIN TRY
        BEGIN TRANSACTION;


		 -- Select the old quantity from order_details
        SELECT @OldOrderDetailQuantity = order_detail_quantity
        FROM order_details
        WHERE order_id = @OrderId;


		    -- Get the product quantity in stock
		SELECT @ProductQuantityInStock = product_quantity_in_stock
		FROM products
		WHERE product_id = @ProductId;

		
		SET @newQty=((@ProductQuantityInStock+@OldOrderDetailQuantity) - @OrderDetailQuantity)
		
				-- Check if the order quantity exceeds the available stock quantity
			IF  @newQty <0
			BEGIN
				RAISERROR ('Order quantity exceeds available stock quantity or is negative.', 16, 1);
			RETURN;
			END

		
        -- Update the order
        UPDATE orders
        SET customer_id = @CustomerId,
		order_total_amount=(@OrderDetailUnitPrice*@OrderDetailQuantity),
		updated_at=GETDATE(),
		order_date=@OrderDate
        WHERE order_id = @OrderId;

        -- Update the order detail

		
        UPDATE order_details
        SET product_id = @ProductId,
            order_detail_quantity = @OrderDetailQuantity,
            order_detail_unit_price = @OrderDetailUnitPrice,
			updated_at=GETDATE()
        WHERE order_id = @OrderId;


		 -- Deduct the ordered quantity from the product stock quantity
    UPDATE products
    SET product_quantity_in_stock = (product_quantity_in_stock+@OldOrderDetailQuantity) - @OrderDetailQuantity,
	updated_at=GETDATE()
    WHERE product_id = @ProductId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
