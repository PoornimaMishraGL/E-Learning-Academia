using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_GL.Migrations.ApplicationDb
{
    public partial class phone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNum",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNum",
                table: "AspNetUsers");
        }
    }
}
