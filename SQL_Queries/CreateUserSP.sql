USE Ramand;
GO

USE Ramand;
GO

CREATE PROCEDURE InsertUsersAndTokens @json NVARCHAR(MAX)
AS
BEGIN
    DECLARE @jsonTable TABLE (username NVARCHAR(255), password NVARCHAR(120), jwtToken NVARCHAR(255), expire DATETIME, refreshToken NVARCHAR(50), refreshTokenExp DATETIME)

    INSERT INTO @jsonTable
    SELECT * 
    FROM OPENJSON (@json)
    WITH (username NVARCHAR(255) '$.username', password NVARCHAR(120) '$.password')

   INSERT INTO users 
   SELECT username , password FROM @jsonTable J
   
  -- where username not in (select username from users)

END
