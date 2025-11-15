using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicYearToCommitments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "MemberScholarshipCommitments",
                type: "TEXT",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "MemberScholarshipCommitments");
        }
    }
}
