USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[delete_product]    Script Date: 16-02-2024 07:13:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[delete_product]
(
	@product_id int
)
AS
BEGIN
    delete from products
    where product_id=@product_id
END;
 