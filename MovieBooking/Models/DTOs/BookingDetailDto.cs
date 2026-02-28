namespace MovieBooking.Models.DTOs
{
    public class BookingDetailDto
    {
        public int BookingId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; } = string.Empty;
        public string CinemaName { get; set; } = string.Empty;
        public List<string> Seats { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public string QrCodeData { get; set; } = string.Empty;
        public bool IsUpcoming { get; set; }
    }
}
