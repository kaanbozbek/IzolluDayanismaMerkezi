using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

public class MemberScholarshipDetail
{
    public string MemberName { get; set; } = string.Empty;
    public int PledgedCount { get; set; }
    public int RealizedCount { get; set; }
}

public class RoleBasedScholarshipPdf
{
    public string Role { get; set; } = string.Empty;
    public int PledgedCount { get; set; }
    public int RealizedCount { get; set; }
    public double Percentage { get; set; }
}

public class PdfService
{
    public PdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateStudentReport(List<Student> students, string title)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text(text =>
                    {
                        text.Span("İzollu Dayanışma Merkezi\n").FontSize(20).Bold();
                        text.Span(title).FontSize(14);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(5);

                        column.Item().Text($"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(9);
                        column.Item().Text($"Toplam Öğrenci: {students.Count}").FontSize(9).Bold();

                        column.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Ad Soyad").Bold();
                                header.Cell().Element(CellStyle).Text("Üniversite").Bold();
                                header.Cell().Element(CellStyle).Text("Bölüm").Bold();
                                header.Cell().Element(CellStyle).Text("Sınıf").Bold();
                                header.Cell().Element(CellStyle).Text("Dönem").Bold();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.Border(1).Background(Colors.Grey.Lighten2).Padding(5);
                                }
                            });

                            foreach (var student in students)
                            {
                                table.Cell().Element(CellStyle).Text(student.AdSoyad);
                                table.Cell().Element(CellStyle).Text(student.Universite ?? "-");
                                table.Cell().Element(CellStyle).Text(student.Bolum ?? "-");
                                table.Cell().Element(CellStyle).Text(student.Sinif.ToString());
                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.Border(1).BorderColor(Colors.Grey.Lighten1).Padding(5);
                                }
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return document.GeneratePdf();
    }

    public byte[] GenerateMeetingPresentation(
        int totalStudents,
        int activeScholarships,
        decimal totalAmount,
        Dictionary<string, decimal> sectorDistribution,
        Dictionary<string, int> universityDistribution)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                // Kapak sayfası
                page.Content()
                    .AlignCenter()
                    .AlignMiddle()
                    .Column(column =>
                    {
                        column.Item().Text("İzollu Dayanışma Merkezi").FontSize(36).Bold();
                        column.Item().PaddingTop(20).Text("Burs Programı Toplantı Sunumu").FontSize(24);
                        column.Item().PaddingTop(40).Text($"{DateTime.Now:MMMM yyyy}").FontSize(18);
                    });
            });

            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(14));

                page.Header()
                    .Text("Özet Bilgiler").FontSize(24).Bold();

                page.Content()
                    .PaddingTop(2, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Border(2).BorderColor(Colors.Blue.Medium).Padding(20)
                                .Column(col =>
                                {
                                    col.Item().Text("Toplam Öğrenci").FontSize(16);
                                    col.Item().PaddingTop(10).Text(totalStudents.ToString()).FontSize(32).Bold();
                                });

                            row.RelativeItem().Border(2).BorderColor(Colors.Green.Medium).Padding(20)
                                .Column(col =>
                                {
                                    col.Item().Text("Aktif Burs Alan").FontSize(16);
                                    col.Item().PaddingTop(10).Text(activeScholarships.ToString()).FontSize(32).Bold();
                                });

                            row.RelativeItem().Border(2).BorderColor(Colors.Orange.Medium).Padding(20)
                                .Column(col =>
                                {
                                    col.Item().Text("Toplam Aylık Burs").FontSize(16);
                                    col.Item().PaddingTop(10).Text($"{totalAmount:N0} ₺").FontSize(32).Bold();
                                });
                        });

                        if (sectorDistribution.Any())
                        {
                            column.Item().PaddingTop(20).Text("Sektör Bazlı Dağılım").FontSize(18).Bold();
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                foreach (var item in sectorDistribution.OrderByDescending(x => x.Value))
                                {
                                    table.Cell().Padding(5).Text(item.Key);
                                    table.Cell().Padding(5).AlignRight().Text($"{item.Value:N0} ₺");
                                }
                            });
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return document.GeneratePdf();
    }

    public byte[] GenerateStudentsPdfReport(List<Student> students)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("İzollu Dayanışma Merkezi\n").FontSize(18).Bold().FontColor(Colors.Orange.Darken2);
                        text.Span("Öğrenci Listesi Raporu\n").FontSize(14).FontColor(Colors.Grey.Darken1);
                        text.Span($"{DateTime.Now:dd MMMM yyyy HH:mm}").FontSize(9).FontColor(Colors.Grey.Medium);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Item().Text($"Toplam Öğrenci: {students.Count}").FontSize(10).Bold();
                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Ad Soyad").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Üniversite").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Bölüm").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Sınıf").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Burs").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Telefon").Bold().FontSize(8);
                            });

                            foreach (var student in students)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(student.AdSoyad).FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(student.Universite ?? "-").FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(student.Bolum ?? "-").FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(student.Sinif?.ToString() ?? "-").FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(student.AktifBursMu ? "✓" : "-").FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(student.Telefon ?? "-").FontSize(8);
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Sayfa ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    public byte[] GenerateMembersPdfReport(List<Member> members)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("İzollu Dayanışma Merkezi\n").FontSize(18).Bold().FontColor(Colors.Orange.Darken2);
                        text.Span("Üye Listesi Raporu\n").FontSize(14).FontColor(Colors.Grey.Darken1);
                        text.Span($"{DateTime.Now:dd MMMM yyyy HH:mm}").FontSize(9).FontColor(Colors.Grey.Medium);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Item().Text($"Toplam Üye: {members.Count}").FontSize(10).Bold();
                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Ad Soyad").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Rol").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Telefon").Bold().FontSize(8);
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Email").Bold().FontSize(8);
                            });

                            foreach (var member in members)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(member.AdSoyad).FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(member.UyelikTuru ?? "-").FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(member.Telefon ?? "-").FontSize(8);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(member.Email ?? "-").FontSize(8);
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Sayfa ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    public byte[] GenerateComprehensiveReport(
        int totalStudents, 
        int activeScholarships, 
        int graduatedCount,
        int totalDonors,
        int totalMembers,
        decimal totalAmount,
        string activePeriod,
        decimal periodBursTutari,
        int pledgedCount,
        int realizedCount,
        int totalAidsCount,
        int villageCount,
        Dictionary<string, int> universityData,
        List<MemberScholarshipDetail> memberScholarshipDetails,
        Dictionary<string, int> genderDistribution,
        Dictionary<string, int> malatyaLocationDistribution,
        List<MemberScholarshipDetail> topDonors,
        List<MemberScholarshipDetail> unfulfilledCommitments,
        Dictionary<string, int> villageAidData,
        List<RoleBasedScholarshipPdf> roleBasedScholarshipData)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("İzollu Dayanışma Merkezi\n").FontSize(22).Bold().FontColor(Colors.Orange.Darken2);
                        text.Span("Kapsamlı Analiz Raporu\n").FontSize(16).FontColor(Colors.Grey.Darken1);
                        text.Span($"Rapor Tarihi: {DateTime.Now:dd MMMM yyyy HH:mm}").FontSize(10).FontColor(Colors.Grey.Medium);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(15);

                        // Genel İstatistikler - Tüm Dashboard Verileri
                        column.Item().Background(Colors.Orange.Lighten4).Padding(10).Column(col =>
                        {
                            col.Item().Text("GENEL İSTATİSTİKLER").FontSize(14).Bold().FontColor(Colors.Orange.Darken3);
                            col.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text($"Aktif Dönem: {activePeriod}").FontSize(11);
                                    c.Item().Text($"Toplam Öğrenci: {totalStudents}").FontSize(11);
                                    c.Item().Text($"Burs Alan Öğrenci: {activeScholarships}").FontSize(11);
                                    c.Item().Text($"Mezun Öğrenci: {graduatedCount}").FontSize(11);
                                    c.Item().Text($"İş Adamı Sayısı: {totalDonors}").FontSize(11);
                                    c.Item().Text($"Üye Sayısı: {totalMembers}").FontSize(11);
                                });
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text($"Taahhüt Edilen Burs Tutarı: {periodBursTutari:N2} ₺").FontSize(11);
                                    c.Item().Text($"Toplam Burs Tutarı: {totalAmount:N2} ₺").FontSize(11);
                                    c.Item().Text($"Taahhüt Edilen Burs Sayısı: {pledgedCount}").FontSize(11);
                                    c.Item().Text($"Gerçekleşen Burs Sayısı: {realizedCount}").FontSize(11);
                                    c.Item().Text($"Toplam Yardım Sayısı: {totalAidsCount}").FontSize(11);
                                    c.Item().Text($"Köy Sayısı: {villageCount}").FontSize(11);
                                });
                            });
                        });

                        // Burs Taahhüt Özeti
                        var totalPledged = memberScholarshipDetails.Sum(m => m.PledgedCount);
                        var totalRealized = memberScholarshipDetails.Sum(m => m.RealizedCount);
                        var realizationRate = totalPledged > 0 ? (totalRealized * 100.0 / totalPledged) : 0;
                        
                        column.Item().PaddingTop(10).Background(Colors.Purple.Lighten4).Padding(10).Column(col =>
                        {
                            col.Item().Text("BURS TAAHHÜT ÖZETİ").FontSize(14).Bold().FontColor(Colors.Purple.Darken3);
                            col.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem().Border(1).BorderColor(Colors.Purple.Medium).Padding(10).Column(c =>
                                {
                                    c.Item().Text("Toplam Taahhüt").FontSize(10);
                                    c.Item().Text($"{totalPledged} Burs").FontSize(16).Bold().FontColor(Colors.Purple.Darken2);
                                });
                                row.RelativeItem().Border(1).BorderColor(Colors.Green.Medium).Padding(10).Column(c =>
                                {
                                    c.Item().Text("Gerçekleşen").FontSize(10);
                                    c.Item().Text($"{totalRealized} Burs").FontSize(16).Bold().FontColor(Colors.Green.Darken2);
                                });
                                row.RelativeItem().Border(1).BorderColor(Colors.Blue.Medium).Padding(10).Column(c =>
                                {
                                    c.Item().Text("Gerçekleşme Oranı").FontSize(10);
                                    c.Item().Text($"%{realizationRate:F1}").FontSize(16).Bold().FontColor(Colors.Blue.Darken2);
                                });
                            });
                        });

                        // Cinsiyet Dağılımı
                        if (genderDistribution != null && genderDistribution.Any())
                        {
                            column.Item().Text("CİNSİYETE GÖRE DAĞILIM").FontSize(12).Bold();
                            column.Item().Row(row =>
                            {
                                foreach (var item in genderDistribution)
                                {
                                    var percentage = totalStudents > 0 ? (item.Value * 100.0 / totalStudents) : 0;
                                    row.RelativeItem().Border(2).BorderColor(item.Key == "Erkek" ? Colors.Blue.Medium : Colors.Pink.Medium).Padding(10).Column(col =>
                                    {
                                        col.Item().Text(item.Key).FontSize(11).Bold();
                                        col.Item().Text(item.Value.ToString()).FontSize(20).Bold();
                                        col.Item().Text($"%{percentage:F1}").FontSize(10);
                                        // Görsel bar gösterimi
                                        col.Item().PaddingTop(5).Height(8).Background(Colors.Grey.Lighten3).Row(barRow =>
                                        {
                                            barRow.RelativeItem((float)percentage).Background(item.Key == "Erkek" ? Colors.Blue.Medium : Colors.Pink.Medium);
                                            barRow.RelativeItem((float)(100 - percentage)).Background(Colors.Grey.Lighten3);
                                        });
                                    });
                                }
                            });
                        }

                        // Malatya Konum Dağılımı
                        if (malatyaLocationDistribution != null && malatyaLocationDistribution.Any())
                        {
                            column.Item().PaddingTop(10).Text("MALATYA KONUMUNA GÖRE DAĞILIM").FontSize(12).Bold();
                            column.Item().Row(row =>
                            {
                                foreach (var item in malatyaLocationDistribution)
                                {
                                    var percentage = totalStudents > 0 ? (item.Value * 100.0 / totalStudents) : 0;
                                    row.RelativeItem().Border(2).BorderColor(item.Key == "Malatya İçi" ? Colors.Green.Medium : Colors.Orange.Medium).Padding(10).Column(col =>
                                    {
                                        col.Item().Text(item.Key).FontSize(11).Bold();
                                        col.Item().Text(item.Value.ToString()).FontSize(20).Bold();
                                        col.Item().Text($"%{percentage:F1}").FontSize(10);
                                        // Görsel bar gösterimi
                                        col.Item().PaddingTop(5).Height(8).Background(Colors.Grey.Lighten3).Row(barRow =>
                                        {
                                            barRow.RelativeItem((float)percentage).Background(item.Key == "Malatya İçi" ? Colors.Green.Medium : Colors.Orange.Medium);
                                            barRow.RelativeItem((float)(100 - percentage)).Background(Colors.Grey.Lighten3);
                                        });
                                    });
                                }
                            });
                        }

                        // Üniversite Dağılımı
                        if (universityData.Any())
                        {
                            column.Item().PaddingTop(10).Text("ÜNİVERSİTEYE GÖRE ÖĞRENCİ DAĞILIMI (İLK 10)").FontSize(12).Bold();
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Üniversite").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Öğrenci").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Oran").Bold();
                                });

                                foreach (var item in universityData.Take(10))
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Key);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Value.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"%{(item.Value * 100.0 / totalStudents):F1}");
                                }
                            });
                        }

                        // Köylere Göre Yardım Dağılımı
                        if (villageAidData != null && villageAidData.Any())
                        {
                            column.Item().PageBreak();
                            column.Item().Background(Colors.Teal.Lighten4).Padding(10).Column(col =>
                            {
                                col.Item().Text("KÖYLERE GÖRE YARDIM DAĞILIMI").FontSize(12).Bold().FontColor(Colors.Teal.Darken3);
                            });
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Köy").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Yardım Sayısı").Bold();
                                });

                                foreach (var item in villageAidData.OrderByDescending(v => v.Value))
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Key);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.Value}");
                                }
                            });
                        }

                        // Rol Bazlı Burs Dağılımı
                        if (roleBasedScholarshipData != null && roleBasedScholarshipData.Any())
                        {
                            column.Item().PaddingTop(15).Background(Colors.Indigo.Lighten4).Padding(10).Column(col =>
                            {
                                col.Item().Text("ROL BAZLI BURS DAĞILIMI").FontSize(12).Bold().FontColor(Colors.Indigo.Darken3);
                            });
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Rol").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Taahhüt").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Gerçekleşen").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Yüzde").Bold();
                                });

                                foreach (var item in roleBasedScholarshipData)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Role);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.PledgedCount}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.RealizedCount}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"%{item.Percentage:F1}");
                                }
                            });
                        }

                        // En Çok Burs Verenler
                        if (topDonors != null && topDonors.Any())
                        {
                            column.Item().PageBreak();
                            column.Item().Background(Colors.Green.Lighten4).Padding(10).Column(col =>
                            {
                                col.Item().Text("EN ÇOK BURS VERENLER (İLK 10)").FontSize(12).Bold().FontColor(Colors.Green.Darken3);
                            });
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Sıra").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Üye Adı").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Taahhüt").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Gerçekleşen").Bold();
                                });

                                int rank = 1;
                                foreach (var item in topDonors.Take(10))
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{rank++}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.MemberName);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.PledgedCount}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.RealizedCount}").FontColor(Colors.Green.Darken2).Bold();
                                }
                            });
                        }

                        // Taahhüdünü Yerine Getirmeyenler
                        if (unfulfilledCommitments != null && unfulfilledCommitments.Any())
                        {
                            column.Item().PaddingTop(15).Background(Colors.Red.Lighten4).Padding(10).Column(col =>
                            {
                                col.Item().Text("TAAHHÜDÜNÜ YERİNE GETİRMEYENLER").FontSize(12).Bold().FontColor(Colors.Red.Darken3);
                            });
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Üye Adı").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Taahhüt").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Gerçekleşen").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Eksik").Bold();
                                });

                                foreach (var item in unfulfilledCommitments)
                                {
                                    var deficit = item.PledgedCount - item.RealizedCount;
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.MemberName);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.PledgedCount}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.RealizedCount}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{deficit}").FontColor(Colors.Red.Darken2).Bold();
                                }
                            });
                        }

                        // Üyelerin Verdiği Burs Dağılımı
                        if (memberScholarshipDetails.Any())
                        {
                            column.Item().PageBreak();
                            column.Item().Text("TÜM ÜYELERİN BURS DAĞILIMI").FontSize(12).Bold();
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Üye Adı").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Taahhüt Edilen").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Gerçekleşen").Bold();
                                });

                                foreach (var item in memberScholarshipDetails.OrderByDescending(x => x.PledgedCount))
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.MemberName);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.PledgedCount}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{item.RealizedCount}");
                                }
                            });
                        }

                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("İzollu Dayanışma Merkezi - ").FontSize(9).FontColor(Colors.Grey.Medium);
                        text.CurrentPageNumber().FontSize(9);
                        text.Span(" / ").FontSize(9);
                        text.TotalPages().FontSize(9);
                    });
            });
        });

        return document.GeneratePdf();
    }
}