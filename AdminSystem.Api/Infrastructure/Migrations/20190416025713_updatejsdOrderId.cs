using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminSystem.Api.Infrastructure.Migrations
{
    public partial class updatejsdOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JsdOrderItems_JsdOrders_JsdOrderId",
                table: "JsdOrderItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "JsdOrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "JsdOrderId",
                table: "JsdOrderItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JsdOrderItems_JsdOrders_JsdOrderId",
                table: "JsdOrderItems",
                column: "JsdOrderId",
                principalTable: "JsdOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JsdOrderItems_JsdOrders_JsdOrderId",
                table: "JsdOrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "JsdOrderId",
                table: "JsdOrderItems",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "JsdOrderItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_JsdOrderItems_JsdOrders_JsdOrderId",
                table: "JsdOrderItems",
                column: "JsdOrderId",
                principalTable: "JsdOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
