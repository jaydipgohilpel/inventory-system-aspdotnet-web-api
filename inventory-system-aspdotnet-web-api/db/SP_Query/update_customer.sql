USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[update_customer]    Script Date: 16-02-2024 07:11:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[update_customer]
(
	@customer_id int,
    @customer_name nvarchar(50),
    @customer_email nvarchar(50),
    @customer_address nvarchar(50) = NULL,
    @customer_phone nvarchar(50) = NULL,
	@userId INT NULL
)
AS
BEGIN
    update customers
	set
	customer_name=@customer_name, 
	customer_email=@customer_email, 
	customer_address=@customer_address,
	customer_phone=@customer_phone,
	userId=@userId,
	updated_at=GETDATE()
    where customer_id=@customer_id

	
    SELECT * FROM customers WHERE customer_id = @customer_id;
END;
 