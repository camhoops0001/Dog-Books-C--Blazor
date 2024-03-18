USE master
GO
DROP DATABASE IF EXISTS DogBooks;
GO
CREATE DATABASE DogBooks;
GO

USE DogBooks;

CREATE TABLE Books (
  BookID INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incrementing primary key
  Title nvarchar(100) NOT NULL,
  Author nvarchar(100) NOT NULL,
  DogPictureUrl nvarchar(100) NOT NULL
);