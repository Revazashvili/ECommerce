using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class specify_relationtiop_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductCategory",
                schema: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_categories",
                schema: "products",
                table: "product_categories");

            migrationBuilder.DropIndex(
                name: "IX_product_categories_Name",
                schema: "products",
                table: "product_categories");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "products",
                table: "product_categories");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "products",
                table: "product_categories",
                newName: "CategoriesId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriesId",
                schema: "products",
                table: "product_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                schema: "products",
                table: "product_categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_categories",
                schema: "products",
                table: "product_categories",
                columns: new[] { "CategoriesId", "ProductsId" });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_ProductsId",
                schema: "products",
                table: "product_categories",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                schema: "products",
                table: "categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_product_categories_categories_CategoriesId",
                schema: "products",
                table: "product_categories",
                column: "CategoriesId",
                principalSchema: "products",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_categories_products_ProductsId",
                schema: "products",
                table: "product_categories",
                column: "ProductsId",
                principalSchema: "products",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_categories_categories_CategoriesId",
                schema: "products",
                table: "product_categories");

            migrationBuilder.DropForeignKey(
                name: "FK_product_categories_products_ProductsId",
                schema: "products",
                table: "product_categories");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_categories",
                schema: "products",
                table: "product_categories");

            migrationBuilder.DropIndex(
                name: "IX_product_categories_ProductsId",
                schema: "products",
                table: "product_categories");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                schema: "products",
                table: "product_categories");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                schema: "products",
                table: "product_categories",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "products",
                table: "product_categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "products",
                table: "product_categories",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_categories",
                schema: "products",
                table: "product_categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductProductCategory",
                schema: "products",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductCategory", x => new { x.CategoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductProductCategory_product_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "products",
                        principalTable: "product_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductCategory_products_ProductsId",
                        column: x => x.ProductsId,
                        principalSchema: "products",
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_Name",
                schema: "products",
                table: "product_categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductCategory_ProductsId",
                schema: "products",
                table: "ProductProductCategory",
                column: "ProductsId");
        }
    }
}
