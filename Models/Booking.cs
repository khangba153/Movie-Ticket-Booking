namespace MovieBooking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ShowtimeId { get; set; }
        public Showtime Showtime { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<BookingDetail> BookingDetails { get; set; } = new();
    }
}
