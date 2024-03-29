USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[get_all_product]    Script Date: 16-02-2024 07:11:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[get_all_product]
AS
BEGIN
    SELECT [product_id]
      ,[product_name]
      ,[product_description]
      ,[product_quantity_in_stock]
      ,[product_cost_price]
      ,[product_selling_price]
      ,[created_at]
	  ,[product_supplier_id]
      ,[product_reorder_point]
  FROM [inventory_system].[dbo].[products]
  order by created_at desc
END;
 