using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingsFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Baslik = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Konum = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentMeetingAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    MeetingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Katildi = table.Column<bool>(type: "INTEGER", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMeetingAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentMeetingAttendances_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentMeetingAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentMeetingAttendances_MeetingId",
                table: "StudentMeetingAttendances",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMeetingAttendances_StudentId_MeetingId",
                table: "StudentMeetingAttendances",
                columns: new[] { "StudentId", "MeetingId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentMeetingAttendances");

            migrationBuilder.DropTable(
                name: "Meetings");
        }
    }
}
