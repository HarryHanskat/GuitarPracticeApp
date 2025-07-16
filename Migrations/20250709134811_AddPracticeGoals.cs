using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuitarPracticeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPracticeGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PracticeGoalId",
                table: "PracticeSessions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PracticeGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetValue = table.Column<int>(type: "INTEGER", nullable: false),
                    Period = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeGoals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeSessions_PracticeGoalId",
                table: "PracticeSessions",
                column: "PracticeGoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PracticeSessions_PracticeGoals_PracticeGoalId",
                table: "PracticeSessions",
                column: "PracticeGoalId",
                principalTable: "PracticeGoals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PracticeSessions_PracticeGoals_PracticeGoalId",
                table: "PracticeSessions");

            migrationBuilder.DropTable(
                name: "PracticeGoals");

            migrationBuilder.DropIndex(
                name: "IX_PracticeSessions_PracticeGoalId",
                table: "PracticeSessions");

            migrationBuilder.DropColumn(
                name: "PracticeGoalId",
                table: "PracticeSessions");
        }
    }
}
