using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddFirmaToMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Firma",
                table: "Members",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firma",
                table: "Members");
        }
    }
}
