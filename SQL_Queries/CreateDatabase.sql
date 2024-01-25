IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'YourDatabaseName')
BEGIN
    CREATE DATABASE Ramand;
END;
