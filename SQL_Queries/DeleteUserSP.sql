USE Ramand;
GO

CREATE PROCEDURE DeleteUser
    @username NVARCHAR(255)
AS
BEGIN
    DELETE FROM users WHERE username= @username
END
