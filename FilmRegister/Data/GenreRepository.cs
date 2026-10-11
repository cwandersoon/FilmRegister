using FilmRegister.Models;
using Microsoft.Data.SqlClient;

namespace FilmRegister.Data
{
    public class GenreRepository
    {
        private readonly string _connectionString;

        public GenreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Genre> GetAllGenres()
        {
            var genres = new List<Genre>();
            const string sql = "SELECT GenreId, Name FROM Genre ORDER BY Name";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                genres.Add(new Genre
                {
                    GenreId = reader.GetInt32(0),
                    Name = reader.GetString(1)
                }); 
            }
            return genres;
        }
    }
}