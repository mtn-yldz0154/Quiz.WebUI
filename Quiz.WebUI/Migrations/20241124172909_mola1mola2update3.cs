using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.WebUI.Migrations
{
    public partial class mola1mola2update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mola1",
                table: "QuizSessions");

            migrationBuilder.DropColumn(
                name: "Mola2",
                table: "QuizSessions");

            migrationBuilder.AddColumn<int>(
                name: "Mola1",
                table: "Oturums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mola2",
                table: "Oturums",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mola1",
                table: "Oturums");

            migrationBuilder.DropColumn(
                name: "Mola2",
                table: "Oturums");

            migrationBuilder.AddColumn<int>(
                name: "Mola1",
                table: "QuizSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mola2",
                table: "QuizSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
