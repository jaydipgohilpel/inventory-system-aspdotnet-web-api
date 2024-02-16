USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[add_user_registration]    Script Date: 16-02-2024 07:13:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[add_user_registration]
(
    @name NVARCHAR(50),
    @email NVARCHAR(100),
    @password NVARCHAR(100)
    -- Add other parameters as needed for user registration
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the email already exists
    IF EXISTS (SELECT 1 FROM users WHERE email = @email)
    BEGIN
        -- Return a specific error code or message indicating that the email already exists
        RAISERROR ('Email already exists', 16, 1);
        RETURN -1; -- Return a negative value or any other value to indicate failure
    END

    -- Your logic to insert user registration data into the database table
    INSERT INTO users(name, email, password)
    VALUES (@name, @email, @password);

    -- Optional: You can return the newly inserted user's ID or any other relevant data
    SELECT SCOPE_IDENTITY() AS UserId; -- If you have an identity column for UserId
END;
