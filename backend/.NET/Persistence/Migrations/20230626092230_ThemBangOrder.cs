using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class ThemBangOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Vouchers",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Paymethod = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PassivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    AppUserId = table.Column<string>(type: "text", nullable: true),
                    VoucherId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders",
                column: "VoucherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Vouchers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
