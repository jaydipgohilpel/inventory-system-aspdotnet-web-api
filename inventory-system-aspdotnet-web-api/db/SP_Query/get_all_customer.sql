USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[get_all_customer]    Script Date: 16-02-2024 07:13:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[get_all_customer]
AS
BEGIN
    SELECT * from customers
	order by created_at desc

END;
 