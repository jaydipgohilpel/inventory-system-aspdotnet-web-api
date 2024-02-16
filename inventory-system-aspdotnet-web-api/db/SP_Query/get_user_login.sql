USE [inventory_system]
GO
/****** Object:  StoredProcedure [dbo].[get_user_login]    Script Date: 16-02-2024 07:11:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[get_user_login]
(
    @email NVARCHAR(50),
    @password VARCHAR(100)
)
AS
BEGIN
    DECLARE @UserExists INT;

    -- Check if the email exists
    SELECT @UserExists = COUNT(*) 
    FROM users 
    WHERE email = @email;

    -- If no matching email is found, raise an error
    IF @UserExists = 0
    BEGIN
        RAISERROR ('Email not found.', 16, 1);
        RETURN;
    END

    -- If the email exists, check if the password matches
    DECLARE @CorrectPassword BIT;
    SELECT @CorrectPassword = CASE WHEN password = @password THEN 1 ELSE 0 END 
    FROM users 
    WHERE email = @email;

    -- If the password is incorrect, raise an error
    IF @CorrectPassword = 0
    BEGIN
        RAISERROR ('Incorrect password.', 16, 1);
        RETURN;
    END

    -- If both email and password are correct, return the user details
    SELECT TOP 1 UserId, name, email, password,created_at 
    FROM users 
    WHERE email = @email 
      AND password = @password;
END
