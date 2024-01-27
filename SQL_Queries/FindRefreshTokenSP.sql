USE Ramand;
GO 
CREATE PROCEDURE FindRefreshToken
    @RefreshToken NVARCHAR(MAX)
AS
BEGIN
    SELECT * FROM UserToken WHERE RefreshToken = @RefreshToken
END
