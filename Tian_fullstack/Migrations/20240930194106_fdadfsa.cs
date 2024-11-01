using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tian_fullstack.Migrations
{
    /// <inheritdoc />
    public partial class fdadfsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "enableCodeEditor",
                table: "Slides",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enableCodeEditor",
                table: "Slides");
        }
    }
}
