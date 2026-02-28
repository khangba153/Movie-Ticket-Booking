using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public interface IBookingService
    {
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<List<Booking>> GetBookingsByShowtimeAsync(int showtimeId);
        Task<Booking> CreateBookingAsync(Booking booking, List<int> seatIds);
        Task<List<int>> GetBookedSeatIdsByShowtimeAsync(int showtimeId);
        Task<List<BookingDetailDto>> GetUserBookingsAsync(int userId);
    }
}
