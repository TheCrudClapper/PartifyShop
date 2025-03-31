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
                table: "DeliveryTypes");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryTypes_OfferId",
                table: "DeliveryTypes");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "DeliveryTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "DeliveryTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTypes_OfferId",
                table: "DeliveryTypes",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTypes_Offers_OfferId",
                table: "DeliveryTypes",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }
    }
}
