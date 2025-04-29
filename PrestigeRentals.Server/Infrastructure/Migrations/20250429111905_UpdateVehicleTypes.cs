using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrestigeRentals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePhotos_Vehicles_VehicleId1",
                table: "VehiclePhotos");

            migrationBuilder.DropIndex(
                name: "IX_VehiclePhotos_VehicleId1",
                table: "VehiclePhotos");

            migrationBuilder.DropColumn(
                name: "VehicleId1",
                table: "VehiclePhotos");

            migrationBuilder.AlterColumn<int>(
                name: "EngineSize",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<long>(
                name: "VehicleId",
                table: "VehiclePhotos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "VehiclePhotos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePhotos_VehicleId",
                table: "VehiclePhotos",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePhotos_Vehicles_VehicleId",
                table: "VehiclePhotos",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePhotos_Vehicles_VehicleId",
                table: "VehiclePhotos");

            migrationBuilder.DropIndex(
                name: "IX_VehiclePhotos_VehicleId",
                table: "VehiclePhotos");

            migrationBuilder.AlterColumn<decimal>(
                name: "EngineSize",
                table: "Vehicles",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "VehiclePhotos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "VehiclePhotos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "VehicleId1",
                table: "VehiclePhotos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePhotos_VehicleId1",
                table: "VehiclePhotos",
                column: "VehicleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePhotos_Vehicles_VehicleId1",
                table: "VehiclePhotos",
                column: "VehicleId1",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
