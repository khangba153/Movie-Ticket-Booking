using Microsoft.EntityFrameworkCore;
using MovieBooking.Models;

namespace MovieBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Định nghĩa DbSet cho các entity
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for TotalPrice
            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(18, 2);

            // Configure FK constraints
            modelBuilder.Entity<BookingDetail>()
                .HasOne(bd => bd.Seat)
                .WithMany(s => s.BookingDetails)
                .HasForeignKey(bd => bd.SeatId)
                .OnDelete(DeleteBehavior.NoAction);

            // Unique index on Username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Seed test user first
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "testuser",
                    Name = "Test User",
                    Email = "test@example.com",
                    PasswordHash = "dummy_hash"
                }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    MovieId = 1,
                    Title = "Inception",
                    DurationMinutes = 148,
                    PosterUrl = "/images/inception.jpg",
                    Description = "Phim khoa học viễn tưởng về giấc mơ.",
                    Genre = "Sci-Fi, Action",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    AgeRestriction = "T13",
                    Cast = "Leonardo DiCaprio",
                    Director = "Christopher Nolan",
                    Producer = "Emma Thomas",
                    IsActive = true
                },
                new Movie
                {
                    MovieId = 2,
                    Title = "Kung Fu Panda",
                    DurationMinutes = 92,
                    PosterUrl = "/images/kungfu.jpg",
                    Description = "Chú gấu trúc Po trở thành chiến binh rồng bảo vệ thung lũng.",
                    Genre = "Animation, Comedy",
                    ReleaseDate = new DateTime(2008, 6, 6),
                    AgeRestriction = "P",
                    Cast = "Jack Black, Dustin Hoffman",
                    Director = "Mark Osborne, John Stevenson",
                    Producer = "Melissa Cobb",
                    IsActive = true
                },
                new Movie
                {
                    MovieId = 3,
                    Title = "Iron Man",
                    DurationMinutes = 126,
                    PosterUrl = "/images/ironman.jpg",
                    Description = "Tony Stark chế tạo bộ giáp công nghệ cao và trở thành Iron Man.",
                    Genre = "Action, Superhero",
                    ReleaseDate = new DateTime(2008, 5, 2),
                    AgeRestriction = "T13",
                    Cast = "Robert Downey Jr., Gwyneth Paltrow",
                    Director = "Jon Favreau",
                    Producer = "Kevin Feige",
                    IsActive = true
                },

                new Movie
                {
                    MovieId = 4,
                    Title = "Pirates of the Caribbean",
                    DurationMinutes = 143,
                    PosterUrl = "/images/pirates.jpg",
                    Description = "Thuyền trưởng Jack Sparrow phiêu lưu trên biển Caribbean.",
                    Genre = "Adventure, Fantasy",
                    ReleaseDate = new DateTime(2003, 7, 9),
                    AgeRestriction = "T13",
                    Cast = "Johnny Depp, Orlando Bloom",
                    Director = "Gore Verbinski",
                    Producer = "Jerry Bruckheimer",
                    IsActive = true
                }

            );

            // Seed Cinemas
            modelBuilder.Entity<Cinema>().HasData(
                new Cinema { CinemaId = 1, Name = "CGV Vincom", Address = "Quận 1" },
                new Cinema { CinemaId = 2, Name = "Lotte Cinema", Address = "Quận 7" },
                new Cinema { CinemaId = 3, Name = "Galaxy Nguyen Du", Address = "Quận 1" },
                new Cinema { CinemaId = 4, Name = "BHD Star", Address = "Quận 10" }
            );

            // Seed Showtimes (giả sử MovieId 1,2,3 đã tồn tại)
            modelBuilder.Entity<Showtime>().HasData(
                new Showtime
                {
                    ShowtimeId = 1,
                    MovieId = 1,
                    CinemaId = 1,
                    StartTime = new DateTime(2026, 2, 27, 19, 0, 0),
                    EndTime = new DateTime(2026, 2, 27, 21, 0, 0)
                },
                new Showtime
                {
                    ShowtimeId = 2,
                    MovieId = 1,
                    CinemaId = 2,
                    StartTime = new DateTime(2026, 2, 27, 21, 30, 0),
                    EndTime = new DateTime(2026, 2, 27, 23, 30, 0)
                },
                new Showtime
                {
                    ShowtimeId = 3,
                    MovieId = 1,
                    CinemaId = 1,
                    StartTime = new DateTime(2026, 2, 27, 14, 30, 0),
                    EndTime = new DateTime(2026, 2, 27, 16, 30, 0)
                },
                new Showtime
                {
                    ShowtimeId = 4,
                    MovieId = 1,
                    CinemaId = 2,
                    StartTime = new DateTime(2026, 2, 27, 17, 0, 0),
                    EndTime = new DateTime(2026, 2, 27, 19, 0, 0)
                },
                new Showtime
                {
                    ShowtimeId = 5,
                    MovieId = 1,
                    CinemaId = 3,
                    StartTime = new DateTime(2026, 2, 28, 18, 0, 0),
                    EndTime = new DateTime(2026, 2, 28, 20, 0, 0)
                },
                new Showtime
                {
                    ShowtimeId = 6,
                    MovieId = 2,
                    CinemaId = 1,
                    StartTime = new DateTime(2026, 2, 27, 17, 0, 0),
                    EndTime = new DateTime(2026, 2, 27, 18, 40, 0)
                },
                new Showtime
                {
                    ShowtimeId = 7,
                    MovieId = 2,
                    CinemaId = 4,
                    StartTime = new DateTime(2026, 2, 28, 20, 0, 0),
                    EndTime = new DateTime(2026, 2, 28, 22, 0, 0)
                },
                new Showtime
                {
                    ShowtimeId = 8,
                    MovieId = 3,
                    CinemaId = 2,
                    StartTime = new DateTime(2026, 2, 28, 19, 0, 0),
                    EndTime = new DateTime(2026, 2, 28, 21, 0, 0)
                },
                new Showtime
                {
                    ShowtimeId = 9,
                    MovieId = 3,
                    CinemaId = 3,
                    StartTime = new DateTime(2026, 2, 28, 21, 15, 0),
                    EndTime = new DateTime(2026, 2, 28, 23, 15, 0)
                },
                new Showtime
                {
                    ShowtimeId = 10,
                    MovieId = 4,
                    CinemaId = 4,
                    StartTime = new DateTime(2026, 3, 1, 18, 30, 0),
                    EndTime = new DateTime(2026, 3, 1, 20, 50, 0)
                },
                new Showtime
                {
                    ShowtimeId = 11,
                    MovieId = 4,
                    CinemaId = 1,
                    StartTime = new DateTime(2026, 3, 1, 21, 0, 0),
                    EndTime = new DateTime(2026, 3, 1, 23, 20, 0)
                },
                new Showtime
                {
                    ShowtimeId = 12,
                    MovieId = 2,
                    CinemaId = 2,
                    StartTime = new DateTime(2026, 2, 28, 16, 0, 0),
                    EndTime = new DateTime(2026, 2, 28, 17, 40, 0)
                },
                new Showtime
                {
                    ShowtimeId = 13,
                    MovieId = 1,
                    CinemaId = 3,
                    StartTime = new DateTime(2026, 2, 28, 20, 30, 0),
                    EndTime = new DateTime(2026, 2, 28, 22, 30, 0)
                },
                new Showtime
                {
                    ShowtimeId = 14,
                    MovieId = 1,
                    CinemaId = 4,
                    StartTime = new DateTime(2026, 2, 28, 19, 15, 0),
                    EndTime = new DateTime(2026, 2, 28, 21, 15, 0)
                },
                new Showtime
                {
                    ShowtimeId = 15,
                    MovieId = 1,
                    CinemaId = 4,
                    StartTime = new DateTime(2026, 2, 27, 20, 0, 0),
                    EndTime = new DateTime(2026, 2, 27, 22, 0, 0)
                },
                new Showtime
                {
                    ShowtimeId = 16,
                    MovieId = 1,
                    CinemaId = 2,
                    StartTime = new DateTime(2026, 2, 27, 15, 0, 0),
                    EndTime = new DateTime(2026, 2, 27, 17, 0, 0)
                }
            );

            // Seed one test booking (ShowtimeId=1) with 3 seats booked (D4, D5, D6)
            // Note: Seats will be seeded separately in Program.cs
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    UserId = 1, // Reference the seeded user
                    ShowtimeId = 1,
                    TotalPrice = 486000m, // 3 seats × 150,000 × 1.08 VAT
                    CreatedAt = new DateTime(2026, 2, 27, 10, 0, 0)
                }
            );

            // Note: BookingDetails for booking 1 are seeded in Program.cs after seats are created
        }
    }
}