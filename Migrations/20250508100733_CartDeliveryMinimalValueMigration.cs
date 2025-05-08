using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class CartDeliveryMinimalValueMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinimalDeliveryValue",
                table: "Carts",
                type: "decimal(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimalDeliveryValue",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductImages",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductCategories",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Offers",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OfferDeliveryTypes",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DeliveryTypes",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Countries",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conditions",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Carts",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartItems",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUsers",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUserClaims",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetRoles",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetRoleClaims",
                newName: "CartiD");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Addresses",
                newName: "CartiD");
        }
    }
}
