namespace MovieBooking.Models
{
    public class BookingDetail
    {
        public int BookingDetailId { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;
    }
}
