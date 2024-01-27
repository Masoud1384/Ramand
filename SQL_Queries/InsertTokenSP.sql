USE Ramand;
GO
CREATE PROCEDURE UpsertUserToken
    @Id INT,
    @Token NVARCHAR(MAX),
    @Expire DATETIME,
    @RefreshToken NVARCHAR(MAX),
    @RefreshTokenExp DATETIME
AS
BEGIN
    IF EXISTS (SELECT 1 FROM UserToken WHERE Id = @Id)
    BEGIN
        UPDATE UserToken
        SET Token = @Token, Expire = @Expire, RefreshToken = @RefreshToken, RefreshTokenExp = @RefreshTokenExp
        WHERE Id = @Id
    END
    ELSE
    BEGIN
        INSERT INTO UserToken(Id, Token, Expire, RefreshToken, RefreshTokenExp)
        VALUES (@Id, @Token, @Expire, @RefreshToken, @RefreshTokenExp)
    END
END
