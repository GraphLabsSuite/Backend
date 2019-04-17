using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class LogPenalty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Penalty",
                table: "TaskVariantLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Penalty",
                table: "TaskVariantLogs");
        }
    }
}
