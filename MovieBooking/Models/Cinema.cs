namespace MovieBooking.Models
{
    public class Cinema
    {
        public int CinemaId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public List<Showtime> Showtimes { get; set; } = new();
        public List<Seat> Seats { get; set; } = new();
    }
}