using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Showtimes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 1,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 2,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 3,
                column: "Price",
                value: 120000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 4,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 5,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 6,
                column: "Price",
                value: 120000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 7,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 8,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 9,
                column: "Price",
                value: 180000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 10,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 11,
                column: "Price",
                value: 180000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 12,
                column: "Price",
                value: 120000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 13,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 14,
                column: "Price",
                value: 150000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 15,
                column: "Price",
                value: 180000m);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 16,
                column: "Price",
                value: 120000m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Showtimes");
        }
    }
}
