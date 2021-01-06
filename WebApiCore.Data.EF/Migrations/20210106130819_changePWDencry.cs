using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCore.Data.EF.Migrations
{
    public partial class changePWDencry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreateTime", "Creator", "Password" },
                values: new object[] { new DateTime(2021, 1, 6, 21, 8, 18, 0, DateTimeKind.Unspecified), "System", "546677201aae8c8cb69893a4a30d4464" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreateTime", "Creator", "Password" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "zxc111" });
        }
    }
}
