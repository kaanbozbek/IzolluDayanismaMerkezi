using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingTypeAndTimeRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BitisTarihi",
                table: "Meetings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ToplantiTuru",
                table: "Meetings",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE \"Meetings\" SET \"ToplantiTuru\" = 'Genel Toplantı' WHERE \"ToplantiTuru\" = '' OR \"ToplantiTuru\" IS NULL;");
            migrationBuilder.Sql("UPDATE \"Meetings\" SET \"BitisTarihi\" = CASE WHEN \"Tarih\" IS NOT NULL THEN datetime(\"Tarih\", '+1 hour') ELSE datetime('now', '+1 hour') END WHERE \"BitisTarihi\" <= '1900-01-01T00:00:00';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BitisTarihi",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "ToplantiTuru",
                table: "Meetings");
        }
    }
}
