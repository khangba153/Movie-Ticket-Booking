using Microsoft.AspNetCore.Mvc;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimeService _service;

        public ShowtimeController(IShowtimeService service)
        {
            _service = service;
        }

        // Project showtime to flat DTO to avoid circular reference bloat
        private object MapShowtime(MovieBooking.Models.Showtime s) => new
        {
            s.ShowtimeId,
            s.MovieId,
            s.CinemaId,
            s.StartTime,
            s.EndTime,
            s.Price,
            Movie = s.Movie == null ? null : new
            {
                s.Movie.MovieId,
                s.Movie.Title,
                s.Movie.DurationMinutes,
                s.Movie.PosterUrl,
                s.Movie.Genre,
                s.Movie.AgeRestriction
            },
            Cinema = s.Cinema == null ? null : new
            {
                s.Cinema.CinemaId,
                s.Cinema.Name,
                s.Cinema.Address
            }
        };

        [HttpGet]
        public async Task<IActionResult> GetShowtimes()
        {
            var showtimes = await _service.GetAllShowtimesAsync();
            return Ok(showtimes.Select(MapShowtime));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowtimeById(int id)
        {
            if (id <= 0)
                return BadRequest("Showtime ID không hợp lệ.");

            var showtime = await _service.GetShowtimeByIdAsync(id);

            if (showtime == null)
                return NotFound();

            return Ok(MapShowtime(showtime));
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowtimesByMovie(int movieId)
        {
            if (movieId <= 0)
                return BadRequest("Movie ID không hợp lệ.");

            var showtimes = await _service.GetShowtimesByMovieAsync(movieId);
            return Ok(showtimes.Select(MapShowtime));
        }

        [HttpGet("cinema/{cinemaId}")]
        public async Task<IActionResult> GetShowtimesByCinema(int cinemaId)
        {
            if (cinemaId <= 0)
                return BadRequest("Cinema ID không hợp lệ.");

            var showtimes = await _service.GetShowtimesByCinemaAsync(cinemaId);
            return Ok(showtimes.Select(MapShowtime));
        }

        [HttpGet("movie/{movieId}/date/{date}")]
        public async Task<IActionResult> GetByMovieAndDate(int movieId, DateTime date)
        {
            if (movieId <= 0)
                return BadRequest("Movie ID không hợp lệ.");

            var showtimes = await _service.GetShowtimesByMovieAndDate(movieId, date);
            return Ok(showtimes.Select(MapShowtime));
        }
    }
}
