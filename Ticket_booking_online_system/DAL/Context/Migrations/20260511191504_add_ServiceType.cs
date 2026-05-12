using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_booking_online_system.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_ServiceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "Services");
        }
    }
}
