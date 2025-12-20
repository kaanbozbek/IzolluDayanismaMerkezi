# 3) Yönetici & Konfigürasyon Rehberi

## Ayarlar ekranı

Ayarlar (`/settings`) yönetimsel işlemlerin toplandığı ekrandır:

- **Transkript Kontrolü**: GNO’su 2.0 altı öğrencilerin burs durumunu otomatik günceller.
- **Toplantı Kontrolü**: toplantı/katılım kontrollerini çalıştırır.
- Liste yönetimleri:
  - **Sektörler** (Yeni Sektör Ekle)
  - **Üniversiteler** (Yeni Üniversite Ekle)
  - **Bölümler** (Yeni Bölüm Ekle)
  - **Dönemler** (Yeni Dönem Ekle)
  - **Burs Tutarları** (Yeni Burs Tutarı Ekle)

## Uygulama ayarları (kalıcı)

Uygulamada iki ana ayar yaklaşımı vardır:

- `Settings`: Anahtar-değer ve liste tipinde ayarlar (ör. varsayılan burs tutarı, sektör/üniversite/bölüm listeleri).
- `SystemSettings`: sistem seviyesinde bilgiler (örn. `AppVersion`, notlar ve aktif dönem referansı gibi alanlar).

Not: Ayarların hangi ekrandan hangi anahtarlarla kullanıldığı kod üzerinden takip edilir.

## Sürüm/Not yönetimi

`SystemSettings` üzerinden uygulama sürümü ve not gibi sistem metadata alanları tutulur.
