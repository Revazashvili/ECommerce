using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class specify_schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductCategory_ProductCategories_CategoriesId",
                table: "ProductProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductCategory_Products_ProductsId",
                table: "ProductProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories");

            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products",
                newSchema: "products");

            migrationBuilder.RenameTable(
                name: "ProductProductCategory",
                newName: "ProductProductCategory",
                newSchema: "products");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                newName: "product_categories",
                newSchema: "products");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Name",
                schema: "products",
                table: "products",
                newName: "IX_products_Name");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_Name",
                schema: "products",
                table: "product_categories",
                newName: "IX_product_categories_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                schema: "products",
                table: "products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_categories",
                schema: "products",
                table: "product_categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCategory_product_categories_CategoriesId",
                schema: "products",
                table: "ProductProductCategory",
                column: "CategoriesId",
                principalSchema: "products",
                principalTable: "product_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCategory_products_ProductsId",
                schema: "products",
                table: "ProductProductCategory",
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
                name: "FK_ProductProductCategory_product_categories_CategoriesId",
                schema: "products",
                table: "ProductProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductCategory_products_ProductsId",
                schema: "products",
                table: "ProductProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                schema: "products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_categories",
                schema: "products",
                table: "product_categories");

            migrationBuilder.RenameTable(
                name: "products",
                schema: "products",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "ProductProductCategory",
                schema: "products",
                newName: "ProductProductCategory");

            migrationBuilder.RenameTable(
                name: "product_categories",
                schema: "products",
                newName: "ProductCategories");

            migrationBuilder.RenameIndex(
                name: "IX_products_Name",
                table: "Products",
                newName: "IX_Products_Name");

            migrationBuilder.RenameIndex(
                name: "IX_product_categories_Name",
                table: "ProductCategories",
                newName: "IX_ProductCategories_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCategory_ProductCategories_CategoriesId",
                table: "ProductProductCategory",
                column: "CategoriesId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCategory_Products_ProductsId",
                table: "ProductProductCategory",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
