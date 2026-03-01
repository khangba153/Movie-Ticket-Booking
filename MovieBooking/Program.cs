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

// Thêm headers để cho phép script chạy
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com; style-src 'self' 'unsafe-inline'; img-src 'self' data:;");
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