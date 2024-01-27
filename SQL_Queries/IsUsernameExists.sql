USE Ramand;
GO

CREATE PROCEDURE IsUsernameExist
    @username NVARCHAR(255)
AS
BEGIN
    SELECT CASE WHEN EXISTS (
        SELECT * FROM users WHERE username = @username
    )
    THEN 1 ELSE 0 END
END;