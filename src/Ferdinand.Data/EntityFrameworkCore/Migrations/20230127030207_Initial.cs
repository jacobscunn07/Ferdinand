using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ferdinand.Data.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uuid", nullable: false),
                    Tenant = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HexValue = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Color");
        }
    }
}
