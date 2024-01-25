USE Ramand;
GO

CREATE PROCEDURE UpdateUser
    @Id INT,
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    UPDATE users
    SET username = @Username, password = @Password
    WHERE Id = @Id
END
