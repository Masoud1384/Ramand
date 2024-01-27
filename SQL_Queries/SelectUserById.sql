USE Ramand;
GO

CREATE PROCEDURE SelectUserById
    @Id INT
AS
BEGIN
    SELECT u.*, t.Token, t.Expire, t.RefreshToken, t.RefreshTokenExp
    FROM Users u
    LEFT JOIN UserToken t ON u.Id = t.Id
    WHERE u.Id = @Id
END
