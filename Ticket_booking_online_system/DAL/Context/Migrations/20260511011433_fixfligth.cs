using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_booking_online_system.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixfligth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_Flight_Number",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Flight_Number",
                table: "Bookings",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_Flight_Number",
                table: "Bookings",
                column: "Flight_Number",
                principalTable: "Flights",
                principalColumn: "Flight_Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_Flight_Number",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Flight_Number",
                table: "Bookings",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_Flight_Number",
                table: "Bookings",
                column: "Flight_Number",
                principalTable: "Flights",
                principalColumn: "Flight_Number",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
