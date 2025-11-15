using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddTermBasedSnapshotModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    End = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberTermRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    TermId = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBoardOfTrustees = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsExecutiveBoard = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAuditCommittee = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsProvidingScholarship = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    RoleStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RoleEndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberTermRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberTermRoles_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberTermRoles_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    TermId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsGraduated = table.Column<bool>(type: "INTEGER", nullable: false),
                    MonthlyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScholarshipStart = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ScholarshipEnd = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Gpa = table.Column<double>(type: "REAL", nullable: true),
                    ClassLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    DonorName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Department = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    University = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    TotalScholarshipReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TermNotes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TranscriptNotes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTerms_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTerms_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberTermRoles_MemberId",
                table: "MemberTermRoles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberTermRoles_TermId",
                table: "MemberTermRoles",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberTermRoles_TermId_IsActive",
                table: "MemberTermRoles",
                columns: new[] { "TermId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_MemberTermRoles_TermId_IsExecutiveBoard",
                table: "MemberTermRoles",
                columns: new[] { "TermId", "IsExecutiveBoard" });

            migrationBuilder.CreateIndex(
                name: "IX_MemberTermRoles_TermId_IsProvidingScholarship",
                table: "MemberTermRoles",
                columns: new[] { "TermId", "IsProvidingScholarship" });

            migrationBuilder.CreateIndex(
                name: "IX_MemberTermRoles_TermId_MemberId",
                table: "MemberTermRoles",
                columns: new[] { "TermId", "MemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTerms_StudentId",
                table: "StudentTerms",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTerms_TermId",
                table: "StudentTerms",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTerms_TermId_IsActive",
                table: "StudentTerms",
                columns: new[] { "TermId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTerms_TermId_IsGraduated",
                table: "StudentTerms",
                columns: new[] { "TermId", "IsGraduated" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTerms_TermId_StudentId",
                table: "StudentTerms",
                columns: new[] { "TermId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terms_IsActive",
                table: "Terms",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Terms_Start_End",
                table: "Terms",
                columns: new[] { "Start", "End" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberTermRoles");

            migrationBuilder.DropTable(
                name: "StudentTerms");

            migrationBuilder.DropTable(
                name: "Terms");
        }
    }
}
