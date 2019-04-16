using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminSystem.Api.Infrastructure.Migrations
{
    public partial class addjsdOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JsdOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderCode = table.Column<string>(nullable: true),
                    DeptCode = table.Column<string>(nullable: true),
                    OprDate = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    LastUpDate = table.Column<DateTime>(nullable: false),
                    WeightCode = table.Column<string>(nullable: true),
                    DeliveryUserId = table.Column<string>(nullable: true),
                    DeliveryUserName = table.Column<string>(nullable: true),
                    TrueName = table.Column<string>(nullable: true),
                    CreateOrderMobile = table.Column<string>(nullable: true),
                    CancelUserId = table.Column<string>(nullable: true),
                    CancelTureName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true),
                    IsThisSystemChange = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsdOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JsdOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: false),
                    OrderCode = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    Qty = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    MaterialCode = table.Column<string>(nullable: true),
                    JsdOrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsdOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JsdOrderItems_JsdOrders_JsdOrderId",
                        column: x => x.JsdOrderId,
                        principalTable: "JsdOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JsdOrderItems_JsdOrderId",
                table: "JsdOrderItems",
                column: "JsdOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JsdOrderItems");

            migrationBuilder.DropTable(
                name: "JsdOrders");
        }
    }
}
