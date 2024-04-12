using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Brickwell.Data.Migrations
{
    /// <inheritdoc />
    public partial class favorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    favorite_1 = table.Column<int>(type: "int", nullable: true),
                    favorite_2 = table.Column<int>(type: "int", nullable: true),
                    favorite_3 = table.Column<int>(type: "int", nullable: true),
                    favorite_4 = table.Column<int>(type: "int", nullable: true),
                    favorite_5 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.type);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");
        }
    }
}
