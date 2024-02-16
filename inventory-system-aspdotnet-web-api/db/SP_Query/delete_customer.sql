USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[delete_customer]    Script Date: 16-02-2024 07:13:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[delete_customer]
(
	@customer_id int
)
AS
BEGIN
    delete from customers
    where customer_id=@customer_id
END;
 