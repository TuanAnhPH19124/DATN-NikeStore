using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class UpdateTableShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartItemsProductsId",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartItemsShoppingCartsId",
                table: "Product",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    ShoppingCartsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductsId = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => new { x.ShoppingCartsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_ShoppingCarts_ShoppingCartsId",
                        column: x => x.ShoppingCartsId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ShoppingCartItemsShoppingCartsId_ShoppingCartItemsP~",
                table: "Product",
                columns: new[] { "ShoppingCartItemsShoppingCartsId", "ShoppingCartItemsProductsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductsId",
                table: "ShoppingCartItems",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_AppUserId",
                table: "ShoppingCarts",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ShoppingCartItems_ShoppingCartItemsShoppingCartsId_~",
                table: "Product",
                columns: new[] { "ShoppingCartItemsShoppingCartsId", "ShoppingCartItemsProductsId" },
                principalTable: "ShoppingCartItems",
                principalColumns: new[] { "ShoppingCartsId", "ProductsId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ShoppingCartItems_ShoppingCartItemsShoppingCartsId_~",
                table: "Product");

            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_Product_ShoppingCartItemsShoppingCartsId_ShoppingCartItemsP~",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShoppingCartItemsProductsId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShoppingCartItemsShoppingCartsId",
                table: "Product");
        }
    }
}
