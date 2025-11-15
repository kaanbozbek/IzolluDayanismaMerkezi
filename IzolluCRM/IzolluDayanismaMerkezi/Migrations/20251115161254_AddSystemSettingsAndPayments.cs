using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemSettingsAndPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScholarshipPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommitmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    TermId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    PaymentMethod = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScholarshipPayments_MemberScholarshipCommitments_CommitmentId",
                        column: x => x.CommitmentId,
                        principalTable: "MemberScholarshipCommitments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScholarshipPayments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScholarshipPayments_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActiveTermId = table.Column<int>(type: "INTEGER", nullable: true),
                    AppVersion = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSettings_Terms_ActiveTermId",
                        column: x => x.ActiveTermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipPayments_CommitmentId",
                table: "ScholarshipPayments",
                column: "CommitmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipPayments_PaymentDate",
                table: "ScholarshipPayments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipPayments_StudentId",
                table: "ScholarshipPayments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipPayments_TermId",
                table: "ScholarshipPayments",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipPayments_TermId_PaymentDate",
                table: "ScholarshipPayments",
                columns: new[] { "TermId", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_ActiveTermId",
                table: "SystemSettings",
                column: "ActiveTermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScholarshipPayments");

            migrationBuilder.DropTable(
                name: "SystemSettings");
        }
    }
}
