USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[delete_order]    Script Date: 16-02-2024 07:13:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[delete_order]
    @OrderId INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;


        -- Update stock quantity in product table
        UPDATE p
        SET p.product_quantity_in_stock = p.product_quantity_in_stock + od.order_detail_quantity
        FROM products p
        INNER JOIN order_details od ON p.product_id = od.product_id
        WHERE od.order_id = @OrderId;

		        -- Delete from order details
        DELETE FROM order_details WHERE order_id = @OrderId;


        -- Delete from orders
        DELETE FROM orders WHERE order_id = @OrderId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
