using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models.DTOs;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/admin/movies")]
    public class AdminMovieController : ControllerBase
    {
        private readonly IAdminMovieService _service;

        public AdminMovieController(IAdminMovieService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader(Name = "X-User-Id")] int userId)
        {
            try
            {
                var movies = await _service.GetAllMoviesAsync(userId);
                return Ok(movies);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromHeader(Name = "X-User-Id")] int userId, int id)
        {
            try
            {
                var movie = await _service.GetMovieByIdAsync(userId, id);
                if (movie == null)
                    return NotFound(new { message = "Không tìm thấy phim." });
                return Ok(movie);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader(Name = "X-User-Id")] int userId, [FromBody] AdminMovieRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                    return BadRequest(new { message = "Tên phim không được để trống." });

                if (request.DurationMinutes <= 0)
                    return BadRequest(new { message = "Thời lượng phim phải lớn hơn 0." });

                var movie = await _service.CreateMovieAsync(userId, request);
                return Ok(movie);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromHeader(Name = "X-User-Id")] int userId, int id, [FromBody] AdminMovieRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                    return BadRequest(new { message = "Tên phim không được để trống." });

                var movie = await _service.UpdateMovieAsync(userId, id, request);
                if (movie == null)
                    return NotFound(new { message = "Không tìm thấy phim." });
                return Ok(movie);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromHeader(Name = "X-User-Id")] int userId, int id)
        {
            try
            {
                var result = await _service.DeleteMovieAsync(userId, id);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy phim." });
                return Ok(new { message = "Đã vô hiệu hóa phim." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }
    }
}
