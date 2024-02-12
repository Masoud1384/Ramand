USE Ramand;
GO

CREATE PROCEDURE CreateUsersTable
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'users')
    BEGIN
        CREATE TABLE users (
            id INT IDENTITY(1,1) PRIMARY KEY,
            [password] NVARCHAR(50) NULL,
            [username] NVARCHAR(128) NULL
        );
    END
END;
