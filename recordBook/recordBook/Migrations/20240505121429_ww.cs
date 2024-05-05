using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recordBook.Migrations
{
    /// <inheritdoc />
    public partial class ww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Group",
                table: "Curator");

            migrationBuilder.RenameColumn(
                name: "ID_Login",
                table: "Student",
                newName: "ID_LoginStudent");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Logins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Financing_source",
                table: "Group",
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

            migrationBuilder.CreateTable(
                name: "LoginsStudents",
                columns: table => new
                {
                    NumberRecordBook = table.Column<int>(name: "Number_RecordBook", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginsStudents", x => x.NumberRecordBook);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginsStudents");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Financing_source",
                table: "Group");

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

            migrationBuilder.RenameColumn(
                name: "ID_LoginStudent",
                table: "Student",
                newName: "ID_Login");

            migrationBuilder.AddColumn<int>(
                name: "ID_Group",
                table: "Curator",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
