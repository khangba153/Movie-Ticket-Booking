using Microsoft.AspNetCore.Mvc;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/admin/bookings")]
    public class AdminBookingController : ControllerBase
    {
        private readonly IAdminBookingService _service;

        public AdminBookingController(IAdminBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromHeader(Name = "X-User-Id")] int userId,
            [FromQuery] int? movieId,
            [FromQuery] int? cinemaId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            try
            {
                var bookings = await _service.GetAllBookingsAsync(userId, movieId, cinemaId, fromDate, toDate);
                return Ok(bookings);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }
    }
}
