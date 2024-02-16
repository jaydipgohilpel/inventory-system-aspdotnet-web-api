USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[add_customer]    Script Date: 16-02-2024 07:13:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[add_customer]
(
    @customer_name nvarchar(50),
    @customer_email nvarchar(50),
    @customer_address nvarchar(50) = NULL,
    @customer_phone nvarchar(50) = NULL,
	@userId int =NULL
)
AS
BEGIN

	  -- Check if the email already exists
    IF EXISTS (SELECT 1 FROM customers WHERE customer_address = @customer_address)
    BEGIN
        -- Return a specific error code or message indicating that the email already exists
        RAISERROR ('Email already exists', 16, 1);
        RETURN -1; -- Return a negative value or any other value to indicate failure
    END

    INSERT INTO customers(customer_name, customer_email, customer_address, customer_phone,userId)
    VALUES (@customer_name, @customer_email, @customer_address, @customer_phone,@userId);

    -- Return all data for the newly inserted customer
    SELECT * FROM customers WHERE customer_id = SCOPE_IDENTITY();
END;
 