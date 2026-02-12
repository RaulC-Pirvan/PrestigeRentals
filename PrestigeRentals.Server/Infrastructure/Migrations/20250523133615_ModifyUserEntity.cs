using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrestigeRentals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "UsersDetails");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageFileName",
                table: "UsersDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageFileName",
                table: "UsersDetails");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "UsersDetails",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
