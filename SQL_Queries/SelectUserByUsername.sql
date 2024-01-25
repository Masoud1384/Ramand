USE Ramand;
GO

CREATE PROCEDURE SelectUserByUsername
    @Username NVARCHAR(255)
AS
BEGIN
    SELECT * FROM users WHERE username = @Username;
END;
