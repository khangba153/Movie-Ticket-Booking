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

            // Configure decimal precision for Showtime Price
            modelBuilder.Entity<Showtime>()
                .Property(s => s.Price)
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

            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "testuser",
                    Name = "Test User",
                    Email = "test@example.com",
                    PasswordHash = "dummy_hash",
                    Role = "User"
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

            // Seed Showtimes
            modelBuilder.Entity<Showtime>().HasData(
                new Showtime { ShowtimeId = 1, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 2, 27, 19, 0, 0), EndTime = new DateTime(2026, 2, 27, 21, 0, 0), Price = 150000m },
                new Showtime { ShowtimeId = 2, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 2, 27, 21, 30, 0), EndTime = new DateTime(2026, 2, 27, 23, 30, 0), Price = 150000m },
                new Showtime { ShowtimeId = 3, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 2, 27, 14, 30, 0), EndTime = new DateTime(2026, 2, 27, 16, 30, 0), Price = 120000m },
                new Showtime { ShowtimeId = 4, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 2, 27, 17, 0, 0), EndTime = new DateTime(2026, 2, 27, 19, 0, 0), Price = 150000m },
                new Showtime { ShowtimeId = 5, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 2, 28, 18, 0, 0), EndTime = new DateTime(2026, 2, 28, 20, 0, 0), Price = 150000m },
                new Showtime { ShowtimeId = 6, MovieId = 2, CinemaId = 1, StartTime = new DateTime(2026, 2, 27, 17, 0, 0), EndTime = new DateTime(2026, 2, 27, 18, 40, 0), Price = 120000m },
                new Showtime { ShowtimeId = 7, MovieId = 2, CinemaId = 4, StartTime = new DateTime(2026, 2, 28, 20, 0, 0), EndTime = new DateTime(2026, 2, 28, 22, 0, 0), Price = 150000m },
                new Showtime { ShowtimeId = 8, MovieId = 3, CinemaId = 2, StartTime = new DateTime(2026, 2, 28, 19, 0, 0), EndTime = new DateTime(2026, 2, 28, 21, 0, 0), Price = 150000m },
                new Showtime { ShowtimeId = 9, MovieId = 3, CinemaId = 3, StartTime = new DateTime(2026, 2, 28, 21, 15, 0), EndTime = new DateTime(2026, 2, 28, 23, 15, 0), Price = 180000m },
                new Showtime { ShowtimeId = 10, MovieId = 4, CinemaId = 4, StartTime = new DateTime(2026, 3, 1, 18, 30, 0), EndTime = new DateTime(2026, 3, 1, 20, 50, 0), Price = 150000m },
                new Showtime { ShowtimeId = 11, MovieId = 4, CinemaId = 1, StartTime = new DateTime(2026, 3, 1, 21, 0, 0), EndTime = new DateTime(2026, 3, 1, 23, 20, 0), Price = 180000m },
                new Showtime { ShowtimeId = 12, MovieId = 2, CinemaId = 2, StartTime = new DateTime(2026, 2, 28, 16, 0, 0), EndTime = new DateTime(2026, 2, 28, 17, 40, 0), Price = 120000m },
                new Showtime { ShowtimeId = 13, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 2, 28, 20, 30, 0), EndTime = new DateTime(2026, 2, 28, 22, 30, 0), Price = 150000m },
                new Showtime { ShowtimeId = 14, MovieId = 1, CinemaId = 4, StartTime = new DateTime(2026, 2, 28, 19, 15, 0), EndTime = new DateTime(2026, 2, 28, 21, 15, 0), Price = 150000m },
                new Showtime { ShowtimeId = 15, MovieId = 1, CinemaId = 4, StartTime = new DateTime(2026, 2, 27, 20, 0, 0), EndTime = new DateTime(2026, 2, 27, 22, 0, 0), Price = 180000m },
                new Showtime { ShowtimeId = 16, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 2, 27, 15, 0, 0), EndTime = new DateTime(2026, 2, 27, 17, 0, 0), Price = 120000m },

                // ===== March 1 (Sunday - Weekend) =====
                // Inception (148 min)
                new Showtime { ShowtimeId = 17, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 3, 1, 10, 0, 0), EndTime = new DateTime(2026, 3, 1, 12, 28, 0), Price = 100000m },
                new Showtime { ShowtimeId = 18, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 3, 1, 14, 30, 0), EndTime = new DateTime(2026, 3, 1, 16, 58, 0), Price = 130000m },
                new Showtime { ShowtimeId = 19, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 3, 1, 19, 0, 0), EndTime = new DateTime(2026, 3, 1, 21, 28, 0), Price = 160000m },
                // Kung Fu Panda (92 min)
                new Showtime { ShowtimeId = 20, MovieId = 2, CinemaId = 2, StartTime = new DateTime(2026, 3, 1, 10, 0, 0), EndTime = new DateTime(2026, 3, 1, 11, 32, 0), Price = 100000m },
                new Showtime { ShowtimeId = 21, MovieId = 2, CinemaId = 1, StartTime = new DateTime(2026, 3, 1, 14, 0, 0), EndTime = new DateTime(2026, 3, 1, 15, 32, 0), Price = 130000m },
                new Showtime { ShowtimeId = 22, MovieId = 2, CinemaId = 3, StartTime = new DateTime(2026, 3, 1, 18, 0, 0), EndTime = new DateTime(2026, 3, 1, 19, 32, 0), Price = 160000m },
                // Iron Man (126 min)
                new Showtime { ShowtimeId = 23, MovieId = 3, CinemaId = 3, StartTime = new DateTime(2026, 3, 1, 10, 30, 0), EndTime = new DateTime(2026, 3, 1, 12, 36, 0), Price = 100000m },
                new Showtime { ShowtimeId = 24, MovieId = 3, CinemaId = 4, StartTime = new DateTime(2026, 3, 1, 15, 0, 0), EndTime = new DateTime(2026, 3, 1, 17, 6, 0), Price = 130000m },
                new Showtime { ShowtimeId = 25, MovieId = 3, CinemaId = 1, StartTime = new DateTime(2026, 3, 1, 20, 0, 0), EndTime = new DateTime(2026, 3, 1, 22, 6, 0), Price = 160000m },
                // Pirates of the Caribbean (143 min)
                new Showtime { ShowtimeId = 26, MovieId = 4, CinemaId = 2, StartTime = new DateTime(2026, 3, 1, 14, 30, 0), EndTime = new DateTime(2026, 3, 1, 16, 53, 0), Price = 130000m },
                new Showtime { ShowtimeId = 27, MovieId = 4, CinemaId = 3, StartTime = new DateTime(2026, 3, 1, 20, 30, 0), EndTime = new DateTime(2026, 3, 1, 22, 53, 0), Price = 180000m },

                // ===== March 2 (Monday) =====
                // Inception
                new Showtime { ShowtimeId = 28, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 3, 2, 10, 0, 0), EndTime = new DateTime(2026, 3, 2, 12, 28, 0), Price = 90000m },
                new Showtime { ShowtimeId = 29, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 3, 2, 14, 0, 0), EndTime = new DateTime(2026, 3, 2, 16, 28, 0), Price = 120000m },
                new Showtime { ShowtimeId = 30, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 3, 2, 19, 0, 0), EndTime = new DateTime(2026, 3, 2, 21, 28, 0), Price = 150000m },
                // Kung Fu Panda
                new Showtime { ShowtimeId = 31, MovieId = 2, CinemaId = 1, StartTime = new DateTime(2026, 3, 2, 10, 0, 0), EndTime = new DateTime(2026, 3, 2, 11, 32, 0), Price = 90000m },
                new Showtime { ShowtimeId = 32, MovieId = 2, CinemaId = 3, StartTime = new DateTime(2026, 3, 2, 15, 0, 0), EndTime = new DateTime(2026, 3, 2, 16, 32, 0), Price = 120000m },
                new Showtime { ShowtimeId = 33, MovieId = 2, CinemaId = 4, StartTime = new DateTime(2026, 3, 2, 19, 0, 0), EndTime = new DateTime(2026, 3, 2, 20, 32, 0), Price = 150000m },
                // Iron Man
                new Showtime { ShowtimeId = 34, MovieId = 3, CinemaId = 4, StartTime = new DateTime(2026, 3, 2, 10, 30, 0), EndTime = new DateTime(2026, 3, 2, 12, 36, 0), Price = 90000m },
                new Showtime { ShowtimeId = 35, MovieId = 3, CinemaId = 2, StartTime = new DateTime(2026, 3, 2, 14, 30, 0), EndTime = new DateTime(2026, 3, 2, 16, 36, 0), Price = 120000m },
                new Showtime { ShowtimeId = 36, MovieId = 3, CinemaId = 1, StartTime = new DateTime(2026, 3, 2, 19, 30, 0), EndTime = new DateTime(2026, 3, 2, 21, 36, 0), Price = 150000m },
                // Pirates
                new Showtime { ShowtimeId = 37, MovieId = 4, CinemaId = 3, StartTime = new DateTime(2026, 3, 2, 10, 0, 0), EndTime = new DateTime(2026, 3, 2, 12, 23, 0), Price = 90000m },
                new Showtime { ShowtimeId = 38, MovieId = 4, CinemaId = 4, StartTime = new DateTime(2026, 3, 2, 14, 0, 0), EndTime = new DateTime(2026, 3, 2, 16, 23, 0), Price = 120000m },
                new Showtime { ShowtimeId = 39, MovieId = 4, CinemaId = 2, StartTime = new DateTime(2026, 3, 2, 19, 0, 0), EndTime = new DateTime(2026, 3, 2, 21, 23, 0), Price = 150000m },

                // ===== March 3 (Tuesday) =====
                // Inception
                new Showtime { ShowtimeId = 40, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 3, 3, 10, 0, 0), EndTime = new DateTime(2026, 3, 3, 12, 28, 0), Price = 90000m },
                new Showtime { ShowtimeId = 41, MovieId = 1, CinemaId = 4, StartTime = new DateTime(2026, 3, 3, 14, 30, 0), EndTime = new DateTime(2026, 3, 3, 16, 58, 0), Price = 120000m },
                new Showtime { ShowtimeId = 42, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 3, 3, 19, 30, 0), EndTime = new DateTime(2026, 3, 3, 21, 58, 0), Price = 150000m },
                // Kung Fu Panda
                new Showtime { ShowtimeId = 43, MovieId = 2, CinemaId = 4, StartTime = new DateTime(2026, 3, 3, 10, 0, 0), EndTime = new DateTime(2026, 3, 3, 11, 32, 0), Price = 90000m },
                new Showtime { ShowtimeId = 44, MovieId = 2, CinemaId = 1, StartTime = new DateTime(2026, 3, 3, 14, 30, 0), EndTime = new DateTime(2026, 3, 3, 16, 2, 0), Price = 120000m },
                new Showtime { ShowtimeId = 45, MovieId = 2, CinemaId = 2, StartTime = new DateTime(2026, 3, 3, 18, 30, 0), EndTime = new DateTime(2026, 3, 3, 20, 2, 0), Price = 150000m },
                // Iron Man
                new Showtime { ShowtimeId = 46, MovieId = 3, CinemaId = 1, StartTime = new DateTime(2026, 3, 3, 10, 30, 0), EndTime = new DateTime(2026, 3, 3, 12, 36, 0), Price = 90000m },
                new Showtime { ShowtimeId = 47, MovieId = 3, CinemaId = 3, StartTime = new DateTime(2026, 3, 3, 15, 0, 0), EndTime = new DateTime(2026, 3, 3, 17, 6, 0), Price = 120000m },
                new Showtime { ShowtimeId = 48, MovieId = 3, CinemaId = 4, StartTime = new DateTime(2026, 3, 3, 20, 0, 0), EndTime = new DateTime(2026, 3, 3, 22, 6, 0), Price = 150000m },
                // Pirates
                new Showtime { ShowtimeId = 49, MovieId = 4, CinemaId = 2, StartTime = new DateTime(2026, 3, 3, 10, 0, 0), EndTime = new DateTime(2026, 3, 3, 12, 23, 0), Price = 90000m },
                new Showtime { ShowtimeId = 50, MovieId = 4, CinemaId = 1, StartTime = new DateTime(2026, 3, 3, 14, 0, 0), EndTime = new DateTime(2026, 3, 3, 16, 23, 0), Price = 120000m },
                new Showtime { ShowtimeId = 51, MovieId = 4, CinemaId = 3, StartTime = new DateTime(2026, 3, 3, 19, 0, 0), EndTime = new DateTime(2026, 3, 3, 21, 23, 0), Price = 150000m },

                // ===== March 4 (Wednesday) =====
                // Inception
                new Showtime { ShowtimeId = 52, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 3, 4, 10, 30, 0), EndTime = new DateTime(2026, 3, 4, 12, 58, 0), Price = 90000m },
                new Showtime { ShowtimeId = 53, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 3, 4, 15, 0, 0), EndTime = new DateTime(2026, 3, 4, 17, 28, 0), Price = 120000m },
                new Showtime { ShowtimeId = 54, MovieId = 1, CinemaId = 4, StartTime = new DateTime(2026, 3, 4, 20, 0, 0), EndTime = new DateTime(2026, 3, 4, 22, 28, 0), Price = 150000m },
                // Kung Fu Panda
                new Showtime { ShowtimeId = 55, MovieId = 2, CinemaId = 3, StartTime = new DateTime(2026, 3, 4, 10, 0, 0), EndTime = new DateTime(2026, 3, 4, 11, 32, 0), Price = 90000m },
                new Showtime { ShowtimeId = 56, MovieId = 2, CinemaId = 2, StartTime = new DateTime(2026, 3, 4, 14, 0, 0), EndTime = new DateTime(2026, 3, 4, 15, 32, 0), Price = 120000m },
                new Showtime { ShowtimeId = 57, MovieId = 2, CinemaId = 1, StartTime = new DateTime(2026, 3, 4, 18, 0, 0), EndTime = new DateTime(2026, 3, 4, 19, 32, 0), Price = 150000m },
                // Iron Man
                new Showtime { ShowtimeId = 58, MovieId = 3, CinemaId = 2, StartTime = new DateTime(2026, 3, 4, 10, 30, 0), EndTime = new DateTime(2026, 3, 4, 12, 36, 0), Price = 90000m },
                new Showtime { ShowtimeId = 59, MovieId = 3, CinemaId = 1, StartTime = new DateTime(2026, 3, 4, 15, 0, 0), EndTime = new DateTime(2026, 3, 4, 17, 6, 0), Price = 120000m },
                new Showtime { ShowtimeId = 60, MovieId = 3, CinemaId = 3, StartTime = new DateTime(2026, 3, 4, 19, 30, 0), EndTime = new DateTime(2026, 3, 4, 21, 36, 0), Price = 150000m },
                // Pirates
                new Showtime { ShowtimeId = 61, MovieId = 4, CinemaId = 4, StartTime = new DateTime(2026, 3, 4, 10, 0, 0), EndTime = new DateTime(2026, 3, 4, 12, 23, 0), Price = 90000m },
                new Showtime { ShowtimeId = 62, MovieId = 4, CinemaId = 3, StartTime = new DateTime(2026, 3, 4, 14, 30, 0), EndTime = new DateTime(2026, 3, 4, 16, 53, 0), Price = 120000m },
                new Showtime { ShowtimeId = 63, MovieId = 4, CinemaId = 1, StartTime = new DateTime(2026, 3, 4, 19, 0, 0), EndTime = new DateTime(2026, 3, 4, 21, 23, 0), Price = 150000m },

                // ===== March 5 (Thursday) =====
                // Inception
                new Showtime { ShowtimeId = 64, MovieId = 1, CinemaId = 4, StartTime = new DateTime(2026, 3, 5, 10, 0, 0), EndTime = new DateTime(2026, 3, 5, 12, 28, 0), Price = 90000m },
                new Showtime { ShowtimeId = 65, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 3, 5, 14, 30, 0), EndTime = new DateTime(2026, 3, 5, 16, 58, 0), Price = 120000m },
                new Showtime { ShowtimeId = 66, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 3, 5, 19, 0, 0), EndTime = new DateTime(2026, 3, 5, 21, 28, 0), Price = 150000m },
                // Kung Fu Panda
                new Showtime { ShowtimeId = 67, MovieId = 2, CinemaId = 2, StartTime = new DateTime(2026, 3, 5, 10, 30, 0), EndTime = new DateTime(2026, 3, 5, 12, 2, 0), Price = 90000m },
                new Showtime { ShowtimeId = 68, MovieId = 2, CinemaId = 4, StartTime = new DateTime(2026, 3, 5, 14, 0, 0), EndTime = new DateTime(2026, 3, 5, 15, 32, 0), Price = 120000m },
                new Showtime { ShowtimeId = 69, MovieId = 2, CinemaId = 3, StartTime = new DateTime(2026, 3, 5, 19, 0, 0), EndTime = new DateTime(2026, 3, 5, 20, 32, 0), Price = 150000m },
                // Iron Man
                new Showtime { ShowtimeId = 70, MovieId = 3, CinemaId = 3, StartTime = new DateTime(2026, 3, 5, 10, 0, 0), EndTime = new DateTime(2026, 3, 5, 12, 6, 0), Price = 90000m },
                new Showtime { ShowtimeId = 71, MovieId = 3, CinemaId = 1, StartTime = new DateTime(2026, 3, 5, 14, 30, 0), EndTime = new DateTime(2026, 3, 5, 16, 36, 0), Price = 120000m },
                new Showtime { ShowtimeId = 72, MovieId = 3, CinemaId = 2, StartTime = new DateTime(2026, 3, 5, 20, 0, 0), EndTime = new DateTime(2026, 3, 5, 22, 6, 0), Price = 150000m },
                // Pirates
                new Showtime { ShowtimeId = 73, MovieId = 4, CinemaId = 1, StartTime = new DateTime(2026, 3, 5, 10, 0, 0), EndTime = new DateTime(2026, 3, 5, 12, 23, 0), Price = 90000m },
                new Showtime { ShowtimeId = 74, MovieId = 4, CinemaId = 2, StartTime = new DateTime(2026, 3, 5, 15, 0, 0), EndTime = new DateTime(2026, 3, 5, 17, 23, 0), Price = 120000m },
                new Showtime { ShowtimeId = 75, MovieId = 4, CinemaId = 4, StartTime = new DateTime(2026, 3, 5, 19, 30, 0), EndTime = new DateTime(2026, 3, 5, 21, 53, 0), Price = 150000m },

                // ===== March 6 (Friday) =====
                // Inception
                new Showtime { ShowtimeId = 76, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 3, 6, 10, 0, 0), EndTime = new DateTime(2026, 3, 6, 12, 28, 0), Price = 90000m },
                new Showtime { ShowtimeId = 77, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 3, 6, 15, 0, 0), EndTime = new DateTime(2026, 3, 6, 17, 28, 0), Price = 120000m },
                new Showtime { ShowtimeId = 78, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 3, 6, 19, 30, 0), EndTime = new DateTime(2026, 3, 6, 21, 58, 0), Price = 150000m },
                // Kung Fu Panda
                new Showtime { ShowtimeId = 79, MovieId = 2, CinemaId = 1, StartTime = new DateTime(2026, 3, 6, 10, 0, 0), EndTime = new DateTime(2026, 3, 6, 11, 32, 0), Price = 90000m },
                new Showtime { ShowtimeId = 80, MovieId = 2, CinemaId = 3, StartTime = new DateTime(2026, 3, 6, 15, 30, 0), EndTime = new DateTime(2026, 3, 6, 17, 2, 0), Price = 120000m },
                new Showtime { ShowtimeId = 81, MovieId = 2, CinemaId = 4, StartTime = new DateTime(2026, 3, 6, 19, 0, 0), EndTime = new DateTime(2026, 3, 6, 20, 32, 0), Price = 150000m },
                // Iron Man
                new Showtime { ShowtimeId = 82, MovieId = 3, CinemaId = 4, StartTime = new DateTime(2026, 3, 6, 10, 30, 0), EndTime = new DateTime(2026, 3, 6, 12, 36, 0), Price = 90000m },
                new Showtime { ShowtimeId = 83, MovieId = 3, CinemaId = 2, StartTime = new DateTime(2026, 3, 6, 14, 30, 0), EndTime = new DateTime(2026, 3, 6, 16, 36, 0), Price = 120000m },
                new Showtime { ShowtimeId = 84, MovieId = 3, CinemaId = 1, StartTime = new DateTime(2026, 3, 6, 20, 0, 0), EndTime = new DateTime(2026, 3, 6, 22, 6, 0), Price = 150000m },
                // Pirates
                new Showtime { ShowtimeId = 85, MovieId = 4, CinemaId = 3, StartTime = new DateTime(2026, 3, 6, 10, 0, 0), EndTime = new DateTime(2026, 3, 6, 12, 23, 0), Price = 90000m },
                new Showtime { ShowtimeId = 86, MovieId = 4, CinemaId = 4, StartTime = new DateTime(2026, 3, 6, 14, 0, 0), EndTime = new DateTime(2026, 3, 6, 16, 23, 0), Price = 120000m },
                new Showtime { ShowtimeId = 87, MovieId = 4, CinemaId = 2, StartTime = new DateTime(2026, 3, 6, 19, 0, 0), EndTime = new DateTime(2026, 3, 6, 21, 23, 0), Price = 150000m },

                // ===== March 7 (Saturday - Weekend) =====
                // Inception
                new Showtime { ShowtimeId = 88, MovieId = 1, CinemaId = 1, StartTime = new DateTime(2026, 3, 7, 10, 0, 0), EndTime = new DateTime(2026, 3, 7, 12, 28, 0), Price = 100000m },
                new Showtime { ShowtimeId = 89, MovieId = 1, CinemaId = 2, StartTime = new DateTime(2026, 3, 7, 14, 0, 0), EndTime = new DateTime(2026, 3, 7, 16, 28, 0), Price = 130000m },
                new Showtime { ShowtimeId = 90, MovieId = 1, CinemaId = 4, StartTime = new DateTime(2026, 3, 7, 19, 0, 0), EndTime = new DateTime(2026, 3, 7, 21, 28, 0), Price = 160000m },
                new Showtime { ShowtimeId = 91, MovieId = 1, CinemaId = 3, StartTime = new DateTime(2026, 3, 7, 21, 30, 0), EndTime = new DateTime(2026, 3, 7, 23, 58, 0), Price = 180000m },
                // Kung Fu Panda
                new Showtime { ShowtimeId = 92, MovieId = 2, CinemaId = 3, StartTime = new DateTime(2026, 3, 7, 10, 0, 0), EndTime = new DateTime(2026, 3, 7, 11, 32, 0), Price = 100000m },
                new Showtime { ShowtimeId = 93, MovieId = 2, CinemaId = 4, StartTime = new DateTime(2026, 3, 7, 14, 30, 0), EndTime = new DateTime(2026, 3, 7, 16, 2, 0), Price = 130000m },
                new Showtime { ShowtimeId = 94, MovieId = 2, CinemaId = 1, StartTime = new DateTime(2026, 3, 7, 18, 0, 0), EndTime = new DateTime(2026, 3, 7, 19, 32, 0), Price = 160000m },
                new Showtime { ShowtimeId = 95, MovieId = 2, CinemaId = 2, StartTime = new DateTime(2026, 3, 7, 20, 0, 0), EndTime = new DateTime(2026, 3, 7, 21, 32, 0), Price = 180000m },
                // Iron Man
                new Showtime { ShowtimeId = 96, MovieId = 3, CinemaId = 2, StartTime = new DateTime(2026, 3, 7, 10, 0, 0), EndTime = new DateTime(2026, 3, 7, 12, 6, 0), Price = 100000m },
                new Showtime { ShowtimeId = 97, MovieId = 3, CinemaId = 3, StartTime = new DateTime(2026, 3, 7, 14, 0, 0), EndTime = new DateTime(2026, 3, 7, 16, 6, 0), Price = 130000m },
                new Showtime { ShowtimeId = 98, MovieId = 3, CinemaId = 4, StartTime = new DateTime(2026, 3, 7, 19, 0, 0), EndTime = new DateTime(2026, 3, 7, 21, 6, 0), Price = 160000m },
                new Showtime { ShowtimeId = 99, MovieId = 3, CinemaId = 1, StartTime = new DateTime(2026, 3, 7, 21, 30, 0), EndTime = new DateTime(2026, 3, 7, 23, 36, 0), Price = 180000m },
                // Pirates
                new Showtime { ShowtimeId = 100, MovieId = 4, CinemaId = 4, StartTime = new DateTime(2026, 3, 7, 10, 0, 0), EndTime = new DateTime(2026, 3, 7, 12, 23, 0), Price = 100000m },
                new Showtime { ShowtimeId = 101, MovieId = 4, CinemaId = 1, StartTime = new DateTime(2026, 3, 7, 14, 30, 0), EndTime = new DateTime(2026, 3, 7, 16, 53, 0), Price = 130000m },
                new Showtime { ShowtimeId = 102, MovieId = 4, CinemaId = 2, StartTime = new DateTime(2026, 3, 7, 18, 30, 0), EndTime = new DateTime(2026, 3, 7, 20, 53, 0), Price = 160000m },
                new Showtime { ShowtimeId = 103, MovieId = 4, CinemaId = 3, StartTime = new DateTime(2026, 3, 7, 21, 0, 0), EndTime = new DateTime(2026, 3, 7, 23, 23, 0), Price = 180000m }
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
