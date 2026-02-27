using Microsoft.AspNetCore.Mvc;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly IBookingService _bookingService;

        public SeatController(ISeatService seatService, IBookingService bookingService)
        {
            _seatService = seatService;
            _bookingService = bookingService;
        }

        [HttpGet("showtime/{showtimeId}")]
        public async Task<IActionResult> GetSeatsByShowtime(int showtimeId)
        {
            if (showtimeId <= 0)
                return BadRequest("Showtime ID không hợp lệ.");

            try
            {
                var seats = await _seatService.GetSeatsByShowtimeAsync(showtimeId);
                var bookedSeatIds = await _bookingService.GetBookedSeatIdsByShowtimeAsync(showtimeId);

                var seatDtos = seats.Select(s => new
                {
                    seatId = s.SeatId,
                    row = s.Row,
                    number = s.Number,
                    seatCode = s.SeatCode,
                    isBooked = bookedSeatIds.Contains(s.SeatId)
                }).ToList();

                return Ok(seatDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetSeatsByShowtime: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new { error = "Lỗi khi lấy danh sách ghế.", details = ex.Message }); // FIX: Trả về error thay vì empty array
            }
        }

        [HttpGet("cinema/{cinemaId}")]
        public async Task<IActionResult> GetSeatsByCinema(int cinemaId)
        {
            if (cinemaId <= 0)
                return BadRequest("Cinema ID không hợp lệ.");

            var seats = await _seatService.GetSeatsByCinemaAsync(cinemaId);
            return Ok(seats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            if (id <= 0)
                return BadRequest("Seat ID không hợp lệ.");

            var seat = await _seatService.GetSeatByIdAsync(id);
            if (seat == null)
                return NotFound();

            return Ok(seat);
        }
    }
}
