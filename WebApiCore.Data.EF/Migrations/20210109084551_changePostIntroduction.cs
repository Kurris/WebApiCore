using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCore.Data.EF.Migrations
{
    public partial class changePostIntroduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                  name: "Instruction",
                     table: "Posts",
                     newName: "Introduction"
                );

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: 1,
                column: "CreateTime",
                value: new DateTime(2021, 1, 9, 16, 45, 50, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreateTime",
                value: new DateTime(2021, 1, 9, 16, 45, 50, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Instruction",
                table: "Posts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: 1,
                column: "CreateTime",
                value: new DateTime(2021, 1, 8, 15, 6, 48, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreateTime",
                value: new DateTime(2021, 1, 8, 15, 6, 48, 0, DateTimeKind.Unspecified));
        }
    }
}
