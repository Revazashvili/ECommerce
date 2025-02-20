using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class specifiy_schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderNumber",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "orders",
                newSchema: "orders");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "order_items",
                newSchema: "orders");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderNumber",
                schema: "orders",
                table: "order_items",
                newName: "IX_order_items_OrderNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                schema: "orders",
                table: "orders",
                column: "OrderNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_items",
                schema: "orders",
                table: "order_items",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_orders_OrderNumber",
                schema: "orders",
                table: "order_items",
                column: "OrderNumber",
                principalSchema: "orders",
                principalTable: "orders",
                principalColumn: "OrderNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_items_orders_OrderNumber",
                schema: "orders",
                table: "order_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                schema: "orders",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_items",
                schema: "orders",
                table: "order_items");

            migrationBuilder.RenameTable(
                name: "orders",
                schema: "orders",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "order_items",
                schema: "orders",
                newName: "OrderItems");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_OrderNumber",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderNumber",
                table: "OrderItems",
                column: "OrderNumber",
                principalTable: "Orders",
                principalColumn: "OrderNumber");
        }
    }
}
