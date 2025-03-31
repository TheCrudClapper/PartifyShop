using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerServiceOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class ConditionMigrationRepair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProducImages_Products_ProductId",
                table: "ProducImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProducImages",
                table: "ProducImages");

            migrationBuilder.RenameTable(
                name: "ProducImages",
                newName: "ProductImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProducImages_ProductId",
                table: "ProductImages",
                newName: "IX_ProductImages_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "ConditionId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ConditionId",
                table: "Products",
                column: "ConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Conditions_ConditionId",
                table: "Products",
                column: "ConditionId",
                principalTable: "Conditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Conditions_ConditionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ConditionId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ConditionId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProducImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProducImages",
                newName: "IX_ProducImages_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProducImages",
                table: "ProducImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProducImages_Products_ProductId",
                table: "ProducImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
