USE Ramand;
GO 
CREATE PROCEDURE CreateUserTokenTable
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tokens')
    CREATE TABLE UserToken
    (
        Id INT PRIMARY KEY FOREIGN KEY REFERENCES users(id),
        Token NVARCHAR(128),
        Expire DATETIME,
        RefreshToken NVARCHAR(128),
        RefreshTokenExp DATETIME
    )
END
