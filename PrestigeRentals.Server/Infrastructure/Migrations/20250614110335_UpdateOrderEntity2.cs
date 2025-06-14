using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrestigeRentals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReviewReminderSet",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewReminderSet",
                table: "Orders");
        }
    }
}
