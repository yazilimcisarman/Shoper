using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ordermodelchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymnetMethod",
                table: "Orders",
                newName: "PaymentMethod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Orders",
                newName: "PaymnetMethod");
        }
    }
}
