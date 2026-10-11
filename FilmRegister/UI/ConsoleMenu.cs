using FilmRegister.Data;
using FilmRegister.Models;
using Microsoft.Data.SqlClient;

namespace FilmRegister.UI;

public class ConsoleMenu
{
    private readonly MovieRepository _movieRepository;
    private readonly GenreRepository _genreRepository;

    public ConsoleMenu(MovieRepository movieRepository, GenreRepository genreRepository)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("--- Filmregister ---");
            Console.WriteLine("1. Visa alla filmer");
            Console.WriteLine("2. Sök filmer efter genre");
            Console.WriteLine("3. Lägg till film");
            Console.WriteLine("4. Ta bort film");
            Console.WriteLine("0. Avsluta");
            Console.Write("Välj: ");

            string? choice = Console.ReadLine();
            if (choice == "0")
            {
                return;
            }

            try
            {
                switch (choice)
                {
                    case "1": ShowAllMovies(); break;
                    case "2": SearchMoviesByGenre(); break;
                    case "3": AddMovie(); break;
                    case "4": DeleteMovie(); break;
                    default: Console.WriteLine("Ogiltigt val. Försök igen."); break;
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Databasfel: {ex.Message}");
            }
        }
    }

    private void ShowAllMovies()
    {
        PrintMovies(_movieRepository.GetAllMovies());
    }
    
    private void SearchMoviesByGenre()
    {
        string genre = ReadString("Ange genre: ");
        List<Movie> movies = _movieRepository.GetMoviesByGenre(genre);

        if (movies.Count == 0)
        {
            Console.WriteLine($"Inga filmer hittades för genren '{genre}'.");
            return;
        }
        PrintMovies(movies);
    }

    private void AddMovie()
    {
        List<Genre> genres = _genreRepository.GetAllGenres();

        string title = ReadString("Titel: ");
        int releaseYear = ReadInt("Utgivningsår: ");

        Console.WriteLine("Tillgängliga genrer:");
        foreach (var genre in genres)
        {
            Console.WriteLine($"{genre.GenreId}. {genre.Name}");
        }
        int genreId = ReadInt("Genre-id: ");

        if (!genres.Exists(g => g.GenreId == genreId))
        {
            Console.WriteLine("Ogiltigt genre-id.");
            return;
        }

        _movieRepository.AddMovie(new Movie { Title = title, ReleaseYear = releaseYear, GenreId = genreId });
        Console.WriteLine($"Filmen '{title}' har lagts till.");
    }

    private void DeleteMovie()
    {
        PrintMovies(_movieRepository.GetAllMovies(), showIds: true);
        int id = ReadInt("Ange id för filmen som ska tas bort: ");

        bool deleted = _movieRepository.DeleteMovie(id);
        Console.WriteLine(deleted ? $"Filmen med id {id} har tagits bort." : $"Ingen film med id {id} hittades.");
    }

    private static void PrintMovies(List<Movie> movies, bool showIds = false)
    {
        foreach (Movie movie in movies)
        {
            Console.WriteLine(showIds ? $"[{movie.MovieId}] {movie}" : movie.ToString());
        }
    }
    private static string ReadString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }
            Console.WriteLine("Fältet får inte vara tomt.");
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }
            Console.WriteLine("Ogiltig siffra.");
        }
    }

}
