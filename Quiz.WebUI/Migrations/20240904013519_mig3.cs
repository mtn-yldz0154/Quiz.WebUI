using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.WebUI.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oturums_QuestionCategory_QuestionCategoryId",
                table: "Oturums");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionCategory_QuestionCategoryId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionCategory",
                table: "QuestionCategory");

            migrationBuilder.RenameTable(
                name: "QuestionCategory",
                newName: "QuestionCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionCategories",
                table: "QuestionCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Oturums_QuestionCategories_QuestionCategoryId",
                table: "Oturums",
                column: "QuestionCategoryId",
                principalTable: "QuestionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionCategories_QuestionCategoryId",
                table: "Questions",
                column: "QuestionCategoryId",
                principalTable: "QuestionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oturums_QuestionCategories_QuestionCategoryId",
                table: "Oturums");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionCategories_QuestionCategoryId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionCategories",
                table: "QuestionCategories");

            migrationBuilder.RenameTable(
                name: "QuestionCategories",
                newName: "QuestionCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionCategory",
                table: "QuestionCategory",
                column: "Id");

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
    }
}
