using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskModules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    FatherName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskVariants",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    VariantData = table.Column<string>(nullable: true),
                    TaskModuleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskVariants_TaskModules_TaskModuleId",
                        column: x => x.TaskModuleId,
                        principalTable: "TaskModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskVariantLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Action = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    VariantId = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: false),
                    Penalty = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_TaskVariants_TaskModuleId",
                table: "TaskVariants",
                column: "TaskModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Group",
                table: "Users",
                column: "Group");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskVariantLogs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TaskVariants");

            migrationBuilder.DropTable(
                name: "TaskModules");
        }
    }
}
