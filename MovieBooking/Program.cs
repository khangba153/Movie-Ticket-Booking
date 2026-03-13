using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Data;
using MovieBooking.Services;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Đăng ký Services
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ICinemaService, CinemaService>();
builder.Services.AddScoped<IShowtimeService, ShowtimeService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Đăng ký Admin Services
builder.Services.AddScoped<IAdminMovieService, AdminMovieService>();
builder.Services.AddScoped<IAdminCinemaService, AdminCinemaService>();
builder.Services.AddScoped<IAdminShowtimeService, AdminShowtimeService>();
builder.Services.AddScoped<IAdminBookingService, AdminBookingService>();

// Đăng ký Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Swagger (giữ lại nếu muốn)
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

// Thêm headers để cho phép script chạy (chỉ cho HTML responses)
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        var contentType = context.Response.ContentType ?? "";
        if (contentType.Contains("text/html", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Headers.Append("Content-Security-Policy",
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: blob:; " +
                "connect-src 'self'; " +
                "font-src 'self'; " +
                "worker-src 'self' blob:;");
        }
        return Task.CompletedTask;
    });
    await next();
});

// Cấu hình default file là home.html
var defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("auth.html");
app.UseDefaultFiles(defaultFilesOptions);

// Kích hoạt Static Files (phải trước routing)
app.UseStaticFiles();

// Kích hoạt Controller routes
app.MapControllers();

// Migrate and seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Ensure database is created and migrations are applied
    await dbContext.Database.MigrateAsync();
    // Then seed data
    await SeedSeatsAsync(dbContext);
}

async Task SeedSeatsAsync(ApplicationDbContext dbContext)
{
    try
    {
        // Only seed if no seats exist
        if (!dbContext.Seats.Any())
        {
            var seats = new List<MovieBooking.Models.Seat>();
            
            // Create 54 seats per cinema (6 rows x 9 cols)
            for (int cinemaId = 1; cinemaId <= 4; cinemaId++)
            {
                char[] rows = { 'A', 'B', 'C', 'D', 'E', 'F' };
                foreach (char row in rows)
                {
                    for (int num = 1; num <= 9; num++)
                    {
                        seats.Add(new MovieBooking.Models.Seat
                        {
                            // Don't set SeatId - let SQL Server auto-generate
                            CinemaId = cinemaId,
                            Row = row.ToString(),
                            Number = num
                        });
                    }
                }
            }
            
            dbContext.Seats.AddRange(seats);
            await dbContext.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {seats.Count} seats");
        }

        // Seed sample booking details if not exist
        if (!dbContext.Users.Any())
        {
            // Create test user
            var user = new MovieBooking.Models.User
            {
                Username = "testuser",
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = "dummy_hash"
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded test user with Id={user.Id}");
        }

        // Seed admin user if not exist
        if (!dbContext.Users.Any(u => u.Role == "Admin"))
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(salt.Concat(System.Text.Encoding.UTF8.GetBytes("admin123")).ToArray());
            string passwordHash = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);

            var admin = new MovieBooking.Models.User
            {
                Username = "admin",
                Name = "Administrator",
                Email = "admin@foxcinema.com",
                PasswordHash = passwordHash,
                Role = "Admin"
            };
            dbContext.Users.Add(admin);
            await dbContext.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded admin user (admin/admin123) with Id={admin.Id}");
        }

        // Note: Bookings will be created through the UI/API

        // Auto-seed demo showtimes relative to today
        await SeedDynamicShowtimesAsync(dbContext);

        // Seed BookingDetails for the seeded booking (D4, D5, D6 in Cinema 1)
        if (!dbContext.BookingDetails.Any())
        {
            // Cinema 1 seats: Row A=1-9, B=10-18, C=19-27, D=28-36
            // D4=31, D5=32, D6=33
            var d4 = dbContext.Seats.FirstOrDefault(s => s.CinemaId == 1 && s.Row == "D" && s.Number == 4);
            var d5 = dbContext.Seats.FirstOrDefault(s => s.CinemaId == 1 && s.Row == "D" && s.Number == 5);
            var d6 = dbContext.Seats.FirstOrDefault(s => s.CinemaId == 1 && s.Row == "D" && s.Number == 6);

            if (d4 != null && d5 != null && d6 != null)
            {
                dbContext.BookingDetails.AddRange(
                    new MovieBooking.Models.BookingDetail { BookingId = 1, SeatId = d4.SeatId },
                    new MovieBooking.Models.BookingDetail { BookingId = 1, SeatId = d5.SeatId },
                    new MovieBooking.Models.BookingDetail { BookingId = 1, SeatId = d6.SeatId }
                );
                await dbContext.SaveChangesAsync();
                Console.WriteLine("✅ Seeded BookingDetails for booking 1 (D4, D5, D6)");
            }
        }

        // Example code for seeding booking details (commented out)
        // To enable, uncomment and ensure valid Showtime and Seat IDs exist
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error seeding data: {ex.Message}");
    }
}

app.Run();

// Auto-generate demo showtimes so there are always future showtimes available
async Task SeedDynamicShowtimesAsync(ApplicationDbContext dbContext)
{
    try
    {
        var today = DateTime.Today;
        // Check the latest showtime in DB
        var latestShowtime = await dbContext.Showtimes.MaxAsync(s => (DateTime?)s.StartTime);
        var lastCoveredDate = latestShowtime?.Date ?? today.AddDays(-1);

        // Always ensure showtimes exist for the next 7 days from today
        var targetEndDate = today.AddDays(7);

        if (lastCoveredDate >= targetEndDate)
        {
            return; // Already have enough future showtimes
        }

        // Start generating from the day after the last covered date (or today)
        var startDate = lastCoveredDate < today ? today : lastCoveredDate.AddDays(1);

        int[] movieIds = { 1, 2, 3, 4 };
        int[] movieDurations = { 148, 92, 126, 143 }; // minutes
        int[] cinemaIds = { 1, 2, 3, 4 };

        // Time slots: morning, afternoon, evening (+ late night on weekends)
        var weekdaySlots = new[] { (10, 0), (14, 30), (19, 0) };
        var weekendSlots = new[] { (10, 0), (14, 0), (18, 0), (21, 0) };

        // Prices: weekday vs weekend
        decimal[] weekdayPrices = { 90000m, 120000m, 150000m };
        decimal[] weekendPrices = { 100000m, 130000m, 160000m, 180000m };

        // Get next available ShowtimeId
        var newShowtimes = new List<MovieBooking.Models.Showtime>();

        for (var date = startDate; date <= targetEndDate; date = date.AddDays(1))
        {
            bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
            var slots = isWeekend ? weekendSlots : weekdaySlots;
            var prices = isWeekend ? weekendPrices : weekdayPrices;

            for (int m = 0; m < movieIds.Length; m++)
            {
                int movieId = movieIds[m];
                int duration = movieDurations[m];

                for (int s = 0; s < slots.Length; s++)
                {
                    // Rotate cinema: each movie+slot combo gets a different cinema
                    int cinemaId = cinemaIds[(m + s + (int)(date - today).TotalDays) % cinemaIds.Length];
                    var startTime = date.AddHours(slots[s].Item1).AddMinutes(slots[s].Item2);
                    var endTime = startTime.AddMinutes(duration);

                    newShowtimes.Add(new MovieBooking.Models.Showtime
                    {
                        MovieId = movieId,
                        CinemaId = cinemaId,
                        StartTime = startTime,
                        EndTime = endTime,
                        Price = prices[s]
                    });
                }
            }
        }

        if (newShowtimes.Count > 0)
        {
            dbContext.Showtimes.AddRange(newShowtimes);
            await dbContext.SaveChangesAsync();
            Console.WriteLine($"✅ Auto-seeded {newShowtimes.Count} showtimes ({startDate:dd/MM} → {targetEndDate:dd/MM})");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Dynamic showtime seeding skipped: {ex.Message}");
    }
}