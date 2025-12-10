# 📘 İzollu Dayanışma Merkezi - Kullanım Kılavuzu

## 🎯 Genel Bakış

İzollu Dayanışma Merkezi, vakıf bünyesindeki öğrenci bursları, üye yönetimi, bağışçı takibi ve raporlama işlemlerini dijital ortamda yönetmek için geliştirilmiş kapsamlı bir CRM (Müşteri İlişkileri Yönetimi) sistemidir.

### Sistem Özellikleri
- ✅ **Öğrenci Yönetimi**: Burs alan öğrencilerin tüm bilgileri
- ✅ **Üye Yönetimi**: Vakıf üyelerinin kayıtları ve taahhütleri
- ✅ **Bağışçı Takibi**: Bağış yapan kişi ve kurumların kayıtları
- ✅ **Transkript Kontrolü**: Öğrenci not kayıtlarının takibi
- ✅ **Toplantı Yönetimi**: Katılım kayıtları ve raporlama
- ✅ **Finansal Raporlama**: Burs ödemeleri ve taahhüt takibi
- ✅ **Excel İçe/Dışa Aktarma**: Toplu veri işleme
- ✅ **PDF Raporlama**: Profesyonel raporlar oluşturma

---

## 🔐 Giriş Yapma

### İlk Giriş
1. Tarayıcınızda uygulamayı açın
2. **Kullanıcı Adı** ve **Şifre** alanlarını doldurun
3. **GİRİŞ YAP** butonuna tıklayın

**Varsayılan Giriş Bilgileri:**
- Kullanıcı Adı: `admin`
- Şifre: `admin123`

> ⚠️ **Güvenlik Notu:** İlk girişten sonra şifrenizi mutlaka değiştirin!

---

## 📊 Dashboard (Ana Sayfa)

Dashboard, sistemdeki tüm önemli istatistikleri bir bakışta görmenizi sağlar.

### Görünen İstatistikler

#### 📈 Genel İstatistikler
- **Aktif Dönem**: Güncel akademik dönem
- **Toplam Öğrenci**: Sistemdeki tüm öğrenci sayısı
- **Burs Alan**: Aktif burs alan öğrenci sayısı
- **Burs Alamayan**: Bursu kesilen/almayan öğrenci sayısı

#### 💰 Finansal İstatistikler
- **Taahhüt Edilen Burs Adedi**: Üyelerin verdiği toplam burs taahhütleri
- **Gerçekleşen Burs Adedi**: Fiilen yapılan burs atamaları
- **Toplam Yardım Sayısı**: Yapılan ayni/nakdi yardımlar
- **Köy Sayısı**: Kayıtlı köy sayısı

#### 👥 Üye İstatistikleri
- **Üye Sayısı**: Toplam üye sayısı
- **Toplam Dönem Burs Tutarı**: Seçili dönem için toplam burs tutarı
- **Taahhüt Edilen Burs Adedi**: Detaylı taahhüt listesi
- **Gerçekleşen Burs Adedi**: Gerçekleşen burs listesi

### 📋 Raporlar

#### Üniversiteye Göre Öğrenci Dağılımı
Her üniversiteden kaç öğrencinin kayıtlı olduğunu gösterir.

**Kullanım:**
- Liste otomatik olarak öğrenci sayısına göre sıralanır
- Toplam öğrenci sayısı yanında gösterilir

#### Üyelerin Verdiği Burs Dağılımı
Her üyenin kaç öğrenciye taahhüt verdiğini ve gerçekleştirdiğini gösterir.

**Sütunlar:**
- **Üye Adı**: Üyenin adı soyadı
- **Taahhüt Edilen**: Verilen taahhüt sayısı
- **Gerçekleşen**: Gerçekleşen burs sayısı

### 🎛️ Dönem Filtresi
Dashboard'daki tüm verileri belirli bir döneme göre filtreleyebilirsiniz.

**Kullanım:**
1. Üst kısımdaki "Dönem Filtresi" açılır menüsünü tıklayın
2. İstediğiniz dönemi seçin
3. Sayfa otomatik olarak yenilenir ve seçili döneme göre veriler gösterilir

### 📄 PDF Raporu Alma
Dashboard'daki tüm verileri PDF formatında indirebilirsiniz.

**Kullanım:**
1. Sağ üstteki **PDF RAPORU AL** butonuna tıklayın
2. PDF otomatik olarak oluşturulur ve indirilir

**PDF İçeriği:**
- Genel istatistikler
- Üniversiteye göre öğrenci listesi
- Üyelerin burs dağılımı
- Tarih damgası

---

## 🎓 Öğrenciler Sayfası

Burs alan öğrencilerin tüm bilgilerini yönettiğiniz ana modüldür.

### 📊 İstatistik Kartları

Sayfanın üst kısmında 6 adet istatistik kartı bulunur:

1. **Aktif Burslu Öğrenci**: Şu anda burs alan öğrenci sayısı
2. **Bursu Kesilmiş Öğrenci**: Bursu kesilen öğrenci sayısı
3. **Mezun Öğrenci**: Mezun olmuş öğrenci sayısı
4. **Aylık Toplam Burs**: Aylık ödenen toplam burs tutarı
5. **İzollulu Öğrenci**: İzollu'dan olan öğrenci sayısı
6. **Dışarıdan Öğrenci**: Dışarıdan olan öğrenci sayısı

### 🔍 Filtreleme Sistemi

#### Temel Filtreler (Her Zaman Görünür)
- **Arama**: Ad, soyad, telefon, email, sicil numarası ile arama
- **Burs Durumu**:
  - Tümü
  - Aktif Burslu
  - Bursu Kesilmiş
- **Mezuniyet Durumu**:
  - Tümü
  - Öğrenci
  - Mezun

#### Gelişmiş Filtreler
**Filtreler** butonuna tıklayarak açılan panel:

- **İzolluluk Durumu**: İzollulu / Dışarıdan
- **Cinsiyet**: Erkek / Kadın
- **Yaş Aralığı**: Min-Max yaş filtresi
- **Üniversite**: Üniversite adına göre filtreleme
- **Bölüm**: Bölüm adına göre filtreleme
- **Sınıf**: 1, 2, 3, 4, Hazırlık
- **Köy**: Köy adına göre filtreleme
- **Bağışçı**: Bağışçı adına göre filtreleme

**Filtre Kullanımı:**
1. **Filtreler** butonuna tıklayın
2. İstediğiniz filtreleri seçin
3. **Filtreleri Uygula** butonuna tıklayın
4. Temizlemek için **Filtreleri Temizle** kullanın

### 📑 Sekmeler

#### 1. Öğrenciler Sekmesi
Tüm öğrencilerin listelendiği ana sekmedir.

**Tablo Kolonları:**
- **Sicil No**: Otomatik atanan benzersiz numara (20001'den başlar)
- **Adı Soyadı**: Öğrencinin tam adı (tıklanabilir - düzenleme için)
- **TC No**: TC Kimlik numarası
- **Telefon**: İletişim telefonu
- **Bağışçı**: Bursu veren kişi/kurum
- **Burs Tutarı**: Aylık alınan burs miktarı
- **Referans**: Öğrenciyi yönlendiren kişi
- **Köy/Şehir**: Köy veya şehir bilgisi
- **İşlemler**: 
  - 👁️ **Detay**: Öğrenci detay sayfası
  - ✏️ **Düzenle**: Öğrenci bilgilerini düzenle
  - 🎓 **Mezun Et**: Öğrenciyi mezun et
  - ❌ **Sil**: Öğrenciyi sil

**Toplu İşlemler:**
- **Excel'e Aktar**: Listelenen öğrencileri Excel'e aktarır

#### 2. Mezun Öğrenciler Sekmesi
Mezun olmuş öğrencilerin listelendiği sekmedir.

**Tablo Kolonları:**
- Sicil No
- Adı Soyadı
- Üniversite
- Bölüm
- Mezuniyet Tarihi
- Toplam Alınan Burs
- İşlemler (Detay, Düzenle, Sil)

### ➕ Yeni Öğrenci Ekleme

**Adımlar:**
1. Sağ üstteki **+ Yeni Öğrenci** butonuna tıklayın
2. Açılan formda gerekli bilgileri doldurun
3. **Kaydet** butonuna tıklayın

**Gerekli Alanlar:**
- ✅ Adı Soyadı
- ✅ Meslek (varsayılan: "Öğrenci")
- Sicil Numarası (otomatik atanır, değiştirilebilir)

**İsteğe Bağlı Alanlar:**
- TC Kimlik No
- Cinsiyet
- Doğum Tarihi / Yaş
- Telefon / Email
- Adres
- İzollulu mu? (Checkbox)
- Köy
- Ebeveyn Adı / Telefonu

**Eğitim Bilgileri:**
- Üniversite
- Bölüm
- Sınıf

**Burs Bilgileri:**
- Referans
- Bağışçı Adı
- Aylık Tutar
- Burs Başlangıç Tarihi
- Burs Bitiş Tarihi
- IBAN
- Aktif Burs Mu? (Checkbox)

**Notlar:**
- Genel Notlar
- Transkript Notu

> 💡 **İpucu:** Sicil numarası otomatik olarak veritabanındaki son öğrenciden sonraki numara ile doldurulur (20001, 20002, ...).

### ✏️ Öğrenci Düzenleme

**Adımlar:**
1. Öğrenci satırında **Düzenle** (✏️) ikonuna tıklayın
2. Değiştirmek istediğiniz bilgileri güncelleyin
3. **Güncelle** butonuna tıklayın

**Bursu Kesme:**
- "Burs Durumu Değişti" checkbox'ını işaretleyin
- Kesme nedeni ve tarihini girin
- **Güncelle** butonuna tıklayın

### 👁️ Öğrenci Detay Sayfası

Öğrencinin tüm bilgilerini görüntüleyebileceğiniz kapsamlı sayfa.

**Sekmeler:**

#### 1. Genel Bilgiler
- Kişisel bilgiler (Ad, TC, Doğum tarihi, vb.)
- İletişim bilgileri (Telefon, Email, Adres)
- Ebeveyn bilgileri

#### 2. Eğitim Bilgileri
- Üniversite, Bölüm, Sınıf
- Burs bilgileri
- Referans ve Bağışçı

#### 3. Finansal Bilgiler
- Aylık burs tutarı
- Toplam alınan burs
- IBAN
- Burs başlangıç/bitiş tarihleri
- Bursu kesme bilgileri (varsa)

#### 4. Transkript Kayıtları
Öğrencinin dönem not kayıtlarını görüntüleyin ve yönetin.

**Kolonlar:**
- Giriş Tarihi
- Akademik Yıl
- Dönem
- GNO
- Transkript Notu
- İşlemler (Görüntüle, Düzenle, Sil)

**Yeni Transkript Ekleme:**
1. **+ Transkript Ekle** butonuna tıklayın
2. Giriş tarihi, akademik yıl, dönem ve GNO girin
3. İsteğe bağlı transkript notu ekleyin
4. **Kaydet**

**PDF Görüntüleme:**
- Eğer transkript PDF dosyası varsa, **Görüntüle** (👁️) ikonu aktif olur
- Tıklayarak PDF'i yeni sekmede açabilirsiniz

#### 5. Toplantı Katılımları
Öğrencinin toplantılara katılım durumunu görün.

**Kolonlar:**
- Tarih
- Başlık
- Katıldı mı?

**Katılım Durumunu Değiştirme:**
1. Satırdaki checkbox'ı işaretleyin/kaldırın
2. Değişiklik otomatik olarak kaydedilir

#### 6. Ödeme Geçmişi
Öğrenciye yapılan burs ödemelerini görüntüleyin.

**Kolonlar:**
- Dönem
- Tutar
- Ödeme Tarihi
- Durum (Ödendi/Bekliyor/İptal)
- Notlar

**Yeni Ödeme Ekleme:**
1. **+ Ödeme Ekle** butonuna tıklayın
2. Dönem, tutar, ödeme tarihi girin
3. Durum seçin
4. İsteğe bağlı not ekleyin
5. **Kaydet**

### 🎓 Öğrenciyi Mezun Etme

**Adımlar:**
1. Öğrenci satırında **Mezun Et** (🎓) ikonuna tıklayın
2. Açılan onay dialogunda **Evet** butonuna tıklayın
3. Öğrenci "Mezun Öğrenciler" sekmesine taşınır

**Mezuniyet İşlemi:**
- Mezuniyet tarihi otomatik olarak bugünün tarihi olarak kaydedilir
- Öğrencinin bursu otomatik olarak kesilir
- Aktivite loguna kaydedilir

---

## 👥 Üyeler Sayfası

Vakıf üyelerinin kayıtlarını ve burs taahhütlerini yönettiğiniz modüldür.

### 📊 İstatistik Kartları

1. **Toplam Üye**: Sistemdeki toplam üye sayısı
2. **Aktif Üye**: Aktif durumda olan üye sayısı
3. **Pasif Üye**: Pasif durumda olan üye sayısı
4. **Toplam Taahhüt**: Toplam burs taahhüt sayısı
5. **Gerçekleşen**: Gerçekleşen burs sayısı
6. **Bekleyen**: Henüz gerçekleşmemiş taahhüt sayısı

### 🔍 Arama ve Filtreleme

- **Arama Çubuğu**: Ad, soyad, telefon, email, firma ile arama yapın
- **Durum Filtresi**: Aktif / Pasif / Tümü

### 📑 Sekmeler

#### 1. Genel Üyeler Sekmesi
Tüm üyelerin listelendiği ana sekmedir.

**Tablo Kolonları:**
- **Sicil No**: Otomatik atanan benzersiz numara (10001'den başlar)
- **Adı Soyadı**: Üyenin tam adı (tıklanabilir)
- **Mesleği**: Meslek bilgisi
- **Firma**: Çalıştığı firma
- **İletişim**: Telefon ve email
- **Roller**: Üyenin vakıftaki rolleri (Başkan, Üye, vb.)
- **Durum**: Aktif / Pasif
- **İşlemler**:
  - ✏️ **Düzenle**: Üye bilgilerini düzenle
  - 📋 **Taahhütler**: Burs taahhütlerini görüntüle
  - ❌ **Sil**: Üyeyi sil

**Toplu İşlemler:**
- **Excel'e Aktar**: Listelenen üyeleri Excel'e aktarır

#### 2. Yönetim Kurulu Sekmesi
Sadece yönetim kurulu üyelerinin listelendiği sekmedir.

**Filtre Mantığı:**
- "Üye" rolüne sahip olmayan
- "Başkan", "Başkan Yardımcısı", "Yönetim Kurulu" vb. rollere sahip üyeler

#### 3. Burs Taahhütleri Sekmesi
Tüm üyelerin verdiği burs taahhütlerini gösterir.

**Tablo Kolonları:**
- **Üye Adı**: Taahhüt veren üye
- **Öğrenci**: Taahhüt edilen öğrenci
- **Durum**: Aktif / Tamamlandı / İptal
- **Başlangıç Tarihi**: Taahhüt başlangıç tarihi
- **Bitiş Tarihi**: Taahhüt bitiş tarihi
- **Notlar**: Taahhüt notları
- **İşlemler**: Düzenle, Sil

### ➕ Yeni Üye Ekleme

**Adımlar:**
1. Sağ üstteki **+ Yeni Üye** butonuna tıklayın
2. Açılan formda gerekli bilgileri doldurun
3. **Kaydet** butonuna tıklayın

**Gerekli Alanlar:**
- ✅ Adı Soyadı
- ✅ Meslek
- Sicil Numarası (otomatik atanır, değiştirilebilir)

**İsteğe Bağlı Alanlar:**
- Firma
- Telefon / Email
- Adres
- TC Kimlik No
- Doğum Tarihi
- Cinsiyet
- Roller (çoklu seçim):
  - Başkan
  - Başkan Yardımcısı
  - Yönetim Kurulu
  - Denetim Kurulu
  - Üye
- Sektör
- Köy
- Ebeveyn Adı
- Aktif Mi? (Checkbox)

> 💡 **İpucu:** Sicil numarası otomatik olarak veritabanındaki son üyeden sonraki numara ile doldurulur (10001, 10002, ...).

### ✏️ Üye Düzenleme

**Adımlar:**
1. Üye satırında **Düzenle** (✏️) ikonuna tıklayın
2. Değiştirmek istediğiniz bilgileri güncelleyin
3. **Güncelle** butonuna tıklayın

### 📋 Burs Taahhütlerini Görüntüleme

**Adımlar:**
1. Üye satırında **Taahhütler** (📋) ikonuna tıklayın
2. Açılan dialogda üyenin tüm taahhütlerini görün

**Dialog İçeriği:**
- Üyenin adı
- Taahhüt listesi (Öğrenci, Durum, Tarihler)
- **+ Yeni Taahhüt Ekle** butonu
- **Kapat** butonu

**Yeni Taahhüt Ekleme:**
1. Dialog içinde **+ Yeni Taahhüt Ekle** butonuna tıklayın
2. Öğrenci seçin (arama yapılabilir dropdown)
3. Başlangıç ve bitiş tarihi girin
4. Durum seçin (Aktif/Tamamlandı/İptal)
5. İsteğe bağlı not ekleyin
6. **Kaydet**

---

## 🎁 Bağışçılar Sayfası

Bağış yapan kişi ve kurumların yönetildiği modüldür.

### 📊 İstatistik Kartları

1. **Toplam Bağışçı**: Sistemdeki toplam bağışçı sayısı
2. **Aktif Bağışçı**: Aktif durumda olan bağışçı sayısı

### 🔍 Arama ve Filtreleme

- **Arama Çubuğu**: Ad, soyad, telefon, email ile arama yapın

### 📋 Bağışçı Listesi

**Tablo Kolonları:**
- **Adı Soyadı**: Bağışçının adı (tıklanabilir - detay için)
- **Telefon**: İletişim telefonu
- **Email**: Email adresi
- **Durum**: Aktif / Pasif
- **İşlemler**:
  - 👁️ **Detay**: Bağışçı detaylarını görüntüle
  - ✏️ **Düzenle**: Bağışçı bilgilerini düzenle
  - ❌ **Sil**: Bağışçıyı sil

### ➕ Yeni Bağışçı Ekleme

**Adımlar:**
1. Sağ üstteki **+ Yeni Bağışçı** butonuna tıklayın
2. Açılan formda gerekli bilgileri doldurun
3. **Kaydet** butonuna tıklayın

**Gerekli Alanlar:**
- ✅ Adı Soyadı

**İsteğe Bağlı Alanlar:**
- Telefon / Email
- Adres
- Notlar
- Aktif Mi? (Checkbox)

### 👁️ Bağışçı Detayları

**Adımlar:**
1. Bağışçı adına veya **Detay** (👁️) ikonuna tıklayın
2. Açılan dialogda bağışçı bilgilerini görün

**Dialog İçeriği:**
- Ad Soyad
- İletişim bilgileri
- Adres
- Notlar
- Durum
- Kayıt tarihi

---

## 🏘️ Köyler Sayfası

Öğrencilerin geldikleri köylerin yönetildiği modüldür.

### 📊 İstatistik Kartları

1. **Toplam Köy**: Sistemdeki toplam köy sayısı
2. **Toplam Öğrenci**: Köylerden gelen toplam öğrenci sayısı

### 🔍 Arama

- **Arama Çubuğu**: Köy adı ile arama yapın

### 📋 Köy Listesi

**Tablo Kolonları:**
- **Köy Adı**: Köyün adı
- **İlçe**: İlçe bilgisi
- **İl**: İl bilgisi
- **Öğrenci Sayısı**: Bu köyden gelen öğrenci sayısı
- **İşlemler**:
  - ✏️ **Düzenle**: Köy bilgilerini düzenle
  - ❌ **Sil**: Köyü sil

### ➕ Yeni Köy Ekleme

**Adımlar:**
1. Sağ üstteki **+ Yeni Köy** butonuna tıklayın
2. Açılan formda gerekli bilgileri doldurun
3. **Kaydet** butonuna tıklayın

**Gerekli Alanlar:**
- ✅ Köy Adı

**İsteğe Bağlı Alanlar:**
- İlçe
- İl

---

## 🎁 Yardımlar Sayfası

Vakıf tarafından yapılan ayni ve nakdi yardımların yönetildiği modüldür.

### 📊 İstatistik Kartları

1. **Toplam Yardım**: Sistemdeki toplam yardım sayısı
2. **Nakdi Yardım**: Nakdi yardım sayısı
3. **Ayni Yardım**: Ayni yardım sayısı

### 🔍 Arama ve Filtreleme

- **Arama Çubuğu**: Alıcı adı, açıklama ile arama yapın
- **Yardım Türü**: Tümü / Nakdi / Ayni

### 📋 Yardım Listesi

**Tablo Kolonları:**
- **Alıcı Adı**: Yardımı alan kişinin adı
- **Yardım Türü**: Nakdi / Ayni
- **Miktar**: Yardım miktarı (TL veya adet)
- **Tarih**: Yardım tarihi
- **Açıklama**: Yardım açıklaması
- **İşlemler**:
  - ✏️ **Düzenle**: Yardım bilgilerini düzenle
  - ❌ **Sil**: Yardımı sil

### ➕ Yeni Yardım Ekleme

**Adımlar:**
1. Sağ üstteki **+ Yeni Yardım** butonuna tıklayın
2. Açılan formda gerekli bilgileri doldurun
3. **Kaydet** butonuna tıklayın

**Gerekli Alanlar:**
- ✅ Alıcı Adı
- ✅ Yardım Türü (Nakdi/Ayni)
- ✅ Miktar

**İsteğe Bağlı Alanlar:**
- Tarih (varsayılan: bugün)
- Açıklama

---

## 📅 Toplantılar Sayfası

Vakıf toplantılarının ve öğrenci katılımlarının yönetildiği modüldür.

### 📊 İstatistik Kartları

1. **Toplam Toplantı**: Sistemdeki toplam toplantı sayısı
2. **Yaklaşan Toplantı**: Gelecekteki toplantı sayısı
3. **Gerçekleşen Toplantı**: Geçmişteki toplantı sayısı

### 📋 Toplantı Listesi

**Tablo Kolonları:**
- **Başlık**: Toplantı başlığı
- **Tarih**: Toplantı tarihi
- **Konum**: Toplantı yeri
- **Katılımcı Sayısı**: Katılan öğrenci sayısı
- **Açıklama**: Toplantı açıklaması
- **İşlemler**:
  - 👥 **Katılımcılar**: Katılımcı listesini görüntüle
  - ✏️ **Düzenle**: Toplantı bilgilerini düzenle
  - ❌ **Sil**: Toplantıyı sil

### ➕ Yeni Toplantı Ekleme

**Adımlar:**
1. Sağ üstteki **+ Yeni Toplantı** butonuna tıklayın
2. Açılan formda gerekli bilgileri doldurun
3. **Kaydet** butonuna tıklayın

**Gerekli Alanlar:**
- ✅ Başlık
- ✅ Tarih

**İsteğe Bağlı Alanlar:**
- Konum
- Açıklama

### 👥 Katılımcı Yönetimi

**Adımlar:**
1. Toplantı satırında **Katılımcılar** (👥) ikonuna tıklayın
2. Açılan dialogda tüm öğrencilerin listesini görün
3. Katılım checkbox'larını işaretleyin/kaldırın
4. Değişiklikler otomatik olarak kaydedilir

**Katılımcı Listesi:**
- Tüm aktif öğrenciler listelenir
- Checkbox ile katılım durumu işaretlenir
- Katılan öğrenci sayısı üstte gösterilir

---

## 📥 Excel İçe Aktarma

Excel dosyalarından toplu veri içe aktarma işlemleri için kullanılır.

### Desteklenen Veri Türleri

1. **Öğrenciler**: Excel'den öğrenci listesi içe aktarma
2. **Üyeler**: Excel'den üye listesi içe aktarma
3. **Bağışçılar**: Excel'den bağışçı listesi içe aktarma

### 📥 İçe Aktarma Adımları

1. İçe aktarmak istediğiniz veri türünün sekmesine gidin
2. **Dosya Seç** butonuna tıklayın
3. Excel dosyasını seçin (.xlsx veya .xls)
4. **İçe Aktar** butonuna tıklayın
5. İşlem tamamlandığında sonuç mesajı görüntülenir

### 📋 Excel Formatı

Excel dosyanızın aşağıdaki sütunları içermesi gerekir:

#### Öğrenciler İçin:
- Ad Soyad (zorunlu)
- TC No
- Telefon
- Email
- Üniversite
- Bölüm
- Sınıf
- Köy
- Bağışçı Adı
- Aylık Tutar
- vb.

#### Üyeler İçin:
- Ad Soyad (zorunlu)
- Meslek
- Firma
- Telefon
- Email
- Roller
- vb.

#### Bağışçılar İçin:
- Ad Soyad (zorunlu)
- Telefon
- Email
- Adres
- vb.

> 💡 **İpucu:** İlk satırın başlık satırı olduğundan emin olun.

### ⚠️ Hata Yönetimi

- Geçersiz satırlar atlanır ve hata raporu gösterilir
- Zorunlu alanlar eksikse satır işlenmez
- Başarılı ve başarısız kayıt sayıları raporlanır

---

## ⚙️ Ayarlar Sayfası

Sistem genelindeki ayarların yapıldığı modüldür.

### 📑 Sekmeler

#### 1. Transkript Kontrolü

Öğrencilerin transkript kayıtlarını kontrol etmek için kullanılır.

**Kullanım:**
1. **Kontrol Et** butonuna tıklayın
2. Sistem tüm öğrencilerin transkript kayıtlarını analiz eder
3. Eksik veya sorunlu kayıtlar listelenir

**Kontrol Edilen Hususlar:**
- Her öğrencinin en az bir transkript kaydı var mı?
- Transkript tarihleri geçerli mi?
- GNO değerleri 0-4 arasında mı?

#### 2. Dönem Ayarları

Akademik dönemlerin yönetildiği sekmedir.

**Dönem Listesi:**
- **Akademik Yıl**: Örn: 2023-2024
- **Dönem**: Güz / Bahar
- **Başlangıç Tarihi**: Dönem başlangıcı
- **Bitiş Tarihi**: Dönem bitişi
- **Aktif Mi?**: Şu anki aktif dönem

**Yeni Dönem Ekleme:**
1. **+ Yeni Dönem** butonuna tıklayın
2. Akademik yıl, dönem adı ve tarihleri girin
3. Eğer bu dönemi aktif yapmak istiyorsanız "Aktif Mi?" checkbox'ını işaretleyin
4. **Kaydet**

**Dönem Düzenleme:**
1. Düzenle (✏️) ikonuna tıklayın
2. Bilgileri güncelleyin
3. **Güncelle**

#### 3. Genel Ayarlar

Sistem genelindeki listelerin (dropdown) yönetildiği sekmedir.

**Yönetilebilir Listeler:**
- **Sektörler**: Üye sektör listesi
- **Meslekler**: Meslek listesi
- **Roller**: Üye rolleri listesi
- **Üniversiteler**: Üniversite listesi
- **Bölümler**: Bölüm listesi
- **Köyler**: Köy listesi

**Liste Yönetimi:**
1. İstediğiniz listeyi seçin (örn: Sektörler)
2. Mevcut değerleri görün
3. **+ Yeni Ekle** butonuna tıklayın
4. Yeni değeri girin
5. **Kaydet**

**Değer Silme:**
1. Silmek istediğiniz değerin yanındaki çöp kutusu (🗑️) ikonuna tıklayın
2. Onay dialogunda **Evet** deyin

> ⚠️ **Dikkat:** Sildiğiniz değerler ilişkili kayıtlardan kaldırılmaz, sadece yeni kayıtlarda seçilemez hale gelir.

#### 4. Burs Ayarları

Varsayılan burs tutarının ayarlandığı sekmedir.

**Kullanım:**
1. "Varsayılan Aylık Burs Tutarı" alanına tutarı girin (örn: 5000)
2. **Kaydet** butonuna tıklayın

**Etki Alanı:**
- Yeni öğrenci eklerken "Aylık Tutar" alanı bu değerle doldurulur
- Mevcut öğrenciler etkilenmez

---

## 📖 Aktivite Logları

Sistemde yapılan tüm işlemlerin kayıt altına alındığı sayfadır.

### 🔍 Filtreleme

**Filtre Seçenekleri:**
- **İşlem Türü**: Oluşturma, Güncelleme, Silme, Tümü
- **Varlık Türü**: Öğrenci, Üye, Bağışçı, Transkript, Toplantı, vb.
- **Başlangıç Tarihi**: Tarih aralığı başlangıcı
- **Bitiş Tarihi**: Tarih aralığı bitişi
- **Kullanıcı**: İşlemi yapan kullanıcı

**Filtreleme Adımları:**
1. İstediğiniz filtreleri seçin
2. **Filtrele** butonuna tıklayın
3. Sonuçlar anında güncellenir

**Filtreleri Temizleme:**
- **Temizle** butonuna tıklayın

### 📋 Log Listesi

**Tablo Kolonları:**
- **Tarih**: İşlem tarihi ve saati
- **İşlem Türü**: CREATE, UPDATE, DELETE
- **Varlık Türü**: Student, Member, Donor, vb.
- **Varlık ID**: İşlem yapılan kaydın ID'si
- **Açıklama**: İşlem detayı (örn: "Öğrenci eklendi: Ahmet Yılmaz")
- **Kullanıcı**: İşlemi yapan kullanıcı

**Sayfalama:**
- Sayfa başına 50 kayıt gösterilir
- Alt kısımda sayfa numaraları ile gezinebilirsiniz

---

## 🎨 Kullanıcı Arayüzü İpuçları

### Navigasyon Menüsü

**Sol Taraftaki Menü:**
- **Dashboard**: Ana sayfa
- **Öğrenciler**: Öğrenci yönetimi
- **Üyeler**: Üye yönetimi
- **Köyler**: Köy yönetimi
- **Yardımlar**: Yardım kayıtları
- **Toplantılar**: Toplantı yönetimi
- **Excel İçe Aktar**: Excel import
- **Aktivite Logları**: Log kayıtları
- **Ayarlar**: Sistem ayarları
- **Çıkış Yap**: Oturumu kapat

**Menü Kullanımı:**
- Menü üzerindeki öğelere tıklayarak sayfalara geçiş yapın
- Aktif sayfa turuncu renkte vurgulanır
- Menüyü kapatmak için sol üst köşedeki ☰ ikonuna tıklayın

### Renk Kodları

- **Turuncu**: Birincil renk (Butonlar, aktif öğeler)
- **Mavi**: Bilgi mesajları
- **Yeşil**: Başarı mesajları
- **Kırmızı**: Hata mesajları, silme işlemleri
- **Sarı**: Uyarı mesajları

### Tablo İşlemleri

**Sıralama:**
- Kolon başlıklarına tıklayarak sıralama yapın
- Tekrar tıklayarak ters sıralama yapın

**Sayfalama:**
- Tablo altındaki sayfa numaralarını kullanın
- "Sayfa başına kayıt" dropdown'ından görüntülenen kayıt sayısını değiştirin (10, 25, 50, 100)

**Arama:**
- Tablo üstündeki arama kutusuna yazmaya başlayın
- Arama otomatik olarak yapılır (her tuş vuruşunda)

### Dialog (Açılır Pencere) Kullanımı

**Açılır Pencereleri Kapatma:**
- Sağ üst köşedeki ❌ ikonuna tıklayın
- **İptal** veya **Kapat** butonuna tıklayın
- Dialog dışına (karanlık alana) tıklayın

**Form Kaydetme:**
- **Kaydet** veya **Güncelle** butonuna tıklayın
- Gerekli alanları doldurmadan kaydedemezsiniz
- Kırmızı uyarı mesajları eksik alanları gösterir

### Bildirimler (Snackbar)

**Bildirim Türleri:**
- ✅ **Başarı** (Yeşil): İşlem başarıyla tamamlandı
- ❌ **Hata** (Kırmızı): İşlem başarısız oldu
- ⚠️ **Uyarı** (Sarı): Dikkat edilmesi gereken durum
- ℹ️ **Bilgi** (Mavi): Bilgilendirme mesajı

**Bildirim Konumu:**
- Sağ alt köşede görünür
- 5 saniye sonra otomatik olarak kapanır
- ❌ ikonuna tıklayarak manuel kapatabilirsiniz

---

## 🔧 Teknik Bilgiler

### Sistem Gereksinimleri

**Sunucu Tarafı:**
- .NET 8.0
- SQLite veritabanı
- Windows/Linux/macOS

**İstemci Tarafı:**
- Modern web tarayıcısı (Chrome, Firefox, Edge, Safari)
- JavaScript aktif olmalı
- Minimum 1280x720 çözünürlük önerilir

### Tarayıcı Uyumluluğu

**Desteklenen Tarayıcılar:**
- ✅ Google Chrome (90+)
- ✅ Mozilla Firefox (88+)
- ✅ Microsoft Edge (90+)
- ✅ Safari (14+)

**Desteklenmeyen:**
- ❌ Internet Explorer

### Performans İpuçları

**Hızlı Çalışma için:**
- Filtre kullanırken çok fazla tarih aralığı seçmeyin
- Excel import işlemlerinde 1000 satırdan fazla veri için birden fazla işlem yapın
- Büyük listelerde arama kutusunu kullanarak sonuçları daraltın

### Veri Güvenliği

**Otomatik Yedekleme:**
- Sistem günlük otomatik yedekleme yapar
- Yedekler `publish/DB_Backup` klasöründe saklanır
- 30 günden eski yedekler otomatik silinir

**Manuel Yedekleme:**
- `izolluvakfi.db` dosyasını kopyalayın
- Güvenli bir yere kaydedin

### Sorun Giderme

**Sık Karşılaşılan Sorunlar:**

1. **Giriş yapamıyorum**
   - Kullanıcı adı ve şifrenizi kontrol edin
   - Tarayıcı çerezlerini temizleyin
   - Farklı tarayıcı deneyin

2. **Sayfa yüklenmiyor**
   - İnternet bağlantınızı kontrol edin
   - Tarayıcıyı yenileyin (F5)
   - Cache'i temizleyin (Ctrl+Shift+Delete)

3. **Excel içe aktarma hatası**
   - Excel formatını kontrol edin
   - Başlık satırının doğru olduğundan emin olun
   - Dosya boyutunu küçültün (max 5 MB)

4. **PDF oluşturulmuyor**
   - Tarayıcı pop-up engelleyicisini kapatın
   - Farklı tarayıcı deneyin
   - Veritabanında veri olduğundan emin olun

5. **Veritabanı hatası**
   - Uygulama yöneticisine başvurun
   - Log dosyalarını kontrol edin
   - Veritabanı yedeğinden geri yükleyin

---

## 📞 Destek ve İletişim

### Teknik Destek
Sorunlarınız için sistem yöneticinize başvurun.

### Geri Bildirim
Önerileriniz ve hata bildirimleriniz için lütfen iletişime geçin.

---

## 📚 Sık Kullanılan Kısayollar

| İşlem | Kısayol |
|-------|---------|
| Sayfayı Yenile | F5 |
| Arama Kutusuna Git | Ctrl+F |
| Dialog Kapat | Esc |
| Yeni Kayıt Ekle | Alt+N (sayfaya göre değişir) |
| Excel'e Aktar | Alt+E (sayfaya göre değişir) |

---

## 🎓 Eğitim Videoları

*(Eğitim videoları için ayrı bir bölüm oluşturulabilir)*

---

## 📄 Yasal Uyarılar

- Sistemdeki tüm veriler kişisel veri koruma yasalarına (KVKK) tabidir
- Yetkisiz erişim ve veri paylaşımı yasaktır
- Tüm işlemler aktivite loglarında kaydedilir

---

## 📋 Versiyon Geçmişi

| Versiyon | Tarih | Açıklama |
|----------|-------|----------|
| 1.0.0 | 2025-11-16 | İlk sürüm yayınlandı |

---

## ✅ Son Kontrol Listesi

Sistemi kullanmaya başlamadan önce:

- [ ] Giriş bilgilerinizi aldınız mı?
- [ ] Tarayıcınız güncel mi?
- [ ] İnternet bağlantınız stabil mi?
- [ ] Kullanım kılavuzunu okudunuz mu?

---

**© 2025 İzollu Dayanışma Merkezi - Tüm Hakları Saklıdır**
