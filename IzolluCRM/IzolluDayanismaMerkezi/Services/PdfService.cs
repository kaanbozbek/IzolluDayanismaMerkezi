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

    public byte[] GenerateComprehensiveReport(
        int totalStudents, 
        int activeScholarships, 
        int graduatedCount,
        int totalDonors,
        int totalMembers,
        decimal totalAmount,
        string activePeriod,
        decimal periodBursTutari,
        Dictionary<string, int> universityData,
        List<MemberScholarshipDetail> memberScholarshipDetails)
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

                        // Genel İstatistikler
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
                                });
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text($"İş Adamı Sayısı: {totalDonors}").FontSize(11);
                                    c.Item().Text($"Üye Sayısı: {totalMembers}").FontSize(11);
                                    c.Item().Text($"Dönem Burs Tutarı: {periodBursTutari:N2} ₺").FontSize(11);
                                    c.Item().Text($"Aylık Toplam: {totalAmount:N2} ₺").FontSize(11).Bold().FontColor(Colors.Green.Darken2);
                                });
                            });
                        });

                        // Üniversite Dağılımı
                        if (universityData.Any())
                        {
                            column.Item().Text("ÜNİVERSİTEYE GÖRE ÖĞRENCİ DAĞILIMI (İLK 10)").FontSize(12).Bold();
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Üniversite").Bold();
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Öğrenci").Bold();
                                });

                                foreach (var item in universityData.Take(10))
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Key);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Value.ToString());
                                }
                            });
                        }

                        // Üyelerin Verdiği Burs Dağılımı
                        if (memberScholarshipDetails.Any())
                        {
                            column.Item().PageBreak();
                            column.Item().Text("ÜYELERİN VERDİĞİ BURS DAĞILIMI").FontSize(12).Bold();
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