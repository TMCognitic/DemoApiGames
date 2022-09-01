CREATE DATABASE DemoApi;
GO

USE DemoApi
GO

CREATE TABLE Contact
(
	Id INT IDENTITY,
	LastName NVARCHAR(50) NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_Contact PRIMARY KEY (Id)
);
GO

Insert Into Contact (LastName, FirstName) VALUES
 (N'Doe', N'John')
,(N'Doe', N'Jane');
GO