using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recordBook.Migrations
{
    /// <inheritdoc />
    public partial class InsertDatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_Login",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Login",
                table: "Student");
        }
    }
}
