using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.WebUI.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Oturums_OturumId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "OturumId",
                table: "Questions",
                newName: "QuestionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_OturumId",
                table: "Questions",
                newName: "IX_Questions_QuestionCategoryId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Oturums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "QuestionCategoryId",
                table: "Oturums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oturums_QuestionCategoryId",
                table: "Oturums",
                column: "QuestionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Oturums_QuestionCategory_QuestionCategoryId",
                table: "Oturums",
                column: "QuestionCategoryId",
                principalTable: "QuestionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionCategory_QuestionCategoryId",
                table: "Questions",
                column: "QuestionCategoryId",
                principalTable: "QuestionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oturums_QuestionCategory_QuestionCategoryId",
                table: "Oturums");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionCategory_QuestionCategoryId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionCategory");

            migrationBuilder.DropIndex(
                name: "IX_Oturums_QuestionCategoryId",
                table: "Oturums");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Oturums");

            migrationBuilder.DropColumn(
                name: "QuestionCategoryId",
                table: "Oturums");

            migrationBuilder.RenameColumn(
                name: "QuestionCategoryId",
                table: "Questions",
                newName: "OturumId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuestionCategoryId",
                table: "Questions",
                newName: "IX_Questions_OturumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Oturums_OturumId",
                table: "Questions",
                column: "OturumId",
                principalTable: "Oturums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
