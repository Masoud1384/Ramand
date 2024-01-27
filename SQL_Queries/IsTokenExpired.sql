USE Ramand;
GO

CREATE PROCEDURE IsTokenExpired
    @Token VARCHAR(MAX)
AS
BEGIN
    DECLARE @CurrentDateTime DATETIME;
    SET @CurrentDateTime = GETDATE();

    IF EXISTS (SELECT 1 FROM UserToken WHERE Token = @Token AND Expire > @CurrentDateTime)
        SELECT 'true' AS IsTokenExpired;
    ELSE
        SELECT 'false' AS IsTokenExpired;
END
