using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class RLS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"TaskVariantLogs\" ENABLE ROW LEVEL SECURITY;");
            migrationBuilder.Sql("ALTER TABLE \"TaskVariantLogs\" FORCE ROW LEVEL SECURITY;");
            migrationBuilder.Sql("CREATE POLICY hide_other_students_logs ON \"TaskVariantLogs\" " +
                                 "    USING (current_setting('backend.enableRls') <> '1' OR cast(current_setting('backend.userId') as bigint) = \"StudentId\");");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP POLICY IF EXISTS hide_other_students_logs ON \"TaskVariantLogs\";");
            migrationBuilder.Sql("ALTER TABLE \"TaskVariantLogs\" DISABLE ROW LEVEL SECURITY;");
        }
    }
}
