using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrestigeRentals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiclePhotosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photoUrl",
                table: "VehiclePhotos");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "VehiclePhotos",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "VehiclePhotos");

            migrationBuilder.AddColumn<string>(
                name: "photoUrl",
                table: "VehiclePhotos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
