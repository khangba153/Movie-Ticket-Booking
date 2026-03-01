namespace MovieBooking.Models.DTOs
{
    public class AdminBookingResponse
    {
        public int BookingId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string CinemaName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public List<string> Seats { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
