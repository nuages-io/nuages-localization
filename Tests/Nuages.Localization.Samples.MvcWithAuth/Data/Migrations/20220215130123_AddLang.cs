using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication.Data.Migrations
{
    public partial class AddLang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lang",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lang",
                table: "AspNetUsers");
        }
    }
}
