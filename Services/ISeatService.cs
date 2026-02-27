using MovieBooking.Models;

namespace MovieBooking.Services
{
    public interface ISeatService
    {
        Task<List<Seat>> GetSeatsByShowtimeAsync(int showtimeId);
        Task<List<Seat>> GetSeatsByCinemaAsync(int cinemaId);
        Task<Seat?> GetSeatByIdAsync(int seatId);
    }
}
