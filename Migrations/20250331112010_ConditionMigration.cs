using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class ConditionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_DeliveryTypes_DeliveryTypeId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_DeliveryTypeId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryTypeId",
                table: "Offers");

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "ParcelLockerDeliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConditionTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConditionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEdited = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProducImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEdited = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProducImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTypes_OfferId",
                table: "ParcelLockerDeliveries",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_ProducImages_ProductId",
                table: "ProducImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTypes_Offers_OfferId",
                table: "ParcelLockerDeliveries",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTypes_Offers_OfferId",
                table: "ParcelLockerDeliveries");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "ProducImages");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryTypes_OfferId",
                table: "ParcelLockerDeliveries");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "ParcelLockerDeliveries");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTypeId",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DeliveryTypeId",
                table: "Offers",
                column: "DeliveryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_DeliveryTypes_DeliveryTypeId",
                table: "Offers",
                column: "DeliveryTypeId",
                principalTable: "ParcelLockerDeliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
