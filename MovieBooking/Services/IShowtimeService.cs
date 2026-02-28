using MovieBooking.Models;

namespace MovieBooking.Services
{
    public interface IShowtimeService
    {
        Task<List<Showtime>> GetAllShowtimesAsync();
        Task<Showtime?> GetShowtimeByIdAsync(int id);
        Task<List<Showtime>> GetShowtimesByMovieAsync(int movieId);
        Task<List<Showtime>> GetShowtimesByCinemaAsync(int cinemaId);
        Task<List<Showtime>> GetShowtimesByMovieAndDate(int movieId, DateTime date);
    }
}
