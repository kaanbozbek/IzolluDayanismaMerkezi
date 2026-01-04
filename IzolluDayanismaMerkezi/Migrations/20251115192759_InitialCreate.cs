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
                    ToplamTutar = table.Column<decimal>(type: "TEXT", nullable: false),
                    IlkBursVerdigiTarih = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SonBursVerdigiTarih = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BursVermeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Baslik = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ToplantiTuru = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Konum = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
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
                    SicilNumarasi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TCNo = table.Column<string>(type: "TEXT", maxLength: 11, nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Yas = table.Column<int>(type: "INTEGER", nullable: true),
                    IsIzollulu = table.Column<bool>(type: "INTEGER", nullable: false),
                    Koy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UyelikTuru = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UyelikBaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Durum = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IsMutevelli = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsYonetimKurulu = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDenetimKurulu = table.Column<bool>(type: "INTEGER", nullable: false),
                    BursVeriyor = table.Column<bool>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdSoyad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Meslek = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Universite = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Cinsiyet = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Yas = table.Column<int>(type: "INTEGER", nullable: true),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SicilNumarasi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TCNo = table.Column<string>(type: "TEXT", maxLength: 11, nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsIzollulu = table.Column<bool>(type: "INTEGER", nullable: false),
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
                    ToplamAlinanBurs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MezunMu = table.Column<bool>(type: "INTEGER", nullable: false),
                    MezuniyetTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    TranskriptNotu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IBAN = table.Column<string>(type: "TEXT", maxLength: 34, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppVersion = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberScholarshipCommitments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "TranscriptRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    GNO = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    GirisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PdfDosyaYolu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ScholarshipPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommitmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    PaymentMethod = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StudentId1 = table.Column<int>(type: "INTEGER", nullable: true)
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
                        name: "FK_ScholarshipPayments_Students_StudentId1",
                        column: x => x.StudentId1,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_SicilNumarasi",
                table: "Members",
                column: "SicilNumarasi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberScholarshipCommitments_MemberId",
                table: "MemberScholarshipCommitments",
                column: "MemberId");

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
                name: "IX_ScholarshipPayments_StudentId1",
                table: "ScholarshipPayments",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Key",
                table: "Settings",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentMeetingAttendances_MeetingId",
                table: "StudentMeetingAttendances",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMeetingAttendances_StudentId_MeetingId",
                table: "StudentMeetingAttendances",
                columns: new[] { "StudentId", "MeetingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SicilNumarasi",
                table: "Students",
                column: "SicilNumarasi",
                unique: true);

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
                name: "ScholarshipPayments");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StudentMeetingAttendances");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "TranscriptRecords");

            migrationBuilder.DropTable(
                name: "MemberScholarshipCommitments");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
