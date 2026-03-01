using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public class AdminCinemaService : IAdminCinemaService
    {
        private readonly ApplicationDbContext _context;

        public AdminCinemaService(ApplicationDbContext context)
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

        public async Task<List<Cinema>> GetAllCinemasAsync(int userId)
        {
            await ValidateAdmin(userId);

            return await _context.Cinemas.ToListAsync();
        }

        public async Task<Cinema> CreateCinemaAsync(int userId, AdminCinemaRequest request)
        {
            await ValidateAdmin(userId);

            var cinema = new Cinema
            {
                Name = request.Name,
                Address = request.Address
            };

            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();

            // Auto-generate 54 seats (6 rows A-F x 9 columns)
            var rows = new[] { "A", "B", "C", "D", "E", "F" };
            foreach (var row in rows)
            {
                for (int col = 1; col <= 9; col++)
                {
                    var seat = new Seat
                    {
                        CinemaId = cinema.CinemaId,
                        Row = row,
                        Number = col
                    };
                    _context.Seats.Add(seat);
                }
            }

            await _context.SaveChangesAsync();

            return cinema;
        }

        public async Task<Cinema?> UpdateCinemaAsync(int userId, int id, AdminCinemaRequest request)
        {
            await ValidateAdmin(userId);

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return null;
            }

            cinema.Name = request.Name;
            cinema.Address = request.Address;

            await _context.SaveChangesAsync();

            return cinema;
        }

        public async Task<bool> DeleteCinemaAsync(int userId, int id)
        {
            await ValidateAdmin(userId);

            var cinema = await _context.Cinemas
                .Include(c => c.Showtimes)
                    .ThenInclude(s => s.Bookings)
                .Include(c => c.Seats)
                .FirstOrDefaultAsync(c => c.CinemaId == id);

            if (cinema == null)
            {
                return false;
            }

            // Check if cinema has any bookings through its showtimes
            var hasBookings = cinema.Showtimes.Any(s => s.Bookings.Any());
            if (hasBookings)
            {
                throw new InvalidOperationException("Không thể xóa rạp đã có đơn đặt vé.");
            }

            // Delete related seats first
            _context.Seats.RemoveRange(cinema.Seats);

            // Delete related showtimes (no bookings, safe to remove)
            _context.Showtimes.RemoveRange(cinema.Showtimes);

            // Delete cinema
            _context.Cinemas.Remove(cinema);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
