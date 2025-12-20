# 5) Operasyon & Destek

## Yaygın operasyon akışları

- Veri içe aktarma: **Excel İçe Aktar → Excel Dosyası Yükle**
- Rapor alma: **Dashboard/Raporlar → PDF Raporu Al**
- Log inceleme: **Aktivite Logları → Filtrele**

## Troubleshooting

- Giriş yapılamıyor
  - Mesaj: **Kullanıcı adı veya şifre hatalı!**
  - Aksiyon: kullanıcı adı/şifre doğrulama
- Excel yüklemede sorun
  - Ekran: **Dosya işleniyor...** aşamasında takılma/başarısızlık
  - Aksiyon: dosya formatını örnek Excel ile karşılaştırma (**Örnek Excel’i İndir**)

## Veri ve yedekleme

Projede yerel veritabanı dosyaları ve yedekleme scriptleri bulunur (ör. `.ps1`, `.bat`, `.sh`). Üretim ortamında yedekleme ve geri yükleme prosedürü kurum standartlarına göre belirlenmelidir.

## Log/Denetim

Kritik işlemler `ActivityLog` üzerinden izlenir. İnceleme için **Aktivite Logları** ekranını kullanın.
