using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project1.Data.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreItemInventories",
                columns: table => new
                {
                    StoreItemInventoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    itemInventory = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItemInventories", x => x.StoreItemInventoryId);
                });

            migrationBuilder.CreateTable(
                name: "StoreLocations",
                columns: table => new
                {
                    StoreLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreLocations", x => x.StoreLocationId);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    UserInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fName = table.Column<string>(nullable: true),
                    lName = table.Column<string>(nullable: true),
                    userName = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.UserInfoId);
                });

            migrationBuilder.CreateTable(
                name: "StoreItems",
                columns: table => new
                {
                    StoreItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreLocationId = table.Column<int>(nullable: true),
                    StoreItemInventoryId = table.Column<int>(nullable: true),
                    itemName = table.Column<string>(nullable: true),
                    itemPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItems", x => x.StoreItemId);
                    table.ForeignKey(
                        name: "FK_StoreItems_StoreItemInventories_StoreItemInventoryId",
                        column: x => x.StoreItemInventoryId,
                        principalTable: "StoreItemInventories",
                        principalColumn: "StoreItemInventoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreItems_StoreLocations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "StoreLocations",
                        principalColumn: "StoreLocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserOrders",
                columns: table => new
                {
                    UserOrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInfoId = table.Column<int>(nullable: true),
                    StoreLocationId = table.Column<int>(nullable: true),
                    timeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrders", x => x.UserOrderId);
                    table.ForeignKey(
                        name: "FK_UserOrders_StoreLocations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "StoreLocations",
                        principalColumn: "StoreLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserOrders_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "UserInfoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserOrderItems",
                columns: table => new
                {
                    UserOrderItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreItemId = table.Column<int>(nullable: true),
                    UserOrderId = table.Column<int>(nullable: true),
                    OrderQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrderItems", x => x.UserOrderItemId);
                    table.ForeignKey(
                        name: "FK_UserOrderItems_StoreItems_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItems",
                        principalColumn: "StoreItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserOrderItems_UserOrders_UserOrderId",
                        column: x => x.UserOrderId,
                        principalTable: "UserOrders",
                        principalColumn: "UserOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_StoreItemInventoryId",
                table: "StoreItems",
                column: "StoreItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_StoreLocationId",
                table: "StoreItems",
                column: "StoreLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrderItems_StoreItemId",
                table: "UserOrderItems",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrderItems_UserOrderId",
                table: "UserOrderItems",
                column: "UserOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_StoreLocationId",
                table: "UserOrders",
                column: "StoreLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrders_UserInfoId",
                table: "UserOrders",
                column: "UserInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOrderItems");

            migrationBuilder.DropTable(
                name: "StoreItems");

            migrationBuilder.DropTable(
                name: "UserOrders");

            migrationBuilder.DropTable(
                name: "StoreItemInventories");

            migrationBuilder.DropTable(
                name: "StoreLocations");

            migrationBuilder.DropTable(
                name: "UserInfos");
        }
    }
}
