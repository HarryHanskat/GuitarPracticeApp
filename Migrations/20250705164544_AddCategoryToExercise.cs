using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuitarPracticeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cagegory",
                table: "Exercises",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Exercises",
                newName: "Cagegory");
        }
    }
}
