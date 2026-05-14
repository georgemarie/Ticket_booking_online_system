using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_booking_online_system.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightClassEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Convert existing string data to integer values
            migrationBuilder.Sql("UPDATE Flights SET Class = '0' WHERE Class = 'Economy'");
            migrationBuilder.Sql("UPDATE Flights SET Class = '1' WHERE Class = 'Business'");
            migrationBuilder.Sql("UPDATE Flights SET Class = '2' WHERE Class = 'FirstClass'");

            // 2. Handle any nulls or unexpected strings just in case
            migrationBuilder.Sql("UPDATE Flights SET Class = '0' WHERE Class NOT IN ('0', '1', '2')");

            // 3. Now perform the type change
            migrationBuilder.AlterColumn<int>(
                name: "Class",
                table: "Flights",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "Flights",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);
        }
    }
}
