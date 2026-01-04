using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <inheritdoc />
    public partial class AddAidsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    RefereEden = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IsIzollulu = table.Column<bool>(type: "INTEGER", nullable: false),
                    Koy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Sehir = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notlar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aids", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aids");
        }
    }
}
