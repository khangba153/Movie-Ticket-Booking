using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public interface IAdminBookingService
    {
        Task<List<AdminBookingResponse>> GetAllBookingsAsync(int userId, int? movieId = null, int? cinemaId = null, DateTime? fromDate = null, DateTime? toDate = null);
    }
}
