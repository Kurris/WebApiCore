﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiCore.EF;

namespace WebApiCore.EF.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20201224143638_firsttimeadd")]
    partial class firsttimeadd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApiCore.Entity.BlogInfos.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BlogId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("WebApiCore.Entity.BlogInfos.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BlogId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PostId");

                    b.HasIndex("BlogId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("WebApiCore.Entity.BlogInfos.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("BlogId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProfileId");

                    b.HasIndex("BlogId")
                        .IsUnique()
                        .HasFilter("[BlogId] IS NOT NULL");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("WebApiCore.Entity.SystemManage.AutoJobTask", b =>
                {
                    b.Property<int>("AutoJobTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CronExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("ExecuteArgs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExecuteName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobGroup")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("JobName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("JobStatus")
                        .HasColumnType("int");

                    b.Property<int?>("JobType")
                        .HasColumnType("int");

                    b.Property<string>("Modifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<DateTime?>("NextStartTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.HasKey("AutoJobTaskId");

                    b.HasIndex("JobName", "JobGroup")
                        .IsUnique()
                        .HasFilter("[JobName] IS NOT NULL AND [JobGroup] IS NOT NULL");

                    b.ToTable("AutoJobTasks");
                });

            modelBuilder.Entity("WebApiCore.Entity.SystemManage.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("MobilePhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime2")
                        .HasMaxLength(14);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApiCore.Entity.BlogInfos.Post", b =>
                {
                    b.HasOne("WebApiCore.Entity.BlogInfos.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApiCore.Entity.BlogInfos.Profile", b =>
                {
                    b.HasOne("WebApiCore.Entity.BlogInfos.Blog", "Blog")
                        .WithOne("Profile")
                        .HasForeignKey("WebApiCore.Entity.BlogInfos.Profile", "BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
