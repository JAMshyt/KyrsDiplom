using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recordBook.Migrations
{
    /// <inheritdoc />
    public partial class ratingControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Curator",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Academic_performance",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "RatingControl",
                columns: table => new
                {
                    IDRatingControl = table.Column<int>(name: "ID_RatingControl", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDStudent = table.Column<int>(name: "ID_Student", type: "int", nullable: false),
                    IDSubject = table.Column<int>(name: "ID_Subject", type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingControl", x => x.IDRatingControl);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RatingControl");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Curator");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Academic_performance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
