using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddVillagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Nufus = table.Column<int>(type: "INTEGER", nullable: true),
                    IlkokulOgrenciSayisi = table.Column<int>(type: "INTEGER", nullable: true),
                    OrtaokulOgrenciSayisi = table.Column<int>(type: "INTEGER", nullable: true),
                    LiseOgrenciSayisi = table.Column<int>(type: "INTEGER", nullable: true),
                    UniversiteOgrenciSayisi = table.Column<int>(type: "INTEGER", nullable: true),
                    MuhtarAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MuhtarTelefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villages");
        }
    }
}
