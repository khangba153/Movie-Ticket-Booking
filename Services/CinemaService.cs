using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;

namespace MovieBooking.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly ApplicationDbContext _context;

        public CinemaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cinema>> GetAllCinemasAsync()
        {
            return await _context.Cinemas.ToListAsync();
        }

        public async Task<Cinema?> GetCinemaByIdAsync(int id)
        {
            return await _context.Cinemas.FirstOrDefaultAsync(c => c.CinemaId == id);
        }
    }
}
