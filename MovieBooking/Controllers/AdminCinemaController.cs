using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models.DTOs;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/admin/cinemas")]
    public class AdminCinemaController : ControllerBase
    {
        private readonly IAdminCinemaService _service;

        public AdminCinemaController(IAdminCinemaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader(Name = "X-User-Id")] int userId)
        {
            try
            {
                var cinemas = await _service.GetAllCinemasAsync(userId);
                return Ok(cinemas);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader(Name = "X-User-Id")] int userId, [FromBody] AdminCinemaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest(new { message = "Tên rạp không được để trống." });

                if (string.IsNullOrWhiteSpace(request.Address))
                    return BadRequest(new { message = "Địa chỉ không được để trống." });

                var cinema = await _service.CreateCinemaAsync(userId, request);
                return Ok(cinema);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromHeader(Name = "X-User-Id")] int userId, int id, [FromBody] AdminCinemaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest(new { message = "Tên rạp không được để trống." });

                var cinema = await _service.UpdateCinemaAsync(userId, id, request);
                if (cinema == null)
                    return NotFound(new { message = "Không tìm thấy rạp." });
                return Ok(cinema);
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
                var result = await _service.DeleteCinemaAsync(userId, id);
                if (!result)
                    return NotFound(new { message = "Không tìm thấy rạp." });
                return Ok(new { message = "Đã xóa rạp thành công." });
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
