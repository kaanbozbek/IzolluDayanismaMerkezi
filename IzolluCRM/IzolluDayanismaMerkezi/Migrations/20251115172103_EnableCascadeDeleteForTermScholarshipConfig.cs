using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDeleteForTermScholarshipConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermScholarshipConfigs_Terms_TermId",
                table: "TermScholarshipConfigs");

            migrationBuilder.CreateIndex(
                name: "IX_Terms_Start_Unique",
                table: "Terms",
                column: "Start",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TermScholarshipConfigs_Terms_TermId",
                table: "TermScholarshipConfigs",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermScholarshipConfigs_Terms_TermId",
                table: "TermScholarshipConfigs");

            migrationBuilder.DropIndex(
                name: "IX_Terms_Start_Unique",
                table: "Terms");

            migrationBuilder.AddForeignKey(
                name: "FK_TermScholarshipConfigs_Terms_TermId",
                table: "TermScholarshipConfigs",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
