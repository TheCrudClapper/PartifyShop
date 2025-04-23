using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferDeliveryTypes_DeliveryTypes_DeliveryTypeId",
                table: "OfferDeliveryTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryTypes",
                table: "DeliveryTypes");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProductImagesPaths");

            migrationBuilder.RenameTable(
                name: "DeliveryTypes",
                newName: "ParcelLockerDeliveries");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImagesPaths",
                newName: "IX_ProductImagesPaths_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImagesPaths",
                table: "ProductImagesPaths",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParcelLockerDeliveries",
                table: "ParcelLockerDeliveries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferDeliveryTypes_ParcelLockerDeliveries_DeliveryTypeId",
                table: "OfferDeliveryTypes",
                column: "DeliveryTypeId",
                principalTable: "ParcelLockerDeliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImagesPaths_Products_ProductId",
                table: "ProductImagesPaths",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
