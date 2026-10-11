using FilmRegister.Models;
using Microsoft.Data.SqlClient;

namespace FilmRegister.Data;

public class MovieRepository
{
    private readonly string _connectionString;
    public MovieRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Movie> GetAllMovies()
    {
        const string sql = @"
        SELECT m.MovieId, m.Title, m.ReleaseYear, m.GenreId, g.Name
        FROM Movie m
        INNER JOIN Genre g ON m.GenreId = g.GenreId
        ORDER BY m.Title";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();

        return ReadMovies(command);
    }

    public List<Movie> GetMoviesByGenre(string genreName)
    {
        const string sql = @"
        SELECT m.MovieId, m.Title, m.ReleaseYear, m.GenreId, g.Name
        FROM Movie m
        INNER JOIN Genre g ON m.GenreId = g.GenreId
        WHERE g.Name LIKE @genreName
        ORDER BY m.Title";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@genreName", $"%{genreName}%");
        connection.Open();

        return ReadMovies(command);
    }

    public void AddMovie(Movie movie)
    {
        const string sql = @"
            INSERT INTO Movie (Title, ReleaseYear, GenreId)
            VALUES (@title, @releaseYear, @genreId)";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@title", movie.Title);
        command.Parameters.AddWithValue("@releaseYear", movie.ReleaseYear);
        command.Parameters.AddWithValue("@genreId", movie.GenreId);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public bool DeleteMovie(int movieId)
    {
        const string sql = "DELETE FROM Movie WHERE MovieId = @movieId";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@movieId", movieId);
        connection.Open();

        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    private static List<Movie> ReadMovies(SqlCommand command)
    {
        var movies = new List<Movie>();

        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = reader.GetInt32(0),
                Title = reader.GetString(1),
                ReleaseYear = reader.GetInt32(2),
                GenreId = reader.GetInt32(3),
                GenreName = reader.GetString(4)
            });
        }

        return movies;
    }
}