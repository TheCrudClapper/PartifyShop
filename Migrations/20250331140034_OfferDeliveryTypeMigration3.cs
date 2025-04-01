using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class OfferDeliveryTypeMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTypes_Offers_OfferId",
                table: "ParcelLockerDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryTypes_OfferId",
                table: "ParcelLockerDeliveries");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "ParcelLockerDeliveries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "ParcelLockerDeliveries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTypes_OfferId",
                table: "ParcelLockerDeliveries",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTypes_Offers_OfferId",
                table: "ParcelLockerDeliveries",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }
    }
}
