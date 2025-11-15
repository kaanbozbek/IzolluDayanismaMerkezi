using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberScholarshipCommitment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.CreateTable(
                name: "MemberScholarshipCommitments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    TermId = table.Column<int>(type: "INTEGER", nullable: false),
                    PledgedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    GivenCount = table.Column<int>(type: "INTEGER", nullable: false),
                    YearlyAmountPerScholarship = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberScholarshipCommitments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberScholarshipCommitments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberScholarshipCommitments_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberScholarshipCommitments_MemberId_TermId_Unique",
                table: "MemberScholarshipCommitments",
                columns: new[] { "MemberId", "TermId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberScholarshipCommitments_TermId",
                table: "MemberScholarshipCommitments",
                column: "TermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberScholarshipCommitments");

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BursTutari = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });
        }
    }
}
