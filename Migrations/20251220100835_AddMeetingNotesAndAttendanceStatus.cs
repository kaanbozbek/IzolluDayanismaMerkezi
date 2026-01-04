using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingNotesAndAttendanceStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mazeret",
                table: "StudentMeetingAttendances",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentMeetingAttendances",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mazeret",
                table: "MemberMeetingAttendances",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MemberMeetingAttendances",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Meetings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ToplantiNotlari",
                table: "Meetings",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mazeret",
                table: "StudentMeetingAttendances");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentMeetingAttendances");

            migrationBuilder.DropColumn(
                name: "Mazeret",
                table: "MemberMeetingAttendances");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MemberMeetingAttendances");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "ToplantiNotlari",
                table: "Meetings");
        }
    }
}
