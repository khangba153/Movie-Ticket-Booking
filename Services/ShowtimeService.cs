using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;

namespace MovieBooking.Services
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly ApplicationDbContext _context;

        public ShowtimeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Showtime>> GetAllShowtimesAsync()
        {
            return await _context.Showtimes
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .Where(s => s.StartTime > DateTime.Now) // FIX: Chỉ trả về showtime tương lai
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<Showtime?> GetShowtimeByIdAsync(int id)
        {
            return await _context.Showtimes
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.ShowtimeId == id);
        }

        public async Task<List<Showtime>> GetShowtimesByMovieAsync(int movieId)
        {
            return await _context.Showtimes
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .Where(s => s.MovieId == movieId && s.StartTime > DateTime.Now) // FIX: Chỉ showtime tương lai
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<Showtime>> GetShowtimesByCinemaAsync(int cinemaId)
        {
            return await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .Where(s => s.CinemaId == cinemaId && s.StartTime > DateTime.Now) // FIX: Chỉ showtime tương lai
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<Showtime>> GetShowtimesByMovieAndDate(int movieId, DateTime date)
        {
            var targetDate = date.Date;
            return await _context.Showtimes
                .Include(s => s.Cinema)
                .Include(s => s.Movie)
                .Where(s => s.MovieId == movieId 
                    && s.StartTime.Date == targetDate
                    && s.StartTime > DateTime.Now) // FIX: Không cho book showtime quá khứ
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }
    }
}
