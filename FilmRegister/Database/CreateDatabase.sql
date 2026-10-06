IF DB_ID('FilmRegister') IS NULL
	CREATE DATABASE FilmRegister;
GO

USE FilmRegister;
GO

DROP TABLE IF EXISTS Movie;
DROP TABLE IF EXISTS Genre;
GO

CREATE TABLE Genre (
	GenreID INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Movie (
	MovieID INT PRIMARY KEY IDENTITY(1,1),
	Title NVARCHAR(200) NOT NULL,
	ReleaseYear INT NOT NULL,
	GenreID INT NOT NULL,
	FOREIGN KEY (GenreID) REFERENCES Genre(GenreID)
);
GO

INSERT INTO Genre (Name) VALUES
('Action'),
('Comedy'),
('Drama'),
('Horror'),
('Sci-Fi');
GO

INSERT INTO Movie (Title, ReleaseYear, GenreID) VALUES
('The Matrix', 1999, 5),
('The Godfather', 1972, 3),
('Inception', 2010, 5),
('The Hangover', 2009, 2),
('Scream', 1996, 4),
('Alien', 1979, 5),
('The Shining', 1980, 4),
('Die Hard', 1988, 1),
('The Shawshank Redemption', 1994, 3),
('Groundhog Day', 1993, 2),
('Mad Max: Fury Road', 2015, 1);
GO