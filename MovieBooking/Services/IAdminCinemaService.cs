using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public interface IAdminCinemaService
    {
        Task<List<Cinema>> GetAllCinemasAsync(int userId);
        Task<Cinema> CreateCinemaAsync(int userId, AdminCinemaRequest request);
        Task<Cinema?> UpdateCinemaAsync(int userId, int id, AdminCinemaRequest request);
        Task<bool> DeleteCinemaAsync(int userId, int id);
    }
}
