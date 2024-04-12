using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Brickwell.Data.Migrations
{
    /// <inheritdoc />
    public partial class customerRecommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Products_product_ID",
                table: "Recommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations");

            migrationBuilder.RenameTable(
                name: "Recommendations",
                newName: "ProductRecommendations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRecommendations",
                table: "ProductRecommendations",
                column: "product_ID");

            migrationBuilder.CreateTable(
                name: "CustomerRecommendations",
                columns: table => new
                {
                    customer_ID = table.Column<int>(type: "int", nullable: false),
                    recommendation_1 = table.Column<int>(type: "int", nullable: true),
                    recommendation_2 = table.Column<int>(type: "int", nullable: true),
                    recommendation_3 = table.Column<int>(type: "int", nullable: true),
                    recommendation_4 = table.Column<int>(type: "int", nullable: true),
                    recommendation_5 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRecommendations", x => x.customer_ID);
                    table.ForeignKey(
                        name: "FK_CustomerRecommendations_Customers_customer_ID",
                        column: x => x.customer_ID,
                        principalTable: "Customers",
                        principalColumn: "customer_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_Products_product_ID",
                table: "ProductRecommendations",
                column: "product_ID",
                principalTable: "Products",
                principalColumn: "product_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_Products_product_ID",
                table: "ProductRecommendations");

            migrationBuilder.DropTable(
                name: "CustomerRecommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRecommendations",
                table: "ProductRecommendations");

            migrationBuilder.RenameTable(
                name: "ProductRecommendations",
                newName: "Recommendations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations",
                column: "product_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Products_product_ID",
                table: "Recommendations",
                column: "product_ID",
                principalTable: "Products",
                principalColumn: "product_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
