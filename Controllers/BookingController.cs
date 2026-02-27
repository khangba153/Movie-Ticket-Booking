using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(int userId)
        {
            if (userId <= 0)
                return BadRequest("UserId không hợp lệ.");

            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            if (id <= 0)
                return BadRequest("Booking ID không hợp lệ.");

            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet("showtime/{showtimeId}")]
        public async Task<IActionResult> GetBookingsByShowtime(int showtimeId)
        {
            if (showtimeId <= 0)
                return BadRequest("Showtime ID không hợp lệ.");

            var bookings = await _bookingService.GetBookingsByShowtimeAsync(showtimeId);
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            // Validate request
            if (request == null)
                return BadRequest("Request không hợp lệ.");

            if (request.UserId <= 0)
                return BadRequest("UserId không hợp lệ.");

            if (request.ShowtimeId <= 0)
                return BadRequest("ShowtimeId không hợp lệ.");

            if (!request.SeatIds.Any())
                return BadRequest("Vui lòng chọn ít nhất một ghế.");

            // Constants - định nghĩa giá vé
            const decimal SEAT_PRICE = 150000m; // VND per seat
            const decimal VAT_RATE = 0.08m; // 8% VAT

            // Backend calculates price - NOT trusting the frontend
            var seatCount = request.SeatIds.Count;
            var subTotal = seatCount * SEAT_PRICE;
            var vat = subTotal * VAT_RATE;
            var calculatedTotal = subTotal + vat;

            try
            {
                var booking = new Booking
                {
                    UserId = request.UserId, // FIX: Thêm UserId
                    ShowtimeId = request.ShowtimeId,
                    TotalPrice = calculatedTotal,
                    CreatedAt = DateTime.Now
                };

                var createdBooking = await _bookingService.CreateBookingAsync(booking, request.SeatIds);
                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.BookingId }, createdBooking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }
    }

    public class CreateBookingRequest
    {
        public int UserId { get; set; } // FIX: Thêm UserId
        public int ShowtimeId { get; set; }
        public List<int> SeatIds { get; set; } = new();
        // TotalPrice bị bỏ qua - backend tính toán
    }
}
