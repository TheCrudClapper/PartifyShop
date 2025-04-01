using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedMistaskes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "ParcelLockerDeliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ParcelLockerDeliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ParcelLockerDeliveries");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ParcelLockerDeliveries");
        }
    }
}
