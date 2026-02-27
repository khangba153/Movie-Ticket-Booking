using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieBooking.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    CinemaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.CinemaId);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    PosterUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgeRestriction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Director = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CinemaId = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seats_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "CinemaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Showtimes",
                columns: table => new
                {
                    ShowtimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtimes", x => x.ShowtimeId);
                    table.ForeignKey(
                        name: "FK_Showtimes_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "CinemaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Showtimes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ShowtimeId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Showtimes_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalTable: "Showtimes",
                        principalColumn: "ShowtimeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    BookingDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.BookingDetailId);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "SeatId");
                });

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "CinemaId", "Address", "Name" },
                values: new object[,]
                {
                    { 1, "Quận 1", "CGV Vincom" },
                    { 2, "Quận 7", "Lotte Cinema" },
                    { 3, "Quận 1", "Galaxy Nguyen Du" },
                    { 4, "Quận 10", "BHD Star" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "MovieId", "AgeRestriction", "Cast", "Description", "Director", "DurationMinutes", "Genre", "IsActive", "PosterUrl", "Producer", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, "T13", "Leonardo DiCaprio", "Phim khoa học viễn tưởng về giấc mơ.", "Christopher Nolan", 148, "Sci-Fi, Action", true, "/images/inception.jpg", "Emma Thomas", new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inception" },
                    { 2, "P", "Jack Black, Dustin Hoffman", "Chú gấu trúc Po trở thành chiến binh rồng bảo vệ thung lũng.", "Mark Osborne, John Stevenson", 92, "Animation, Comedy", true, "/images/kungfu.jpg", "Melissa Cobb", new DateTime(2008, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kung Fu Panda" },
                    { 3, "T13", "Robert Downey Jr., Gwyneth Paltrow", "Tony Stark chế tạo bộ giáp công nghệ cao và trở thành Iron Man.", "Jon Favreau", 126, "Action, Superhero", true, "/images/ironman.jpg", "Kevin Feige", new DateTime(2008, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iron Man" },
                    { 4, "T13", "Johnny Depp, Orlando Bloom", "Thuyền trưởng Jack Sparrow phiêu lưu trên biển Caribbean.", "Gore Verbinski", 143, "Adventure, Fantasy", true, "/images/pirates.jpg", "Jerry Bruckheimer", new DateTime(2003, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pirates of the Caribbean" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[] { 1, "test@example.com", "Test User", "dummy_hash" });

            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "ShowtimeId", "CinemaId", "EndTime", "MovieId", "StartTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 27, 21, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 27, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2026, 2, 27, 23, 30, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 27, 21, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2026, 2, 27, 16, 30, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 27, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 2, new DateTime(2026, 2, 27, 19, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 27, 17, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 3, new DateTime(2026, 2, 28, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 28, 18, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, new DateTime(2026, 2, 27, 18, 40, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 2, 27, 17, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 4, new DateTime(2026, 2, 28, 22, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 2, 28, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2, new DateTime(2026, 2, 28, 21, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 2, 28, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 3, new DateTime(2026, 2, 28, 23, 15, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 2, 28, 21, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 4, new DateTime(2026, 3, 1, 20, 50, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2026, 3, 1, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 1, new DateTime(2026, 3, 1, 23, 20, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2026, 3, 1, 21, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 2, new DateTime(2026, 2, 28, 17, 40, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 2, 28, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 3, new DateTime(2026, 2, 28, 22, 30, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 28, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 4, new DateTime(2026, 2, 28, 21, 15, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 28, 19, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 4, new DateTime(2026, 2, 27, 22, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 27, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 2, new DateTime(2026, 2, 27, 17, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 27, 15, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CreatedAt", "ShowtimeId", "TotalPrice", "UserId" },
                values: new object[] { 1, new DateTime(2026, 2, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, 450000m, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingId",
                table: "BookingDetails",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_SeatId",
                table: "BookingDetails",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShowtimeId",
                table: "Bookings",
                column: "ShowtimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CinemaId",
                table: "Seats",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_CinemaId",
                table: "Showtimes",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_MovieId",
                table: "Showtimes",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Showtimes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cinemas");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
