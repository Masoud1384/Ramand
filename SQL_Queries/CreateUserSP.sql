USE Ramand;
GO

CREATE PROCEDURE SelectUser
    @Id INT
AS
BEGIN
    SELECT * FROM users WHERE id = @Id
END
