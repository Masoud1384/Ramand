USE Ramand;
GO

CREATE PROCEDURE CountUsers
AS
BEGIN
    SELECT COUNT(*) AS UserCount FROM users;
END;