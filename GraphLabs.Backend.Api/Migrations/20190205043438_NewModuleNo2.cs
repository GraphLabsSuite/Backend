using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphLabs.Backend.Api.Migrations
{
    public partial class NewModuleNo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE TaskModules
SET Name='Модуль Лизы',
    Description='Описание',
    Version='1.0'
WHERE Id=2;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
