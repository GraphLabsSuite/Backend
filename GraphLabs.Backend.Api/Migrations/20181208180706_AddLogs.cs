using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class AddLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskVariantLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    VariantId = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskVariantLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskVariantLogs_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskVariantLogs_TaskVariants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "TaskVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskVariantLogs_DateTime",
                table: "TaskVariantLogs",
                column: "DateTime");

            migrationBuilder.CreateIndex(
                name: "IX_TaskVariantLogs_StudentId",
                table: "TaskVariantLogs",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskVariantLogs_VariantId",
                table: "TaskVariantLogs",
                column: "VariantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskVariantLogs");
        }
    }
}
