USE [Ramand]
GO
/****** Object:  StoredProcedure [dbo].[SelectAllUsers]    Script Date: 2/5/2024 11:11:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SelectAllUsers]
AS
BEGIN
    SELECT * FROM users
END