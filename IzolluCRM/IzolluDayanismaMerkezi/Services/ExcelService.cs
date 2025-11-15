using ClosedXML.Excel;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

public class ExcelService
{
    public byte[] GenerateStudentTemplate()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Öğrenciler");

        // Başlıklar
        var headers = new[]
        {
            "Sicil Numarası", "Ad Soyad", "TC No", "Email", "Telefon", "Cinsiyet", "Doğum Tarihi", "Köy",
            "Ebeveyn Adı", "Ebeveyn Telefon", "Adres", "Üniversite", "Bölüm",
            "Sınıf", "Referans", "Burs Başlangıç", "Dönem", "IBAN"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        // Örnek satır
        worksheet.Cell(2, 1).Value = "2025001";
        worksheet.Cell(2, 2).Value = "Örnek Öğrenci";
        worksheet.Cell(2, 3).Value = "12345678901";
        worksheet.Cell(2, 4).Value = "ornek@example.com";
        worksheet.Cell(2, 5).Value = "0532 123 4567";
        worksheet.Cell(2, 6).Value = "Kadın";
        worksheet.Cell(2, 7).Value = "15.05.2003";
        worksheet.Cell(2, 8).Value = "Örnek Köy";
        worksheet.Cell(2, 9).Value = "Ebeveyn Adı";
        worksheet.Cell(2, 10).Value = "0532 999 8888";
        worksheet.Cell(2, 11).Value = "Adres bilgisi";
        worksheet.Cell(2, 12).Value = "İstanbul Üniversitesi";
        worksheet.Cell(2, 13).Value = "Bilgisayar Mühendisliği";
        worksheet.Cell(2, 14).Value = "1";
        worksheet.Cell(2, 15).Value = "Referans kişi";
        worksheet.Cell(2, 16).Value = "01.09.2024";
        worksheet.Cell(2, 17).Value = "2025-2026";
        worksheet.Cell(2, 18).Value = "TR330006100519786457841326";

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportStudents(List<Student> students)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Öğrenciler");

        // Başlıklar
        var headers = new[]
        {
            "Ad Soyad", "TC No", "Email", "Telefon", "Doğum Tarihi", "Yaş", "Cinsiyet", "Köy",
            "Ebeveyn Adı", "Ebeveyn Telefon", "Adres", "Üniversite", "Bölüm",
            "Sınıf", "Referans", "Burs Başlangıç", "Burs Bitiş", "Sicil Numarası",
            "IBAN", "Mezun", "Mezuniyet Tarihi", "Aktif Burs", "Transkript Notu"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
        }

        // Veri satırları
        for (int i = 0; i < students.Count; i++)
        {
            var student = students[i];
            int row = i + 2;

            worksheet.Cell(row, 1).Value = student.AdSoyad;
            worksheet.Cell(row, 2).Value = student.TCNo ?? "";
            worksheet.Cell(row, 3).Value = student.Email ?? "";
            worksheet.Cell(row, 4).Value = student.Telefon ?? "";
            worksheet.Cell(row, 5).Value = student.DogumTarihi?.ToString("dd.MM.yyyy") ?? "";
            worksheet.Cell(row, 6).Value = student.Yas?.ToString() ?? "";
            worksheet.Cell(row, 7).Value = student.Cinsiyet ?? "";
            worksheet.Cell(row, 8).Value = student.Koy ?? "";
            worksheet.Cell(row, 9).Value = student.EbeveynAdi ?? "";
            worksheet.Cell(row, 10).Value = student.EbeveynTelefon ?? "";
            worksheet.Cell(row, 11).Value = student.Adres ?? "";
            worksheet.Cell(row, 12).Value = student.Universite ?? "";
            worksheet.Cell(row, 13).Value = student.Bolum ?? "";
            worksheet.Cell(row, 14).Value = student.Sinif;
            worksheet.Cell(row, 15).Value = student.Referans ?? "";
            worksheet.Cell(row, 16).Value = student.BursBaslangicTarihi?.ToString("dd.MM.yyyy") ?? "";
            worksheet.Cell(row, 17).Value = student.BursBitisTarihi?.ToString("dd.MM.yyyy") ?? "";
            worksheet.Cell(row, 18).Value = student.SicilNumarasi ?? "";
            worksheet.Cell(row, 19).Value = student.IBAN ?? "";
            worksheet.Cell(row, 20).Value = student.MezunMu ? "Evet" : "Hayır";
            worksheet.Cell(row, 21).Value = student.MezuniyetTarihi?.ToString("dd.MM.yyyy") ?? "";
            worksheet.Cell(row, 22).Value = student.AktifBursMu ? "Evet" : "Hayır";
            worksheet.Cell(row, 23).Value = student.TranskriptNotu ?? "";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public List<Student> ImportStudents(Stream fileStream, string activePeriod)
    {
        var students = new List<Student>();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(1);

        // Başlık kontrolü
        var expectedHeaders = new[]
        {
            "Sicil Numarası", "Ad Soyad", "TC No", "Email", "Telefon", "Cinsiyet", "Doğum Tarihi", "Köy",
            "Ebeveyn Adı", "Ebeveyn Telefon", "Adres", "Üniversite", "Bölüm",
            "Sınıf", "Referans", "Burs Başlangıç", "Dönem", "IBAN"
        };

        for (int i = 0; i < expectedHeaders.Length; i++)
        {
            var cellValue = worksheet.Cell(1, i + 1).GetString();
            if (cellValue != expectedHeaders[i])
            {
                throw new Exception($"Hatalı başlık: '{cellValue}'. Beklenen: '{expectedHeaders[i]}'");
            }
        }

        // Veri okuma (2. satırdan başla, 1. satır başlık)
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            try
            {
                var student = new Student
                {
                    SicilNumarasi = row.Cell(1).GetString(),
                    AdSoyad = row.Cell(2).GetString(),
                    TCNo = row.Cell(3).GetString(),
                    Email = row.Cell(4).GetString(),
                    Telefon = row.Cell(5).GetString(),
                    Cinsiyet = row.Cell(6).GetString(),
                    DogumTarihi = ParseDate(row.Cell(7).GetString()),
                    Koy = row.Cell(8).GetString(),
                    EbeveynAdi = row.Cell(9).GetString(),
                    EbeveynTelefon = row.Cell(10).GetString(),
                    Adres = row.Cell(11).GetString(),
                    Universite = row.Cell(12).GetString(),
                    Bolum = row.Cell(13).GetString(),
                    Sinif = ParseInt(row.Cell(14).GetString(), 1),
                    Referans = row.Cell(15).GetString(),
                    BursBaslangicTarihi = ParseDate(row.Cell(16).GetString()),
                    IBAN = row.Cell(18).GetString(),
                    AktifBursMu = true,
                    OlusturmaTarihi = DateTime.Now
                };

                students.Add(student);
            }
            catch (Exception ex)
            {
                throw new Exception($"Satır {row.RowNumber()} işlenirken hata: {ex.Message}");
            }
        }

        return students;
    }

    private DateTime? ParseDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (DateTime.TryParse(value, out var date))
            return date;

        return null;
    }

    private int ParseInt(string value, int defaultValue = 0)
    {
        if (int.TryParse(value, out var result))
            return result;

        return defaultValue;
    }

    private decimal ParseDecimal(string value)
    {
        if (decimal.TryParse(value, out var result))
            return result;

        return 0;
    }

    public byte[] GenerateMemberTemplate()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Üyeler");

        // Başlıklar
        var headers = new[]
        {
            "Sicil Numarası", "Ad Soyad", "TC No", "Email", "Telefon", "Adres", 
            "Meslek", "Firma", "Doğum Tarihi", "Köy / Mahalle", "Üyelik Türü", 
            "Üyelik Başlangıç Tarihi", "Durum", "Not"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        // Örnek satır
        worksheet.Cell(2, 1).Value = "UYE2025001";
        worksheet.Cell(2, 2).Value = "Örnek Üye";
        worksheet.Cell(2, 3).Value = "12345678901";
        worksheet.Cell(2, 4).Value = "uye@example.com";
        worksheet.Cell(2, 5).Value = "0532 123 4567";
        worksheet.Cell(2, 6).Value = "Adres bilgisi";
        worksheet.Cell(2, 7).Value = "Mühendis";
        worksheet.Cell(2, 8).Value = "Örnek Şirket A.Ş.";
        worksheet.Cell(2, 9).Value = "15.05.1980";
        worksheet.Cell(2, 10).Value = "Örnek Mahalle";
        worksheet.Cell(2, 11).Value = "Üye";
        worksheet.Cell(2, 12).Value = "01.01.2025";
        worksheet.Cell(2, 13).Value = "Aktif";
        worksheet.Cell(2, 14).Value = "Örnek not";

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportMembers(List<Member> members)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Üyeler");

        // Başlıklar
        var headers = new[]
        {
            "Sicil Numarası", "Ad Soyad", "TC No", "Email", "Telefon", "Adres", 
            "Meslek", "Doğum Tarihi", "Yaş", "Köy / Mahalle", "Üyelik Türü", 
            "Üyelik Başlangıç Tarihi", "Durum", "Yönetim Kurulu", "Mütevelli Heyeti",
            "Burs Veriyor", "Not"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
        }

        // Veri satırları
        for (int i = 0; i < members.Count; i++)
        {
            var member = members[i];
            int row = i + 2;

            worksheet.Cell(row, 1).Value = member.SicilNumarasi ?? "";
            worksheet.Cell(row, 2).Value = member.AdSoyad;
            worksheet.Cell(row, 3).Value = member.TCNo ?? "";
            worksheet.Cell(row, 4).Value = member.Email ?? "";
            worksheet.Cell(row, 5).Value = member.Telefon ?? "";
            worksheet.Cell(row, 6).Value = member.Adres ?? "";
            worksheet.Cell(row, 7).Value = member.Meslek ?? "";
            worksheet.Cell(row, 8).Value = member.DogumTarihi?.ToString("dd.MM.yyyy") ?? "";
            worksheet.Cell(row, 9).Value = member.Yas?.ToString() ?? "";
            worksheet.Cell(row, 10).Value = member.Koy ?? "";
            worksheet.Cell(row, 11).Value = member.UyelikTuru ?? "";
            worksheet.Cell(row, 12).Value = member.UyelikBaslangicTarihi?.ToString("dd.MM.yyyy") ?? "";
            worksheet.Cell(row, 13).Value = member.Durum ?? "";
            worksheet.Cell(row, 14).Value = member.IsYonetimKurulu ? "Evet" : "Hayır";
            worksheet.Cell(row, 15).Value = member.IsMutevelli ? "Evet" : "Hayır";
            worksheet.Cell(row, 16).Value = member.BursVeriyor ? "Evet" : "Hayır";
            worksheet.Cell(row, 17).Value = member.Notlar ?? "";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public List<Member> ImportMembers(Stream fileStream)
    {
        var members = new List<Member>();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(1);

        // Başlık kontrolü
        var expectedHeaders = new[]
        {
            "Sicil Numarası", "Ad Soyad", "TC No", "Email", "Telefon", "Adres", 
            "Meslek", "Firma", "Doğum Tarihi", "Köy / Mahalle", "Üyelik Türü", 
            "Üyelik Başlangıç Tarihi", "Durum", "Not"
        };

        for (int i = 0; i < expectedHeaders.Length; i++)
        {
            var cellValue = worksheet.Cell(1, i + 1).GetString();
            if (cellValue != expectedHeaders[i])
            {
                throw new Exception($"Hatalı başlık: '{cellValue}'. Beklenen: '{expectedHeaders[i]}'");
            }
        }

        // Veri okuma (2. satırdan başla, 1. satır başlık)
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            try
            {
                var member = new Member
                {
                    SicilNumarasi = row.Cell(1).GetString(),
                    AdSoyad = row.Cell(2).GetString(),
                    TCNo = row.Cell(3).GetString(),
                    Email = row.Cell(4).GetString(),
                    Telefon = row.Cell(5).GetString(),
                    Adres = row.Cell(6).GetString(),
                    Meslek = row.Cell(7).GetString(),
                    Firma = row.Cell(8).GetString(),
                    DogumTarihi = ParseDate(row.Cell(9).GetString()),
                    Koy = row.Cell(10).GetString(),
                    UyelikTuru = row.Cell(11).GetString(),
                    UyelikBaslangicTarihi = ParseDate(row.Cell(12).GetString()),
                    Durum = row.Cell(13).GetString(),
                    Notlar = row.Cell(14).GetString()
                };

                // Üyelik türüne göre otomatik bayrak ayarla
                var uyeType = member.UyelikTuru?.ToLower() ?? "";
                if (uyeType.Contains("yönetim kurulu"))
                    member.IsYonetimKurulu = true;
                if (uyeType.Contains("mütevelli"))
                    member.IsMutevelli = true;

                members.Add(member);
            }
            catch (Exception ex)
            {
                throw new Exception($"Satır {row.RowNumber()} işlenirken hata: {ex.Message}");
            }
        }

        return members;
    }

    public byte[] ExportScholarshipCutStudents(List<Student> students, string cutType)
    {
        using var workbook = new XLWorkbook();
        // Sheet name must be <= 31 characters
        var worksheet = workbook.Worksheets.Add("Bursu Kesilenler");

        // Başlıklar
        var headers = new[]
        {
            "Ad Soyad", "TC No", "Telefon", "Email", "Üniversite", "Bölüm", 
            "Sınıf", "Bağışçı", "Kesim Sebebi", "Kesim Tarihi", "Aylık Tutar", "Notlar"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.Red;
            worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
        }

        // Veri satırları
        for (int i = 0; i < students.Count; i++)
        {
            var student = students[i];
            int row = i + 2;

            worksheet.Cell(row, 1).Value = student.AdSoyad;
            worksheet.Cell(row, 2).Value = student.TCNo ?? "";
            worksheet.Cell(row, 3).Value = student.Telefon ?? "";
            worksheet.Cell(row, 4).Value = student.Email ?? "";
            worksheet.Cell(row, 5).Value = student.Universite ?? "";
            worksheet.Cell(row, 6).Value = student.Bolum ?? "";
            worksheet.Cell(row, 7).Value = student.Sinif?.ToString() ?? "";
            worksheet.Cell(row, 8).Value = student.BagisciAdi ?? "";
            worksheet.Cell(row, 9).Value = student.ScholarshipCutReason ?? "";
            worksheet.Cell(row, 10).Value = student.ScholarshipCutDate?.ToString("dd.MM.yyyy HH:mm") ?? "";
            worksheet.Cell(row, 11).Value = student.AylikTutar;
            worksheet.Cell(row, 12).Value = student.Notlar ?? "";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}