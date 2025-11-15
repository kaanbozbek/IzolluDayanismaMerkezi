using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cinsiyet",
                table: "Students",
                type: "TEXT",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cinsiyet",
                table: "Students");
        }
    }
}
