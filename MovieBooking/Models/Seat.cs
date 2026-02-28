namespace MovieBooking.Models
{
    public class Seat
    {
        public int SeatId { get; set; }

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;

        public string Row { get; set; } = string.Empty; // A, B, C, D, E, F
        public int Number { get; set; }                  // 1-9

        public string SeatCode => $"{Row}{Number}"; // A1, A2, etc.

        public List<BookingDetail> BookingDetails { get; set; } = new();
    }
}
