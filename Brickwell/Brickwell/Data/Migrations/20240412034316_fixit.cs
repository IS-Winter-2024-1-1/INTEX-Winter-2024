using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Brickwell.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
               name: "ProductRecommendations",
               columns: table => new
               {
                   product_ID = table.Column<int>(type: "int", nullable: false),
                   recommendation_1 = table.Column<int>(type: "int", nullable: true),
                   recommendation_2 = table.Column<int>(type: "int", nullable: true),
                   recommendation_3 = table.Column<int>(type: "int", nullable: true),
                   recommendation_4 = table.Column<int>(type: "int", nullable: true),
                   recommendation_5 = table.Column<int>(type: "int", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Recommendations", x => x.product_ID);
                   table.ForeignKey(
                       name: "FK_Recommendations_Products_product_ID",
                       column: x => x.product_ID,
                       principalTable: "Products",
                       principalColumn: "product_ID",
                       onDelete: ReferentialAction.Cascade);
               });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRecommendations");


        }
    }
}
