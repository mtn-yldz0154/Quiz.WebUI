using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.WebUI.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oturums_QuestionCategories_QuestionCategoryId",
                table: "Oturums");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionCategories_QuestionCategoryId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionCategories");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionCategoryId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Oturums_QuestionCategoryId",
                table: "Oturums");

            migrationBuilder.RenameColumn(
                name: "QuestionCategoryId",
                table: "Questions",
                newName: "QuestionType");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "Questions",
                newName: "QuestionCategoryId");

            migrationBuilder.CreateTable(
                name: "QuestionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionCategoryId",
                table: "Questions",
                column: "QuestionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Oturums_QuestionCategoryId",
                table: "Oturums",
                column: "QuestionCategoryId");

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
    }
}
