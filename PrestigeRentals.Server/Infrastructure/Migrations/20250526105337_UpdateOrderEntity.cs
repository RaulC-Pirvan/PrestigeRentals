using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrestigeRentals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHour",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Orders");
        }
    }
}
