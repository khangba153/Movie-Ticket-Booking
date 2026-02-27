namespace MovieBooking.Models
{
    public class Showtime
    {
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<Booking> Bookings { get; set; } = new();
    }
}