using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ferdinand.Data.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class HexValueToBeUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Color_HexValue",
                table: "Color",
                column: "HexValue",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Color_HexValue",
                table: "Color");
        }
    }
}
