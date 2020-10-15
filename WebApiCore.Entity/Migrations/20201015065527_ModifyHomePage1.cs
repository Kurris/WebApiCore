using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCore.Entity.Migrations
{
    public partial class ModifyHomePage1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomePage_Users_UserId",
                table: "HomePage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomePage",
                table: "HomePage");

            migrationBuilder.RenameTable(
                name: "HomePage",
                newName: "HomePages");

            migrationBuilder.RenameIndex(
                name: "IX_HomePage_UserId",
                table: "HomePages",
                newName: "IX_HomePages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomePages",
                table: "HomePages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HomePages_Users_UserId",
                table: "HomePages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomePages_Users_UserId",
                table: "HomePages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomePages",
                table: "HomePages");

            migrationBuilder.RenameTable(
                name: "HomePages",
                newName: "HomePage");

            migrationBuilder.RenameIndex(
                name: "IX_HomePages_UserId",
                table: "HomePage",
                newName: "IX_HomePage_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomePage",
                table: "HomePage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HomePage_Users_UserId",
                table: "HomePage",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
