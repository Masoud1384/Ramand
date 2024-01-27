USE Ramand;
GO 
CREATE PROCEDURE DeleteUserToken
    @Id INT
AS
BEGIN
    DELETE FROM UserToken WHERE Id = @Id
END

