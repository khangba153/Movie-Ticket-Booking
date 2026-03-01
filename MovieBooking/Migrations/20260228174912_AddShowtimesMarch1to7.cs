using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddShowtimesMarch1to7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "ShowtimeId", "CinemaId", "EndTime", "MovieId", "Price", "StartTime" },
                values: new object[,]
                {
                    { 17, 1, new DateTime(2026, 3, 1, 12, 28, 0, 0, DateTimeKind.Unspecified), 1, 100000m, new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 3, new DateTime(2026, 3, 1, 16, 58, 0, 0, DateTimeKind.Unspecified), 1, 130000m, new DateTime(2026, 3, 1, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 2, new DateTime(2026, 3, 1, 21, 28, 0, 0, DateTimeKind.Unspecified), 1, 160000m, new DateTime(2026, 3, 1, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 2, new DateTime(2026, 3, 1, 11, 32, 0, 0, DateTimeKind.Unspecified), 2, 100000m, new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 1, new DateTime(2026, 3, 1, 15, 32, 0, 0, DateTimeKind.Unspecified), 2, 130000m, new DateTime(2026, 3, 1, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 3, new DateTime(2026, 3, 1, 19, 32, 0, 0, DateTimeKind.Unspecified), 2, 160000m, new DateTime(2026, 3, 1, 18, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 3, new DateTime(2026, 3, 1, 12, 36, 0, 0, DateTimeKind.Unspecified), 3, 100000m, new DateTime(2026, 3, 1, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 4, new DateTime(2026, 3, 1, 17, 6, 0, 0, DateTimeKind.Unspecified), 3, 130000m, new DateTime(2026, 3, 1, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 1, new DateTime(2026, 3, 1, 22, 6, 0, 0, DateTimeKind.Unspecified), 3, 160000m, new DateTime(2026, 3, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 2, new DateTime(2026, 3, 1, 16, 53, 0, 0, DateTimeKind.Unspecified), 4, 130000m, new DateTime(2026, 3, 1, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 3, new DateTime(2026, 3, 1, 22, 53, 0, 0, DateTimeKind.Unspecified), 4, 180000m, new DateTime(2026, 3, 1, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 2, new DateTime(2026, 3, 2, 12, 28, 0, 0, DateTimeKind.Unspecified), 1, 90000m, new DateTime(2026, 3, 2, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, 1, new DateTime(2026, 3, 2, 16, 28, 0, 0, DateTimeKind.Unspecified), 1, 120000m, new DateTime(2026, 3, 2, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 3, new DateTime(2026, 3, 2, 21, 28, 0, 0, DateTimeKind.Unspecified), 1, 150000m, new DateTime(2026, 3, 2, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 1, new DateTime(2026, 3, 2, 11, 32, 0, 0, DateTimeKind.Unspecified), 2, 90000m, new DateTime(2026, 3, 2, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 3, new DateTime(2026, 3, 2, 16, 32, 0, 0, DateTimeKind.Unspecified), 2, 120000m, new DateTime(2026, 3, 2, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 4, new DateTime(2026, 3, 2, 20, 32, 0, 0, DateTimeKind.Unspecified), 2, 150000m, new DateTime(2026, 3, 2, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 4, new DateTime(2026, 3, 2, 12, 36, 0, 0, DateTimeKind.Unspecified), 3, 90000m, new DateTime(2026, 3, 2, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 2, new DateTime(2026, 3, 2, 16, 36, 0, 0, DateTimeKind.Unspecified), 3, 120000m, new DateTime(2026, 3, 2, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 1, new DateTime(2026, 3, 2, 21, 36, 0, 0, DateTimeKind.Unspecified), 3, 150000m, new DateTime(2026, 3, 2, 19, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 3, new DateTime(2026, 3, 2, 12, 23, 0, 0, DateTimeKind.Unspecified), 4, 90000m, new DateTime(2026, 3, 2, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 4, new DateTime(2026, 3, 2, 16, 23, 0, 0, DateTimeKind.Unspecified), 4, 120000m, new DateTime(2026, 3, 2, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 2, new DateTime(2026, 3, 2, 21, 23, 0, 0, DateTimeKind.Unspecified), 4, 150000m, new DateTime(2026, 3, 2, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 3, new DateTime(2026, 3, 3, 12, 28, 0, 0, DateTimeKind.Unspecified), 1, 90000m, new DateTime(2026, 3, 3, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, 4, new DateTime(2026, 3, 3, 16, 58, 0, 0, DateTimeKind.Unspecified), 1, 120000m, new DateTime(2026, 3, 3, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 42, 2, new DateTime(2026, 3, 3, 21, 58, 0, 0, DateTimeKind.Unspecified), 1, 150000m, new DateTime(2026, 3, 3, 19, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 4, new DateTime(2026, 3, 3, 11, 32, 0, 0, DateTimeKind.Unspecified), 2, 90000m, new DateTime(2026, 3, 3, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 44, 1, new DateTime(2026, 3, 3, 16, 2, 0, 0, DateTimeKind.Unspecified), 2, 120000m, new DateTime(2026, 3, 3, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 45, 2, new DateTime(2026, 3, 3, 20, 2, 0, 0, DateTimeKind.Unspecified), 2, 150000m, new DateTime(2026, 3, 3, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 46, 1, new DateTime(2026, 3, 3, 12, 36, 0, 0, DateTimeKind.Unspecified), 3, 90000m, new DateTime(2026, 3, 3, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 47, 3, new DateTime(2026, 3, 3, 17, 6, 0, 0, DateTimeKind.Unspecified), 3, 120000m, new DateTime(2026, 3, 3, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, 4, new DateTime(2026, 3, 3, 22, 6, 0, 0, DateTimeKind.Unspecified), 3, 150000m, new DateTime(2026, 3, 3, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 49, 2, new DateTime(2026, 3, 3, 12, 23, 0, 0, DateTimeKind.Unspecified), 4, 90000m, new DateTime(2026, 3, 3, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 50, 1, new DateTime(2026, 3, 3, 16, 23, 0, 0, DateTimeKind.Unspecified), 4, 120000m, new DateTime(2026, 3, 3, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 51, 3, new DateTime(2026, 3, 3, 21, 23, 0, 0, DateTimeKind.Unspecified), 4, 150000m, new DateTime(2026, 3, 3, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 52, 1, new DateTime(2026, 3, 4, 12, 58, 0, 0, DateTimeKind.Unspecified), 1, 90000m, new DateTime(2026, 3, 4, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 53, 2, new DateTime(2026, 3, 4, 17, 28, 0, 0, DateTimeKind.Unspecified), 1, 120000m, new DateTime(2026, 3, 4, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 54, 4, new DateTime(2026, 3, 4, 22, 28, 0, 0, DateTimeKind.Unspecified), 1, 150000m, new DateTime(2026, 3, 4, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 55, 3, new DateTime(2026, 3, 4, 11, 32, 0, 0, DateTimeKind.Unspecified), 2, 90000m, new DateTime(2026, 3, 4, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 56, 2, new DateTime(2026, 3, 4, 15, 32, 0, 0, DateTimeKind.Unspecified), 2, 120000m, new DateTime(2026, 3, 4, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 57, 1, new DateTime(2026, 3, 4, 19, 32, 0, 0, DateTimeKind.Unspecified), 2, 150000m, new DateTime(2026, 3, 4, 18, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 58, 2, new DateTime(2026, 3, 4, 12, 36, 0, 0, DateTimeKind.Unspecified), 3, 90000m, new DateTime(2026, 3, 4, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 59, 1, new DateTime(2026, 3, 4, 17, 6, 0, 0, DateTimeKind.Unspecified), 3, 120000m, new DateTime(2026, 3, 4, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 60, 3, new DateTime(2026, 3, 4, 21, 36, 0, 0, DateTimeKind.Unspecified), 3, 150000m, new DateTime(2026, 3, 4, 19, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 61, 4, new DateTime(2026, 3, 4, 12, 23, 0, 0, DateTimeKind.Unspecified), 4, 90000m, new DateTime(2026, 3, 4, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 62, 3, new DateTime(2026, 3, 4, 16, 53, 0, 0, DateTimeKind.Unspecified), 4, 120000m, new DateTime(2026, 3, 4, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 63, 1, new DateTime(2026, 3, 4, 21, 23, 0, 0, DateTimeKind.Unspecified), 4, 150000m, new DateTime(2026, 3, 4, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 64, 4, new DateTime(2026, 3, 5, 12, 28, 0, 0, DateTimeKind.Unspecified), 1, 90000m, new DateTime(2026, 3, 5, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 65, 3, new DateTime(2026, 3, 5, 16, 58, 0, 0, DateTimeKind.Unspecified), 1, 120000m, new DateTime(2026, 3, 5, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 66, 1, new DateTime(2026, 3, 5, 21, 28, 0, 0, DateTimeKind.Unspecified), 1, 150000m, new DateTime(2026, 3, 5, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 67, 2, new DateTime(2026, 3, 5, 12, 2, 0, 0, DateTimeKind.Unspecified), 2, 90000m, new DateTime(2026, 3, 5, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 68, 4, new DateTime(2026, 3, 5, 15, 32, 0, 0, DateTimeKind.Unspecified), 2, 120000m, new DateTime(2026, 3, 5, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 69, 3, new DateTime(2026, 3, 5, 20, 32, 0, 0, DateTimeKind.Unspecified), 2, 150000m, new DateTime(2026, 3, 5, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 70, 3, new DateTime(2026, 3, 5, 12, 6, 0, 0, DateTimeKind.Unspecified), 3, 90000m, new DateTime(2026, 3, 5, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 71, 1, new DateTime(2026, 3, 5, 16, 36, 0, 0, DateTimeKind.Unspecified), 3, 120000m, new DateTime(2026, 3, 5, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 72, 2, new DateTime(2026, 3, 5, 22, 6, 0, 0, DateTimeKind.Unspecified), 3, 150000m, new DateTime(2026, 3, 5, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 73, 1, new DateTime(2026, 3, 5, 12, 23, 0, 0, DateTimeKind.Unspecified), 4, 90000m, new DateTime(2026, 3, 5, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 74, 2, new DateTime(2026, 3, 5, 17, 23, 0, 0, DateTimeKind.Unspecified), 4, 120000m, new DateTime(2026, 3, 5, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 75, 4, new DateTime(2026, 3, 5, 21, 53, 0, 0, DateTimeKind.Unspecified), 4, 150000m, new DateTime(2026, 3, 5, 19, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 76, 2, new DateTime(2026, 3, 6, 12, 28, 0, 0, DateTimeKind.Unspecified), 1, 90000m, new DateTime(2026, 3, 6, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 77, 1, new DateTime(2026, 3, 6, 17, 28, 0, 0, DateTimeKind.Unspecified), 1, 120000m, new DateTime(2026, 3, 6, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 78, 3, new DateTime(2026, 3, 6, 21, 58, 0, 0, DateTimeKind.Unspecified), 1, 150000m, new DateTime(2026, 3, 6, 19, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 79, 1, new DateTime(2026, 3, 6, 11, 32, 0, 0, DateTimeKind.Unspecified), 2, 90000m, new DateTime(2026, 3, 6, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 80, 3, new DateTime(2026, 3, 6, 17, 2, 0, 0, DateTimeKind.Unspecified), 2, 120000m, new DateTime(2026, 3, 6, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 81, 4, new DateTime(2026, 3, 6, 20, 32, 0, 0, DateTimeKind.Unspecified), 2, 150000m, new DateTime(2026, 3, 6, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 82, 4, new DateTime(2026, 3, 6, 12, 36, 0, 0, DateTimeKind.Unspecified), 3, 90000m, new DateTime(2026, 3, 6, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 83, 2, new DateTime(2026, 3, 6, 16, 36, 0, 0, DateTimeKind.Unspecified), 3, 120000m, new DateTime(2026, 3, 6, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 84, 1, new DateTime(2026, 3, 6, 22, 6, 0, 0, DateTimeKind.Unspecified), 3, 150000m, new DateTime(2026, 3, 6, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 85, 3, new DateTime(2026, 3, 6, 12, 23, 0, 0, DateTimeKind.Unspecified), 4, 90000m, new DateTime(2026, 3, 6, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 86, 4, new DateTime(2026, 3, 6, 16, 23, 0, 0, DateTimeKind.Unspecified), 4, 120000m, new DateTime(2026, 3, 6, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 87, 2, new DateTime(2026, 3, 6, 21, 23, 0, 0, DateTimeKind.Unspecified), 4, 150000m, new DateTime(2026, 3, 6, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 88, 1, new DateTime(2026, 3, 7, 12, 28, 0, 0, DateTimeKind.Unspecified), 1, 100000m, new DateTime(2026, 3, 7, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 89, 2, new DateTime(2026, 3, 7, 16, 28, 0, 0, DateTimeKind.Unspecified), 1, 130000m, new DateTime(2026, 3, 7, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 90, 4, new DateTime(2026, 3, 7, 21, 28, 0, 0, DateTimeKind.Unspecified), 1, 160000m, new DateTime(2026, 3, 7, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 91, 3, new DateTime(2026, 3, 7, 23, 58, 0, 0, DateTimeKind.Unspecified), 1, 180000m, new DateTime(2026, 3, 7, 21, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 92, 3, new DateTime(2026, 3, 7, 11, 32, 0, 0, DateTimeKind.Unspecified), 2, 100000m, new DateTime(2026, 3, 7, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 93, 4, new DateTime(2026, 3, 7, 16, 2, 0, 0, DateTimeKind.Unspecified), 2, 130000m, new DateTime(2026, 3, 7, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 94, 1, new DateTime(2026, 3, 7, 19, 32, 0, 0, DateTimeKind.Unspecified), 2, 160000m, new DateTime(2026, 3, 7, 18, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 95, 2, new DateTime(2026, 3, 7, 21, 32, 0, 0, DateTimeKind.Unspecified), 2, 180000m, new DateTime(2026, 3, 7, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 96, 2, new DateTime(2026, 3, 7, 12, 6, 0, 0, DateTimeKind.Unspecified), 3, 100000m, new DateTime(2026, 3, 7, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 97, 3, new DateTime(2026, 3, 7, 16, 6, 0, 0, DateTimeKind.Unspecified), 3, 130000m, new DateTime(2026, 3, 7, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 98, 4, new DateTime(2026, 3, 7, 21, 6, 0, 0, DateTimeKind.Unspecified), 3, 160000m, new DateTime(2026, 3, 7, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 99, 1, new DateTime(2026, 3, 7, 23, 36, 0, 0, DateTimeKind.Unspecified), 3, 180000m, new DateTime(2026, 3, 7, 21, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 100, 4, new DateTime(2026, 3, 7, 12, 23, 0, 0, DateTimeKind.Unspecified), 4, 100000m, new DateTime(2026, 3, 7, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 101, 1, new DateTime(2026, 3, 7, 16, 53, 0, 0, DateTimeKind.Unspecified), 4, 130000m, new DateTime(2026, 3, 7, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 102, 2, new DateTime(2026, 3, 7, 20, 53, 0, 0, DateTimeKind.Unspecified), 4, 160000m, new DateTime(2026, 3, 7, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 103, 3, new DateTime(2026, 3, 7, 23, 23, 0, 0, DateTimeKind.Unspecified), 4, 180000m, new DateTime(2026, 3, 7, 21, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 103);
        }
    }
}
