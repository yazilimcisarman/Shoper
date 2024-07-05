using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changetablenameCAtegory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoeries",
                table: "Categoeries");

            migrationBuilder.RenameTable(
                name: "Categoeries",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categoeries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoeries",
                table: "Categoeries",
                column: "CategoryId");
        }
    }
}
