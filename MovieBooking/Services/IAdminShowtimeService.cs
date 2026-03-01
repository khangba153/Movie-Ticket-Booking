using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public interface IAdminShowtimeService
    {
        Task<List<AdminShowtimeResponse>> GetAllShowtimesAsync(int userId, int? movieId = null, int? cinemaId = null, DateTime? date = null);
        Task<Showtime> CreateShowtimeAsync(int userId, AdminShowtimeRequest request);
        Task<Showtime?> UpdateShowtimeAsync(int userId, int id, AdminShowtimeRequest request);
        Task<bool> DeleteShowtimeAsync(int userId, int id);
    }
}
