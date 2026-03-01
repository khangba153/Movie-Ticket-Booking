using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models.DTOs;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/admin/showtimes")]
    public class AdminShowtimeController : ControllerBase
    {
        private readonly IAdminShowtimeService _service;

        public AdminShowtimeController(IAdminShowtimeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromHeader(Name = "X-User-Id")] int userId,
            [FromQuery] int? movieId,
            [FromQuery] int? cinemaId,
            [FromQuery] DateTime? date)
        {
            try
            {
                var showtimes = await _service.GetAllShowtimesAsync(userId, movieId, cinemaId, date);
                return Ok(showtimes);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader(Name = "X-User-Id")] int userId, [FromBody] AdminShowtimeRequest request)
        {
            try
            {
                if (request.Price <= 0)
                    return BadRequest(new { message = "Giá vé phải lớn hơn 0." });

                var showtime = await _service.CreateShowtimeAsync(userId, request);
                return Ok(showtime);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromHeader(Name = "X-User-Id")] int userId, int id, [FromBody] AdminShowtimeRequest request)
        {
            try
            {
                var showtime = await _service.UpdateShowtimeAsync(userId, id, request);
                if (showtime == null)
                    return NotFound(new { message = "Không tìm thấy suất chiếu." });
                return Ok(showtime);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromHeader(Name = "X-User-Id")] int userId, int id)
        {
            try
            {
                var result = await _service.DeleteShowtimeAsync(userId, id);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy suất chiếu." });
                return Ok(new { message = "Đã xóa suất chiếu thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
