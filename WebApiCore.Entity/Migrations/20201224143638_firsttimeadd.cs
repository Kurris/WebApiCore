using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCore.EF.Migrations
{
    public partial class firsttimeadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoJobTasks",
                columns: table => new
                {
                    AutoJobTaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    JobName = table.Column<string>(nullable: true),
                    JobGroup = table.Column<string>(nullable: true),
                    JobType = table.Column<int>(nullable: true),
                    ExecuteName = table.Column<string>(nullable: true),
                    ExecuteArgs = table.Column<string>(nullable: true),
                    JobStatus = table.Column<int>(nullable: true),
                    CronExpression = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    EndTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    NextStartTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoJobTasks", x => x.AutoJobTaskId);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    LastLogin = table.Column<DateTime>(maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    Avatar = table.Column<byte[]>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoJobTasks_JobName_JobGroup",
                table: "AutoJobTasks",
                columns: new[] { "JobName", "JobGroup" },
                unique: true,
                filter: "[JobName] IS NOT NULL AND [JobGroup] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_BlogId",
                table: "Profiles",
                column: "BlogId",
                unique: true,
                filter: "[BlogId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.Sql(@"
if OBJECT_ID('TestA') is not null
drop proc  TestA
go
create proc TestA
as
begin 

INSERT INTO [dbo].[Users]
           ([Creator]
           ,[CreateTime]
           ,[Modifier]
           ,[ModifyTime]
           ,[UserName]
           ,[Password]
           ,[Email]
           ,[MobilePhone]
           ,[LastLogin])
     VALUES ('sys',GETDATE(),'sys',GETDATE(),RAND(),'111','','',GETDATE())
          
end");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoJobTasks");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.Sql(@"
if OBJECT_ID('TestA') is not null
drop proc  TestA");
        }
    }
}
