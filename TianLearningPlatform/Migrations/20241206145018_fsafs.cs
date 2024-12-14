using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tian_fullstack.Migrations
{
    /// <inheritdoc />
    public partial class fsafs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastLessonNumber",
                table: "CompletedLessons",
                newName: "LessonNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessonNumber",
                table: "CompletedLessons",
                newName: "LastLessonNumber");
        }
    }
}
