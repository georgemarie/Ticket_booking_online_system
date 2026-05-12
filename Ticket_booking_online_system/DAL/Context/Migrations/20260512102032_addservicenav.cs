using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_booking_online_system.Data.Migrations
{
    /// <inheritdoc />
    public partial class addservicenav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Hotel_Name",
                table: "HotelServices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.CreateIndex(
                name: "IX_HotelServices_ServiceId",
                table: "HotelServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightServices_ServiceId",
                table: "FlightServices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightServices_Services_ServiceId",
                table: "FlightServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServices_Services_ServiceId",
                table: "HotelServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightServices_Services_ServiceId",
                table: "FlightServices");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServices_Services_ServiceId",
                table: "HotelServices");

            migrationBuilder.DropIndex(
                name: "IX_HotelServices_ServiceId",
                table: "HotelServices");

            migrationBuilder.DropIndex(
                name: "IX_FlightServices_ServiceId",
                table: "FlightServices");

            migrationBuilder.AlterColumn<string>(
                name: "Hotel_Name",
                table: "HotelServices",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
