USE Ramand;
GO 
CREATE PROCEDURE UpdateUserToken
    @Id INT,
    @Token NVARCHAR(MAX),
    @Expire DATETIME,
    @RefreshToken NVARCHAR(MAX),
    @RefreshTokenExp DATETIME
AS
BEGIN
    UPDATE UserToken
    SET Token = @Token, Expire = @Expire, RefreshToken = @RefreshToken, RefreshTokenExp = @RefreshTokenExp
    WHERE Id = @Id
END
