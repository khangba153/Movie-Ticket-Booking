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

        [HttpGet]
        public async Task<IActionResult> GetShowtimes()
        {
            var showtimes = await _service.GetAllShowtimesAsync();
            return Ok(showtimes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowtimeById(int id)
        {
            if (id <= 0)
                return BadRequest("Showtime ID không hợp lệ.");

            var showtime = await _service.GetShowtimeByIdAsync(id);

            if (showtime == null)
                return NotFound();

            return Ok(showtime);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowtimesByMovie(int movieId)
        {
            if (movieId <= 0)
                return BadRequest("Movie ID không hợp lệ.");

            var showtimes = await _service.GetShowtimesByMovieAsync(movieId);
            return Ok(showtimes);
        }

        [HttpGet("cinema/{cinemaId}")]
        public async Task<IActionResult> GetShowtimesByCinema(int cinemaId)
        {
            if (cinemaId <= 0)
                return BadRequest("Cinema ID không hợp lệ.");

            var showtimes = await _service.GetShowtimesByCinemaAsync(cinemaId);
            return Ok(showtimes);
        }

        [HttpGet("movie/{movieId}/date/{date}")]
        public async Task<IActionResult> GetByMovieAndDate(int movieId, DateTime date)
        {
            if (movieId <= 0)
                return BadRequest("Movie ID không hợp lệ.");

            var showtimes = await _service.GetShowtimesByMovieAndDate(movieId, date);
            return Ok(showtimes);
        }
    }
}
