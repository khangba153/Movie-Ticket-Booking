using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public class AdminBookingService : IAdminBookingService
    {
        private readonly ApplicationDbContext _context;

        public AdminBookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task ValidateAdmin(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thực hiện thao tác này.");
            }
        }

        public async Task<List<AdminBookingResponse>> GetAllBookingsAsync(int userId, int? movieId = null, int? cinemaId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            await ValidateAdmin(userId);

            var query = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Showtime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Showtime)
                    .ThenInclude(s => s.Cinema)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Seat)
                .AsQueryable();

            if (movieId.HasValue)
            {
                query = query.Where(b => b.Showtime.MovieId == movieId.Value);
            }

            if (cinemaId.HasValue)
            {
                query = query.Where(b => b.Showtime.CinemaId == cinemaId.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(b => b.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(b => b.CreatedAt <= toDate.Value);
            }

            var bookings = await query
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(b => new AdminBookingResponse
            {
                BookingId = b.BookingId,
                Username = b.User.Username,
                MovieTitle = b.Showtime.Movie.Title,
                CinemaName = b.Showtime.Cinema.Name,
                StartTime = b.Showtime.StartTime,
                Seats = b.BookingDetails
                    .OrderBy(bd => bd.Seat.Row)
                    .ThenBy(bd => bd.Seat.Number)
                    .Select(bd => $"{bd.Seat.Row}{bd.Seat.Number}")
                    .ToList(),
                TotalPrice = b.TotalPrice,
                BookingDate = b.CreatedAt
            }).ToList();
        }
    }
}
