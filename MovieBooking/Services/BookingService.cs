using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Models;
using MovieBooking.Models.DTOs;

namespace MovieBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Seat)
                .Include(b => b.Showtime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Showtime)
                    .ThenInclude(s => s.Cinema)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<List<Booking>> GetBookingsByShowtimeAsync(int showtimeId)
        {
            return await _context.Bookings
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Seat)
                .Include(b => b.Showtime)
                .Include(b => b.User)
                .Where(b => b.ShowtimeId == showtimeId)
                .ToListAsync();
        }

        public async Task<Booking> CreateBookingAsync(Booking booking, List<int> seatIds)
        {
            // Validate 1: UserId
            if (booking.UserId <= 0)
            {
                throw new InvalidOperationException("UserId không hợp lệ. Vui lòng đăng nhập để đặt vé.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == booking.UserId);
            if (!userExists)
            {
                throw new InvalidOperationException($"User với ID {booking.UserId} không tồn tại.");
            }

            // Validate 2: Showtime tồn tại và còn trong tương lai
            var showtime = await _context.Showtimes
                .Include(s => s.Cinema)
                .FirstOrDefaultAsync(s => s.ShowtimeId == booking.ShowtimeId);
            
            if (showtime == null)
            {
                throw new InvalidOperationException($"Suất chiếu với ID {booking.ShowtimeId} không tồn tại.");
            }

            if (showtime.StartTime <= DateTime.Now)
            {
                throw new InvalidOperationException("Không thể đặt vé cho suất chiếu đã qua hoặc đang chiếu.");
            }

            // Validate 3: Tất cả seats tồn tại và thuộc đúng Cinema của Showtime
            var validSeats = await _context.Seats
                .Where(s => seatIds.Contains(s.SeatId))
                .ToListAsync();
            
            if (validSeats.Count != seatIds.Count)
            {
                throw new InvalidOperationException("Một số ghế không tồn tại trong hệ thống.");
            }

            var wrongCinemaSeats = validSeats.Where(s => s.CinemaId != showtime.CinemaId).ToList();
            if (wrongCinemaSeats.Any())
            {
                var wrongSeatCodes = wrongCinemaSeats.Select(s => $"{s.Row}{s.Number}");
                throw new InvalidOperationException($"Ghế {string.Join(", ", wrongSeatCodes)} không thuộc rạp {showtime.Cinema.Name}.");
            }

            // Validate 4: Seats chưa được book (trong transaction để tránh race condition)
            var bookedSeatIds = await GetBookedSeatIdsByShowtimeAsync(booking.ShowtimeId);
            var conflictingSeatIds = seatIds.Intersect(bookedSeatIds).ToList();
            
            if (conflictingSeatIds.Any())
            {
                var conflictingSeatCodes = validSeats
                    .Where(s => conflictingSeatIds.Contains(s.SeatId))
                    .Select(s => $"{s.Row}{s.Number}")
                    .ToList();
                throw new InvalidOperationException($"Ghế {string.Join(", ", conflictingSeatCodes)} đã được đặt. Vui lòng chọn ghế khác.");
            }

            // Use transaction to ensure atomicity
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create booking
                    _context.Bookings.Add(booking);
                    await _context.SaveChangesAsync();

                    // Create booking details for each seat
                    foreach (int seatId in seatIds)
                    {
                        var detail = new BookingDetail
                        {
                            BookingId = booking.BookingId,
                            SeatId = seatId
                        };
                        _context.BookingDetails.Add(detail);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    
                    // FIX 3: Reload booking với đầy đủ relationships
                    var createdBooking = await _context.Bookings
                        .Include(b => b.BookingDetails)
                            .ThenInclude(bd => bd.Seat)
                        .Include(b => b.Showtime)
                        .Include(b => b.User)
                        .FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);
                    
                    return createdBooking ?? booking;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException($"Lỗi khi tạo booking: {ex.Message}", ex);
                }
            }
        }

        public async Task<List<int>> GetBookedSeatIdsByShowtimeAsync(int showtimeId)
        {
            return await _context.BookingDetails
                .Where(bd => bd.Booking.ShowtimeId == showtimeId)
                .Select(bd => bd.SeatId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<BookingDetailDto>> GetUserBookingsAsync(int userId)
        {
            // Optimize: Chỉ Include Showtime 1 lần với cả Movie và Cinema
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Showtime.Movie)
                .Include(b => b.Showtime.Cinema)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Seat)
                .OrderByDescending(b => b.Showtime.StartTime)
                .ToListAsync();

            var result = bookings.Select(b => new BookingDetailDto
            {
                BookingId = b.BookingId,
                MovieTitle = b.Showtime.Movie.Title,
                ShowDate = b.Showtime.StartTime.Date,
                ShowTime = b.Showtime.StartTime.ToString("HH:mm"),
                CinemaName = b.Showtime.Cinema.Name,
                Seats = b.BookingDetails
                    .OrderBy(bd => bd.Seat.Row)
                    .ThenBy(bd => bd.Seat.Number)
                    .Select(bd => $"{bd.Seat.Row}{bd.Seat.Number}")
                    .ToList(),
                TotalPrice = b.TotalPrice,
                QrCodeData = $"BOOKING-{b.BookingId}",
                IsUpcoming = b.Showtime.StartTime > DateTime.Now
            }).ToList();

            return result;
        }
    }
}
