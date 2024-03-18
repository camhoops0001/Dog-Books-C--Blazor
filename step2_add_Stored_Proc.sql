USE DogBooks
GO
CREATE OR ALTER PROCEDURE spGetSavedBooks
AS
BEGIN
SELECT
    b.BookID
  , b.Title
  , b.Author
  , b.DogPictureUrl
FROM [DogBooks].[dbo].[Books] as b;
END;

GO
CREATE OR ALTER PROCEDURE spSaveBook (@Title nvarchar(100), @Author nvarchar(100), @DogPictureUrl nvarchar(100))
AS
BEGIN
  INSERT INTO dbo.Books (Title, Author, DogPictureUrl)
  VALUES (@Title, @Author, @DogPictureUrl);
END;