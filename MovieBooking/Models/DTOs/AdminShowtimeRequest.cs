namespace MovieBooking.Models.DTOs
{
    public class AdminShowtimeRequest
    {
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
    }
}
