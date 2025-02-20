IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'OrderingDB')
    BEGIN
        CREATE DATABASE OrderingDB;
        PRINT 'Database created successfully';
    END
ELSE
    BEGIN
        PRINT 'Database already exists';
    END