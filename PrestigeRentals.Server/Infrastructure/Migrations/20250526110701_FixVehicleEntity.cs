using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrestigeRentals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixVehicleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerHour",
                table: "Vehicles",
                newName: "PricePerDay");

            migrationBuilder.RenameColumn(
                name: "PricePerHour",
                table: "Orders",
                newName: "PricePerDay");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerDay",
                table: "Vehicles",
                newName: "PricePerHour");

            migrationBuilder.RenameColumn(
                name: "PricePerDay",
                table: "Orders",
                newName: "PricePerHour");
        }
    }
}
