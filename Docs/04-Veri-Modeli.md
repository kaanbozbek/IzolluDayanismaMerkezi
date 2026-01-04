# 4) Veri Modeli (Entities)

Bu bölüm, `Data/Entities` altındaki modelleri referans alır.

## Çekirdek varlıklar

- `Student` (Öğrenci)
  - Burs yaşam döngüsü (örn. aktiflik, kesilme nedeni/tarihi), IBAN, mezuniyet vb.
  - İlişkiler: transkript kayıtları, toplantı katılımları, burs ödemeleri
- `Member` (Üye)
  - Rol bayrakları (örn. mütevelli/yönetim/denetim), aktiflik durumu vb.
  - İlişkiler: burs taahhütleri
- `Meeting` (Toplantı)
  - Başlık/tür/konum/açıklama/tarih aralığı
  - İlişkiler: öğrenci katılımları, üye katılımları
- `Village` (Köy)
  - Nüfus, eğitim kademelerine göre öğrenci sayıları, muhtar bilgileri, notlar
- `Aid` (Yardım)
  - Ad/telefon/adres/refere eden, durum/konum gibi alanlar
- `Donor` (Bağışçı)
  - İletişim ve burs/bağış bilgileri

## Destekleyici varlıklar

- `ActivityLog`: kullanıcı, işlem tipi, detay, tarih
- `TranscriptRecord`: transkript bilgileri
- Dönem ve burs yapılandırmaları:
  - `Period` / `Term` (projede kullanılan isimlendirmeye göre)
  - `TermScholarshipConfig`
- Burs taahhüt/ödeme:
  - `MemberScholarshipCommitment`
  - `ScholarshipPayment`
- Katılım:
  - Öğrenci ve üye toplantı katılım varlıkları

## İsimlendirme notu

Kodda hem “Dönem/Period” hem “Term” benzeri kavramlar görülebilir. Dokümantasyonda ekrandaki terim **Dönem** olacak şekilde referanslanır.
