using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;

namespace MovieBooking.Services
{
    public class SeatService : ISeatService
    {
        private readonly ApplicationDbContext _context;

        public SeatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Seat>> GetSeatsByShowtimeAsync(int showtimeId)
        {
            // Get showtime to find the cinema
            var showtime = await _context.Showtimes.FirstOrDefaultAsync(s => s.ShowtimeId == showtimeId);
            if (showtime == null)
                return new List<Seat>();

            // Get all seats for that cinema
            var seats = await _context.Seats
                .Where(s => s.CinemaId == showtime.CinemaId)
                .OrderBy(s => s.Row).ThenBy(s => s.Number)
                .ToListAsync();

            return seats;
        }

        public async Task<List<Seat>> GetSeatsByCinemaAsync(int cinemaId)
        {
            return await _context.Seats
                .Where(s => s.CinemaId == cinemaId)
                .OrderBy(s => s.Row).ThenBy(s => s.Number)
                .ToListAsync();
        }

        public async Task<Seat?> GetSeatByIdAsync(int seatId)
        {
            return await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == seatId);
        }
    }
}
