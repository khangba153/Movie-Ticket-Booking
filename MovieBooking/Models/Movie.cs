namespace MovieBooking.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string PosterUrl { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? AgeRestriction { get; set; }
        public string? Cast { get; set; }
        public string? Director { get; set; }
        public string? Producer { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public List<Showtime> Showtimes { get; set; } = new();
    }
}