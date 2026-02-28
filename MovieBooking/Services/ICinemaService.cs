using MovieBooking.Models;

namespace MovieBooking.Services
{
    public interface ICinemaService
    {
        Task<List<Cinema>> GetAllCinemasAsync();
        Task<Cinema?> GetCinemaByIdAsync(int id);
    }
}
