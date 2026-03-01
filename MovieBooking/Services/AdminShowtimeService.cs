using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public class AdminShowtimeService : IAdminShowtimeService
    {
        private readonly ApplicationDbContext _context;

        public AdminShowtimeService(ApplicationDbContext context)
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

        public async Task<List<AdminShowtimeResponse>> GetAllShowtimesAsync(int userId, int? movieId = null, int? cinemaId = null, DateTime? date = null)
        {
            await ValidateAdmin(userId);

            var query = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .AsQueryable();

            if (movieId.HasValue)
            {
                query = query.Where(s => s.MovieId == movieId.Value);
            }

            if (cinemaId.HasValue)
            {
                query = query.Where(s => s.CinemaId == cinemaId.Value);
            }

            if (date.HasValue)
            {
                query = query.Where(s => s.StartTime.Date == date.Value.Date);
            }

            var showtimes = await query
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            return showtimes.Select(s => new AdminShowtimeResponse
            {
                ShowtimeId = s.ShowtimeId,
                MovieId = s.MovieId,
                MovieTitle = s.Movie.Title,
                CinemaId = s.CinemaId,
                CinemaName = s.Cinema.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Price = s.Price
            }).ToList();
        }

        public async Task<Showtime> CreateShowtimeAsync(int userId, AdminShowtimeRequest request)
        {
            await ValidateAdmin(userId);

            var movie = await _context.Movies.FindAsync(request.MovieId);
            if (movie == null)
            {
                throw new InvalidOperationException($"Phim với ID {request.MovieId} không tồn tại.");
            }

            var cinemaExists = await _context.Cinemas.AnyAsync(c => c.CinemaId == request.CinemaId);
            if (!cinemaExists)
            {
                throw new InvalidOperationException($"Rạp với ID {request.CinemaId} không tồn tại.");
            }

            var showtime = new Showtime
            {
                MovieId = request.MovieId,
                CinemaId = request.CinemaId,
                StartTime = request.StartTime,
                EndTime = request.StartTime.AddMinutes(movie.DurationMinutes),
                Price = request.Price
            };

            _context.Showtimes.Add(showtime);
            await _context.SaveChangesAsync();

            return showtime;
        }

        public async Task<Showtime?> UpdateShowtimeAsync(int userId, int id, AdminShowtimeRequest request)
        {
            await ValidateAdmin(userId);

            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime == null)
            {
                return null;
            }

            var movie = await _context.Movies.FindAsync(request.MovieId);
            if (movie == null)
            {
                throw new InvalidOperationException($"Phim với ID {request.MovieId} không tồn tại.");
            }

            var cinemaExists = await _context.Cinemas.AnyAsync(c => c.CinemaId == request.CinemaId);
            if (!cinemaExists)
            {
                throw new InvalidOperationException($"Rạp với ID {request.CinemaId} không tồn tại.");
            }

            showtime.MovieId = request.MovieId;
            showtime.CinemaId = request.CinemaId;
            showtime.StartTime = request.StartTime;
            showtime.EndTime = request.StartTime.AddMinutes(movie.DurationMinutes);
            showtime.Price = request.Price;

            await _context.SaveChangesAsync();

            return showtime;
        }

        public async Task<bool> DeleteShowtimeAsync(int userId, int id)
        {
            await ValidateAdmin(userId);

            var showtime = await _context.Showtimes
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(s => s.ShowtimeId == id);

            if (showtime == null)
            {
                return false;
            }

            if (showtime.Bookings.Any())
            {
                throw new InvalidOperationException("Không thể xóa suất chiếu đã có đơn đặt vé.");
            }

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
