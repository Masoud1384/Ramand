USE Ramand;
GO 
CREATE PROCEDURE SelectUserToken
    @Id INT
AS
BEGIN
    SELECT * FROM UserToken WHERE Id = @Id
END
