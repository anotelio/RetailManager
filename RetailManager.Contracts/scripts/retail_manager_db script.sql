CREATE DATABASE retail_manager_db
GO

USE retail_manager_db
GO

CREATE TABLE dbo.customers(
	customer_id	BIGINT IDENTITY PRIMARY KEY NOT NULL,
	customer_name NVARCHAR(255) NOT NULL
)
GO

INSERT INTO	dbo.customers
	(customer_name)
VALUES
	( N'Mr. Sidr'),
	( N'Ms. Kaldybarasova'),
	( N'Mr. Stepan')
GO