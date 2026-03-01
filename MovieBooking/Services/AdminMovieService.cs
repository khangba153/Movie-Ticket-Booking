using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public class AdminMovieService : IAdminMovieService
    {
        private readonly ApplicationDbContext _context;

        public AdminMovieService(ApplicationDbContext context)
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

        public async Task<List<Movie>> GetAllMoviesAsync(int userId)
        {
            await ValidateAdmin(userId);

            return await _context.Movies
                .OrderByDescending(m => m.MovieId)
                .ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int userId, int id)
        {
            await ValidateAdmin(userId);

            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> CreateMovieAsync(int userId, AdminMovieRequest request)
        {
            await ValidateAdmin(userId);

            var movie = new Movie
            {
                Title = request.Title,
                Description = request.Description,
                Genre = request.Genre,
                DurationMinutes = request.DurationMinutes,
                ReleaseDate = request.ReleaseDate,
                AgeRestriction = request.AgeRestriction,
                Cast = request.Cast,
                Director = request.Director,
                Producer = request.Producer,
                PosterUrl = request.PosterUrl,
                IsActive = request.IsActive
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie?> UpdateMovieAsync(int userId, int id, AdminMovieRequest request)
        {
            await ValidateAdmin(userId);

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return null;
            }

            movie.Title = request.Title;
            movie.Description = request.Description;
            movie.Genre = request.Genre;
            movie.DurationMinutes = request.DurationMinutes;
            movie.ReleaseDate = request.ReleaseDate;
            movie.AgeRestriction = request.AgeRestriction;
            movie.Cast = request.Cast;
            movie.Director = request.Director;
            movie.Producer = request.Producer;
            movie.PosterUrl = request.PosterUrl;
            movie.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<bool> DeleteMovieAsync(int userId, int id)
        {
            await ValidateAdmin(userId);

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return false;
            }

            movie.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
