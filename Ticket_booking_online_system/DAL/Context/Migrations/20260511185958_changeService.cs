using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_booking_online_system.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_Flight_Number",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Airlines_Airline_ID",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_Airline_ID",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Airline_ID",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Arrival_Time",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Check_In",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Check_Out",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Company_Name",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Departure_Time",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Dropoff_Detail",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Flight_Number",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Hotel_Name",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Pickup_Detail",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Room_Type",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Vehicle_Type",
                table: "Services");

            migrationBuilder.AlterColumn<string>(
                name: "Flight_Number",
                table: "Bookings",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.CreateTable(
                name: "FlightServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Flight_Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Flight_Number1 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Airline_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightServices_Airlines_Airline_ID",
                        column: x => x.Airline_ID,
                        principalTable: "Airlines",
                        principalColumn: "Airline_ID");
                    table.ForeignKey(
                        name: "FK_FlightServices_Flights_Flight_Number1",
                        column: x => x.Flight_Number1,
                        principalTable: "Flights",
                        principalColumn: "Flight_Number");
                });

            migrationBuilder.CreateTable(
                name: "HotelServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Hotel_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Room_Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Check_In = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Check_Out = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportationServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Vehicle_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Company_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Departure_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Arrival_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pickup_Detail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Dropoff_Detail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationServices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightServices_Airline_ID",
                table: "FlightServices",
                column: "Airline_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FlightServices_Flight_Number1",
                table: "FlightServices",
                column: "Flight_Number1");

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

            migrationBuilder.DropTable(
                name: "FlightServices");

            migrationBuilder.DropTable(
                name: "HotelServices");

            migrationBuilder.DropTable(
                name: "TransportationServices");

            migrationBuilder.AddColumn<int>(
                name: "Airline_ID",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Arrival_Time",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Check_In",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Check_Out",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company_Name",
                table: "Services",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Departure_Time",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dropoff_Detail",
                table: "Services",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flight_Number",
                table: "Services",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hotel_Name",
                table: "Services",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pickup_Detail",
                table: "Services",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room_Type",
                table: "Services",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "Services",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vehicle_Type",
                table: "Services",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Flight_Number",
                table: "Bookings",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_Airline_ID",
                table: "Services",
                column: "Airline_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_Flight_Number",
                table: "Bookings",
                column: "Flight_Number",
                principalTable: "Flights",
                principalColumn: "Flight_Number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Airlines_Airline_ID",
                table: "Services",
                column: "Airline_ID",
                principalTable: "Airlines",
                principalColumn: "Airline_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
