USE Ramand;
GO 
CREATE PROCEDURE CreateUserTokenTable
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tokens')
    CREATE TABLE UserToken
    (
        Id INT PRIMARY KEY FOREIGN KEY REFERENCES users(id),
        Token NVARCHAR(MAX),
        Expire DATETIME,
        RefreshToken NVARCHAR(MAX),
        RefreshTokenExp DATETIME
    )
END
