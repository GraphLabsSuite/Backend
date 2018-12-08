using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskModules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskVariants",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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

            migrationBuilder.CreateIndex(
                name: "IX_TaskVariants_TaskModuleId",
                table: "TaskVariants",
                column: "TaskModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskVariants");

            migrationBuilder.DropTable(
                name: "TaskModules");
        }
    }
}
