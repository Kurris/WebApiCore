using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCore.Entity.Migrations
{
    public partial class ModifyHomePage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "HomePage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "HomePage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "HomePage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "HomePage");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "HomePage");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "HomePage");
        }
    }
}
