using FilmRegister.Data;
using FilmRegister.Models;
using FilmRegister.UI;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var movieRepository = new MovieRepository(DatabaseConfig.ConnectionString);
        var genreRepository = new GenreRepository(DatabaseConfig.ConnectionString);

        var menu = new ConsoleMenu(movieRepository, genreRepository);
        menu.Run();
    }
}