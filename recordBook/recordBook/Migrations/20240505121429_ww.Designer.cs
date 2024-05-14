﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using recordBook.Models;

#nullable disable

namespace recordBook.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240505121429_ww")]
    partial class ww
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("recordBook.Models.Academic_performance", b =>
                {
                    b.Property<int>("ID_Academic_performance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Academic_performance"));

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID_Kind_of_work")
                        .HasColumnType("int");

                    b.Property<int>("ID_Student")
                        .HasColumnType("int");

                    b.Property<int>("ID_Subject")
                        .HasColumnType("int");

                    b.HasKey("ID_Academic_performance");

                    b.ToTable("Academic_performance");
                });

            modelBuilder.Entity("recordBook.Models.Attendance", b =>
                {
                    b.Property<int>("ID_Attendance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Attendance"));

                    b.Property<DateTime>("Date_presence")
                        .HasColumnType("datetime2");

                    b.Property<int>("ID_Student")
                        .HasColumnType("int");

                    b.Property<int>("ID_Subject")
                        .HasColumnType("int");

                    b.Property<bool>("Precense")
                        .HasColumnType("bit");

                    b.HasKey("ID_Attendance");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("recordBook.Models.Curator", b =>
                {
                    b.Property<int>("ID_Curator")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Curator"));

                    b.Property<int>("ID_Login")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Curator");

                    b.ToTable("Curator");
                });

            modelBuilder.Entity("recordBook.Models.Department_worker", b =>
                {
                    b.Property<int>("ID_Department_worker")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Department_worker"));

                    b.Property<int>("ID_Login")
                        .HasColumnType("int");

                    b.Property<string>("Institute_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Job_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Department_worker");

                    b.ToTable("Department_worker");
                });

            modelBuilder.Entity("recordBook.Models.Department_worker_Academic_performance", b =>
                {
                    b.Property<int>("ID_Department_worker_Academic_performance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Department_worker_Academic_performance"));

                    b.Property<int>("ID_Academic_performance")
                        .HasColumnType("int");

                    b.Property<int>("ID_Department_worker")
                        .HasColumnType("int");

                    b.HasKey("ID_Department_worker_Academic_performance");

                    b.ToTable("Department_worker_Academic_performance");
                });

            modelBuilder.Entity("recordBook.Models.Group", b =>
                {
                    b.Property<int>("ID_Group")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Group"));

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Decoding")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Financing_source")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Graduating_department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID_Curator")
                        .HasColumnType("int");

                    b.Property<string>("Name_group")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Group");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("recordBook.Models.Group_Subject", b =>
                {
                    b.Property<int>("ID_Group_Subject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Group_Subject"));

                    b.Property<int>("ID_Group")
                        .HasColumnType("int");

                    b.Property<int>("ID_Subject")
                        .HasColumnType("int");

                    b.HasKey("ID_Group_Subject");

                    b.ToTable("Group_Subject");
                });

            modelBuilder.Entity("recordBook.Models.Kind_of_work", b =>
                {
                    b.Property<int>("ID_Kind_of_work")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Kind_of_work"));

                    b.Property<string>("Title_of_kind")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Kind_of_work");

                    b.ToTable("Kind_of_work");
                });

            modelBuilder.Entity("recordBook.Models.Logins", b =>
                {
                    b.Property<int>("ID_Login")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Login"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Phone")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Login");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("recordBook.Models.LoginsStudent", b =>
                {
                    b.Property<int>("Number_RecordBook")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Number_RecordBook"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Phone")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Number_RecordBook");

                    b.ToTable("LoginsStudents");
                });

            modelBuilder.Entity("recordBook.Models.RatingControl", b =>
                {
                    b.Property<int>("ID_RatingControl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_RatingControl"));

                    b.Property<int>("ID_Student")
                        .HasColumnType("int");

                    b.Property<int>("ID_Subject")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("RatingNumber")
                        .HasColumnType("int");

                    b.Property<int>("Semester")
                        .HasColumnType("int");

                    b.HasKey("ID_RatingControl");

                    b.ToTable("RatingControl");
                });

            modelBuilder.Entity("recordBook.Models.Student", b =>
                {
                    b.Property<int>("ID_Student")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Student"));

                    b.Property<int>("ID_Group")
                        .HasColumnType("int");

                    b.Property<int>("ID_LoginStudent")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Student");

                    b.ToTable("Student", t =>
                        {
                            t.HasTrigger("afterStudentAdd");
                        });
                });

            modelBuilder.Entity("recordBook.Models.Subject", b =>
                {
                    b.Property<int>("ID_Subject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Subject"));

                    b.Property<string>("Name_subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_Subject");

                    b.ToTable("Subject");
                });
#pragma warning restore 612, 618
        }
    }
}