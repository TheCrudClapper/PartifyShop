using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class OfferDeliveryTypeMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferDeliveryType_DeliveryTypes_DeliveryTypeId",
                table: "OfferDeliveryType");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferDeliveryType_Offers_OfferId",
                table: "OfferDeliveryType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferDeliveryType",
                table: "OfferDeliveryType");

            migrationBuilder.RenameTable(
                name: "OfferDeliveryType",
                newName: "OfferDeliveryTypes");

            migrationBuilder.RenameIndex(
                name: "IX_OfferDeliveryType_OfferId",
                table: "OfferDeliveryTypes",
                newName: "IX_OfferDeliveryTypes_OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferDeliveryType_DeliveryTypeId",
                table: "OfferDeliveryTypes",
                newName: "IX_OfferDeliveryTypes_DeliveryTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferDeliveryTypes",
                table: "OfferDeliveryTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferDeliveryTypes_DeliveryTypes_DeliveryTypeId",
                table: "OfferDeliveryTypes",
                column: "DeliveryTypeId",
                principalTable: "DeliveryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferDeliveryTypes_Offers_OfferId",
                table: "OfferDeliveryTypes",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferDeliveryTypes_DeliveryTypes_DeliveryTypeId",
                table: "OfferDeliveryTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferDeliveryTypes_Offers_OfferId",
                table: "OfferDeliveryTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferDeliveryTypes",
                table: "OfferDeliveryTypes");

            migrationBuilder.RenameTable(
                name: "OfferDeliveryTypes",
                newName: "OfferDeliveryType");

            migrationBuilder.RenameIndex(
                name: "IX_OfferDeliveryTypes_OfferId",
                table: "OfferDeliveryType",
                newName: "IX_OfferDeliveryType_OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferDeliveryTypes_DeliveryTypeId",
                table: "OfferDeliveryType",
                newName: "IX_OfferDeliveryType_DeliveryTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferDeliveryType",
                table: "OfferDeliveryType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferDeliveryType_DeliveryTypes_DeliveryTypeId",
                table: "OfferDeliveryType",
                column: "DeliveryTypeId",
                principalTable: "DeliveryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferDeliveryType_Offers_OfferId",
                table: "OfferDeliveryType",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
