using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class TestQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                });

            const long defaultSubjectId = 1;
            migrationBuilder.Sql($"INSERT INTO \"Subject\" (\"Id\", \"Name\", \"Description\") VALUES " +
                                 $"({defaultSubjectId}, 'Default', 'Default subject')");
            
            migrationBuilder.AddColumn<long>(
                name: "SubjectId",
                table: "TaskModules",
                nullable: false,
                defaultValue: defaultSubjectId);

            migrationBuilder.CreateTable(
                name: "TestResult",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    MarkEU = table.Column<int>(nullable: false),
                    MarkRU = table.Column<int>(nullable: false),
                    StudentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResult_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SubjectId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestQuestion_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestQuestionVersion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PlainText = table.Column<string>(nullable: true),
                    Difficulty = table.Column<int>(nullable: false),
                    MaxScore = table.Column<int>(nullable: false),
                    TestQuestionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestionVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestQuestionVersion_TestQuestion_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalTable: "TestQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Answer = table.Column<string>(nullable: true),
                    IsRight = table.Column<bool>(nullable: false),
                    TestQuestionVersionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAnswer_TestQuestionVersion_TestQuestionVersionId",
                        column: x => x.TestQuestionVersionId,
                        principalTable: "TestQuestionVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestStudentAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AnswerId = table.Column<long>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    IsRight = table.Column<bool>(nullable: false),
                    TestResultId = table.Column<long>(nullable: true),
                    TestQuestionVersionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestStudentAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestStudentAnswer_TestQuestionVersion_TestQuestionVersionId",
                        column: x => x.TestQuestionVersionId,
                        principalTable: "TestQuestionVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestStudentAnswer_TestResult_TestResultId",
                        column: x => x.TestResultId,
                        principalTable: "TestResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskModules_SubjectId",
                table: "TaskModules",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswer_TestQuestionVersionId",
                table: "TestAnswer",
                column: "TestQuestionVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestion_SubjectId",
                table: "TestQuestion",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestionVersion_TestQuestionId",
                table: "TestQuestionVersion",
                column: "TestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_StudentId",
                table: "TestResult",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TestStudentAnswer_TestQuestionVersionId",
                table: "TestStudentAnswer",
                column: "TestQuestionVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestStudentAnswer_TestResultId",
                table: "TestStudentAnswer",
                column: "TestResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModules_Subject_SubjectId",
                table: "TaskModules",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModules_Subject_SubjectId",
                table: "TaskModules");

            migrationBuilder.DropTable(
                name: "TestAnswer");

            migrationBuilder.DropTable(
                name: "TestStudentAnswer");

            migrationBuilder.DropTable(
                name: "TestQuestionVersion");

            migrationBuilder.DropTable(
                name: "TestResult");

            migrationBuilder.DropTable(
                name: "TestQuestion");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_TaskModules_SubjectId",
                table: "TaskModules");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TaskModules");
        }
    }
}
