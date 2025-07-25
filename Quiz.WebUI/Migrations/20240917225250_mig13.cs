﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.WebUI.Migrations
{
    public partial class mig13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<int>(type: "int", nullable: false),
                    UserToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionToken = table.Column<int>(type: "int", nullable: false),
                    OturumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerQuestions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerQuestions");
        }
    }
}
