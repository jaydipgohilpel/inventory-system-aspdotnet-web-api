USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[update_product]    Script Date: 16-02-2024 07:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[update_product]
(
		   @product_id int,
           @product_name nvarchar(50),
           @product_description nvarchar(100)=NULL,
           @product_quantity_in_stock int,
           @product_cost_price decimal(18,2),
           @product_selling_price decimal(18,2)=NULL,
           @product_supplier_id int=NULL,
           @product_reorder_point int=NULL
)
AS
BEGIN
    -- Check if @product_selling_price is NULL, if so, calculate it
    IF @product_selling_price IS NULL OR @product_selling_price = 0
    BEGIN
     SET @product_selling_price = @product_cost_price;
    END

	update [dbo].[products]
      set 
           
           product_name=@product_name,
           product_description=@product_description,
           product_quantity_in_stock=@product_quantity_in_stock,
           product_cost_price=@product_cost_price,
           product_selling_price=@product_selling_price,
           product_supplier_id=@product_supplier_id,
           product_reorder_point=  @product_reorder_point,
	       updated_at=GETDATE()
	where product_id=@product_id
END;
 