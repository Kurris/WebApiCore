using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCore.Data.EF.Migrations
{
    public partial class tmp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoJobTasks",
                columns: table => new
                {
                    AutoJobTaskId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
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
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Stars = table.Column<int>(nullable: false),
                    Shits = table.Column<int>(nullable: false),
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
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    GithubUrl = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Creator = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(maxLength: 14, nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(maxLength: 14, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Age", "AvatarUrl", "BlogId", "CreateTime", "Creator", "Email", "Gender", "GithubUrl", "Modifier", "ModifyTime", "Name", "Phone" },
                values: new object[] { 1, 23, "https://avatars3.githubusercontent.com/u/42861557?s=460&u=bea03f68386386ea61fc88c76f27c8db90b509fc&v=4", null, new DateTime(2021, 1, 7, 15, 13, 57, 0, DateTimeKind.Unspecified), "ligy", "Ligy.97@foxmail.com", "Male", "https://github.com/Kurris", null, null, "ligy", "13790166319" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreateTime", "Creator", "Email", "LastLogin", "Modifier", "ModifyTime", "Password", "Phone", "UserName" },
                values: new object[] { 1, new DateTime(2021, 1, 7, 15, 13, 57, 0, DateTimeKind.Unspecified), "System", "Ligy.97@foxmail.com", null, null, null, "546677201aae8c8cb69893a4a30d4464", "13790166319", "ligy" });

            migrationBuilder.CreateIndex(
                name: "IX_AutoJobTasks_JobName_JobGroup",
                table: "AutoJobTasks",
                columns: new[] { "JobName", "JobGroup" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_BlogId",
                table: "Profiles",
                column: "BlogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoJobTasks");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
