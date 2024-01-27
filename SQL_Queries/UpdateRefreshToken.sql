USE Ramand;
GO 
CREATE PROCEDURE UpdateRefreshToken
    @Id INT,
    @RefreshToken NVARCHAR(MAX),
    @RefreshTokenExp DATETIME
AS
BEGIN
    UPDATE UserToken
    SET RefreshToken = @RefreshToken, RefreshTokenExp = @RefreshTokenExp
    WHERE Id = @Id
END
