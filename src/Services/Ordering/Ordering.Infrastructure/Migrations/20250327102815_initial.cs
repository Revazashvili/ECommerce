﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ordering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.EnsureSchema(
                name: "outbox");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "orders",
                columns: table => new
                {
                    order_number = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: false),
                    address_city = table.Column<string>(type: "text", nullable: false),
                    address_state = table.Column<string>(type: "text", nullable: true),
                    address_country = table.Column<string>(type: "text", nullable: false),
                    address_zip_code = table.Column<string>(type: "text", nullable: false),
                    order_status = table.Column<string>(type: "text", nullable: false),
                    ordering_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.order_number);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "outbox",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    aggregate_id = table.Column<string>(type: "text", nullable: false),
                    topic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    picture_url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    order_number = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_number",
                        column: x => x.order_number,
                        principalSchema: "orders",
                        principalTable: "orders",
                        principalColumn: "order_number");
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_number",
                schema: "orders",
                table: "order_items",
                column: "order_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_items",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "outbox");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders");
        }
    }
}
