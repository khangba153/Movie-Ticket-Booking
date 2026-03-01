using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public interface IAdminMovieService
    {
        Task<List<Movie>> GetAllMoviesAsync(int userId);
        Task<Movie?> GetMovieByIdAsync(int userId, int id);
        Task<Movie> CreateMovieAsync(int userId, AdminMovieRequest request);
        Task<Movie?> UpdateMovieAsync(int userId, int id, AdminMovieRequest request);
        Task<bool> DeleteMovieAsync(int userId, int id);
    }
}
