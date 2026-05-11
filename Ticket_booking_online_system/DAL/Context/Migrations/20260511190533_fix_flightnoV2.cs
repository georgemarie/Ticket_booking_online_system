using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_booking_online_system.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_flightnoV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightServices_Flights_Flight_Number1",
                table: "FlightServices");

            migrationBuilder.DropIndex(
                name: "IX_FlightServices_Flight_Number1",
                table: "FlightServices");

            migrationBuilder.DropColumn(
                name: "Flight_Number1",
                table: "FlightServices");

            migrationBuilder.CreateIndex(
                name: "IX_FlightServices_Flight_Number",
                table: "FlightServices",
                column: "Flight_Number");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightServices_Flights_Flight_Number",
                table: "FlightServices",
                column: "Flight_Number",
                principalTable: "Flights",
                principalColumn: "Flight_Number",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightServices_Flights_Flight_Number",
                table: "FlightServices");

            migrationBuilder.DropIndex(
                name: "IX_FlightServices_Flight_Number",
                table: "FlightServices");

            migrationBuilder.AddColumn<string>(
                name: "Flight_Number1",
                table: "FlightServices",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightServices_Flight_Number1",
                table: "FlightServices",
                column: "Flight_Number1");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightServices_Flights_Flight_Number1",
                table: "FlightServices",
                column: "Flight_Number1",
                principalTable: "Flights",
                principalColumn: "Flight_Number");
        }
    }
}
