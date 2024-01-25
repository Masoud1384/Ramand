USE Ramand;
GO

CREATE PROCEDURE InsertUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    INSERT INTO users(username, password)
    VALUES (@Username, @Password)
END
