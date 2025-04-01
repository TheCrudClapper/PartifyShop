using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class DBMigrationV101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Offer_OfferId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_DeliveryType_DeliveryTypeId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Products_ProductId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Users_SellerId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategory_ProductCategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryType",
                table: "DeliveryType");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "ProductCategories");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "DeliveryType",
                newName: "ParcelLockerDeliveries");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_SellerId",
                table: "Offers",
                newName: "IX_Offers_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_ProductId",
                table: "Offers",
                newName: "IX_Offers_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_DeliveryTypeId",
                table: "Offers",
                newName: "IX_Offers_DeliveryTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryTypes",
                table: "ParcelLockerDeliveries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Offers_OfferId",
                table: "CartItems",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_DeliveryTypes_DeliveryTypeId",
                table: "Offers",
                column: "DeliveryTypeId",
                principalTable: "ParcelLockerDeliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Products_ProductId",
                table: "Offers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_SellerId",
                table: "Offers",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Offers_OfferId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_DeliveryTypes_DeliveryTypeId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Products_ProductId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_SellerId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryTypes",
                table: "ParcelLockerDeliveries");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                newName: "ProductCategory");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameTable(
                name: "ParcelLockerDeliveries",
                newName: "DeliveryType");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_SellerId",
                table: "Offer",
                newName: "IX_Offer_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_ProductId",
                table: "Offer",
                newName: "IX_Offer_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_DeliveryTypeId",
                table: "Offer",
                newName: "IX_Offer_DeliveryTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryType",
                table: "DeliveryType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Offer_OfferId",
                table: "CartItems",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_DeliveryType_DeliveryTypeId",
                table: "Offer",
                column: "DeliveryTypeId",
                principalTable: "DeliveryType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Products_ProductId",
                table: "Offer",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Users_SellerId",
                table: "Offer",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategory_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
