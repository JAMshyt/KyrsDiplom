using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recordBook.Migrations
{
    /// <inheritdoc />
    public partial class _12312 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Group",
                table: "Curator");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Logins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Graduating_department",
                table: "Group",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ID_Curator",
                table: "Group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Institute_title",
                table: "Department_worker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Job_title",
                table: "Department_worker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Graduating_department",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "ID_Curator",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "Institute_title",
                table: "Department_worker");

            migrationBuilder.DropColumn(
                name: "Job_title",
                table: "Department_worker");

            migrationBuilder.AddColumn<int>(
                name: "ID_Group",
                table: "Curator",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
