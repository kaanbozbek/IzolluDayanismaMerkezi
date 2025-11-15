using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KullaniciAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IslemTipi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Detay = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdSoyad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Meslek = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Sektor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Firma = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Sirket = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    BursAdedi = table.Column<int>(type: "INTEGER", nullable: false),
                    BirimBursTutari = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IlkBursVerdigiTarih = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SonBursVerdigiTarih = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdSoyad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Meslek = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsMutevelli = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsYonetimKurulu = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDenetimKurulu = table.Column<bool>(type: "INTEGER", nullable: false),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdSoyad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Meslek = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Universite = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Yas = table.Column<int>(type: "INTEGER", nullable: true),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Donem = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    SicilNumarasi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TCNo = table.Column<string>(type: "TEXT", maxLength: 11, nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Koy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EbeveynAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EbeveynTelefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Bolum = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Sinif = table.Column<int>(type: "INTEGER", nullable: true),
                    Referans = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    BagisciAdi = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    BursBaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BursBitisTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AktifBursMu = table.Column<bool>(type: "INTEGER", nullable: false),
                    AylikTutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToplamAlinanBurs = table.Column<decimal>(type: "TEXT", nullable: false),
                    MezunMu = table.Column<bool>(type: "INTEGER", nullable: false),
                    MezuniyetTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranscriptRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    GNO = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    GirisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscriptRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscriptRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptRecords_StudentId",
                table: "TranscriptRecords",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "TranscriptRecords");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
