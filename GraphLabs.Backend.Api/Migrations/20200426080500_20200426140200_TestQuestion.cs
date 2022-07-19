using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class _20200426140200_TestQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModules_Subject_SubjectId",
                table: "TaskModules");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswer_TestQuestionVersion_TestQuestionVersionId",
                table: "TestAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestion_Subject_SubjectId",
                table: "TestQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestionVersion_TestQuestion_TestQuestionId",
                table: "TestQuestionVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResult_Users_StudentId",
                table: "TestResult");

            migrationBuilder.DropForeignKey(
                name: "FK_TestStudentAnswer_TestQuestionVersion_TestQuestionVersionId",
                table: "TestStudentAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_TestStudentAnswer_TestResult_TestResultId",
                table: "TestStudentAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestStudentAnswer",
                table: "TestStudentAnswer");

            migrationBuilder.DropIndex(
                name: "IX_TestStudentAnswer_TestQuestionVersionId",
                table: "TestStudentAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestResult",
                table: "TestResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestionVersion",
                table: "TestQuestionVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestAnswer",
                table: "TestAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "TestQuestionVersionId",
                table: "TestStudentAnswer");

            migrationBuilder.RenameTable(
                name: "TestStudentAnswer",
                newName: "TestStudentAnswers");

            migrationBuilder.RenameTable(
                name: "TestResult",
                newName: "TestResults");

            migrationBuilder.RenameTable(
                name: "TestQuestionVersion",
                newName: "TestQuestionVersions");

            migrationBuilder.RenameTable(
                name: "TestQuestion",
                newName: "TestQuestions");

            migrationBuilder.RenameTable(
                name: "TestAnswer",
                newName: "TestAnswers");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameIndex(
                name: "IX_TestStudentAnswer_TestResultId",
                table: "TestStudentAnswers",
                newName: "IX_TestStudentAnswers_TestResultId");

            migrationBuilder.RenameIndex(
                name: "IX_TestResult_StudentId",
                table: "TestResults",
                newName: "IX_TestResults_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestionVersion_TestQuestionId",
                table: "TestQuestionVersions",
                newName: "IX_TestQuestionVersions_TestQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestion_SubjectId",
                table: "TestQuestions",
                newName: "IX_TestQuestions_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TestAnswer_TestQuestionVersionId",
                table: "TestAnswers",
                newName: "IX_TestAnswers_TestQuestionVersionId");

            migrationBuilder.AlterColumn<long>(
                name: "SubjectId",
                table: "TaskModules",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TestResultId",
                table: "TestStudentAnswers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "StudentId",
                table: "TestResults",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TestQuestionId",
                table: "TestQuestionVersions",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SubjectId",
                table: "TestQuestions",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TestQuestionVersionId",
                table: "TestAnswers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestStudentAnswers",
                table: "TestStudentAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestResults",
                table: "TestResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestionVersions",
                table: "TestQuestionVersions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestions",
                table: "TestQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestAnswers",
                table: "TestAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TestStudentAnswers_AnswerId",
                table: "TestStudentAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_DateTime",
                table: "TestResults",
                column: "DateTime");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModules_Subjects_SubjectId",
                table: "TaskModules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswers_TestQuestionVersions_TestQuestionVersionId",
                table: "TestAnswers",
                column: "TestQuestionVersionId",
                principalTable: "TestQuestionVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Subjects_SubjectId",
                table: "TestQuestions",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestionVersions_TestQuestions_TestQuestionId",
                table: "TestQuestionVersions",
                column: "TestQuestionId",
                principalTable: "TestQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Users_StudentId",
                table: "TestResults",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestStudentAnswers_TestQuestionVersions_AnswerId",
                table: "TestStudentAnswers",
                column: "AnswerId",
                principalTable: "TestQuestionVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestStudentAnswers_TestResults_TestResultId",
                table: "TestStudentAnswers",
                column: "TestResultId",
                principalTable: "TestResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModules_Subjects_SubjectId",
                table: "TaskModules");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswers_TestQuestionVersions_TestQuestionVersionId",
                table: "TestAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Subjects_SubjectId",
                table: "TestQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestionVersions_TestQuestions_TestQuestionId",
                table: "TestQuestionVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Users_StudentId",
                table: "TestResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TestStudentAnswers_TestQuestionVersions_AnswerId",
                table: "TestStudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestStudentAnswers_TestResults_TestResultId",
                table: "TestStudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestStudentAnswers",
                table: "TestStudentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_TestStudentAnswers_AnswerId",
                table: "TestStudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestResults",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_DateTime",
                table: "TestResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestionVersions",
                table: "TestQuestionVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestions",
                table: "TestQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestAnswers",
                table: "TestAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.RenameTable(
                name: "TestStudentAnswers",
                newName: "TestStudentAnswer");

            migrationBuilder.RenameTable(
                name: "TestResults",
                newName: "TestResult");

            migrationBuilder.RenameTable(
                name: "TestQuestionVersions",
                newName: "TestQuestionVersion");

            migrationBuilder.RenameTable(
                name: "TestQuestions",
                newName: "TestQuestion");

            migrationBuilder.RenameTable(
                name: "TestAnswers",
                newName: "TestAnswer");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameIndex(
                name: "IX_TestStudentAnswers_TestResultId",
                table: "TestStudentAnswer",
                newName: "IX_TestStudentAnswer_TestResultId");

            migrationBuilder.RenameIndex(
                name: "IX_TestResults_StudentId",
                table: "TestResult",
                newName: "IX_TestResult_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestionVersions_TestQuestionId",
                table: "TestQuestionVersion",
                newName: "IX_TestQuestionVersion_TestQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestions_SubjectId",
                table: "TestQuestion",
                newName: "IX_TestQuestion_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TestAnswers_TestQuestionVersionId",
                table: "TestAnswer",
                newName: "IX_TestAnswer_TestQuestionVersionId");

            migrationBuilder.AlterColumn<long>(
                name: "SubjectId",
                table: "TaskModules",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TestResultId",
                table: "TestStudentAnswer",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "TestQuestionVersionId",
                table: "TestStudentAnswer",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "StudentId",
                table: "TestResult",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TestQuestionId",
                table: "TestQuestionVersion",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "SubjectId",
                table: "TestQuestion",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TestQuestionVersionId",
                table: "TestAnswer",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestStudentAnswer",
                table: "TestStudentAnswer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestResult",
                table: "TestResult",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestionVersion",
                table: "TestQuestionVersion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestAnswer",
                table: "TestAnswer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TestStudentAnswer_TestQuestionVersionId",
                table: "TestStudentAnswer",
                column: "TestQuestionVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModules_Subject_SubjectId",
                table: "TaskModules",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswer_TestQuestionVersion_TestQuestionVersionId",
                table: "TestAnswer",
                column: "TestQuestionVersionId",
                principalTable: "TestQuestionVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestion_Subject_SubjectId",
                table: "TestQuestion",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestionVersion_TestQuestion_TestQuestionId",
                table: "TestQuestionVersion",
                column: "TestQuestionId",
                principalTable: "TestQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestResult_Users_StudentId",
                table: "TestResult",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestStudentAnswer_TestQuestionVersion_TestQuestionVersionId",
                table: "TestStudentAnswer",
                column: "TestQuestionVersionId",
                principalTable: "TestQuestionVersion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestStudentAnswer_TestResult_TestResultId",
                table: "TestStudentAnswer",
                column: "TestResultId",
                principalTable: "TestResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
