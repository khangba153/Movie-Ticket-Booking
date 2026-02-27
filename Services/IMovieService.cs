using MovieBooking.Models;

namespace MovieBooking.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
    }
}
