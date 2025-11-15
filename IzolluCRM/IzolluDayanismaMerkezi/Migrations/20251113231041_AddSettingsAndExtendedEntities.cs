using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingsAndExtendedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfDosyaYolu",
                table: "TranscriptRecords",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KayitliDonemler",
                table: "Students",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MezunOlduguDonem",
                table: "Students",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BursTutari",
                table: "Periods",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "BursVeriyor",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DogumTarihi",
                table: "Members",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "Members",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Koy",
                table: "Members",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SicilNumarasi",
                table: "Members",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TCNo",
                table: "Members",
                type: "TEXT",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UyelikBaslangicTarihi",
                table: "Members",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UyelikTuru",
                table: "Members",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Yas",
                table: "Members",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BursVermeTarihi",
                table: "Donors",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ToplamTutar",
                table: "Donors",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_SicilNumarasi",
                table: "Students",
                column: "SicilNumarasi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_SicilNumarasi",
                table: "Members",
                column: "SicilNumarasi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Key",
                table: "Settings",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Students_SicilNumarasi",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Members_SicilNumarasi",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PdfDosyaYolu",
                table: "TranscriptRecords");

            migrationBuilder.DropColumn(
                name: "KayitliDonemler",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MezunOlduguDonem",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "BursTutari",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "BursVeriyor",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DogumTarihi",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Koy",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SicilNumarasi",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TCNo",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "UyelikBaslangicTarihi",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "UyelikTuru",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Yas",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BursVermeTarihi",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "ToplamTutar",
                table: "Donors");
        }
    }
}
