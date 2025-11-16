# Liste Sayfaları için Ortak Layout Kullanım Kılavuzu

## 📋 Genel Bakış

`ListPageLayout.razor` bileşeni oluşturuldu ve tüm liste sayfaları (Öğrenciler, Üyeler, İş Adamları, Köyler, Yardımlar vb.) için ortak bir yapı sağlıyor.

## ✅ Tamamlanan İşlemler

1. ✅ **Shared/ListPageLayout.razor** - Ortak layout bileşeni oluşturuldu
2. ✅ **wwwroot/css/site.css** - Layout için CSS stilleri eklendi
3. ✅ **Pages/Donors.razor** - Yeni layout'a taşındı (ÖRNEK)

## 🎨 Layout Yapısı

```
┌─────────────────────────────────────┐
│         Header (Sayfa Başlığı)      │
├─────────────────────────────────────┤
│         Stats (İstatistik Kartlar)   │
├─────────────────────────────────────┤
│  ToolbarLeft  │    ToolbarRight     │
│  (Butonlar)   │  (Arama + Filtre)   │
├─────────────────────────────────────┤
│         Tabs (Sekmeler)             │
├─────────────────────────────────────┤
│         Body (Tablo/Grid)           │
│         Pagination                  │
└─────────────────────────────────────┘
```

## 🔧 Kullanım Şablonu

### Basit Örnek (Donors sayfası gibi):

```razor
<ListPageLayout>
    
    <Header>
        <MudText Typo="Typo.h4">Sayfa Başlığı</MudText>
    </Header>

    <Stats>
        <MudGrid>
            <!-- İstatistik kartları -->
        </MudGrid>
    </Stats>

    <ToolbarLeft>
        <MudButton>Yeni Ekle</MudButton>
        <MudButton>Excel'e Aktar</MudButton>
    </ToolbarLeft>

    <ToolbarRight>
        <MudTextField @bind-Value="_searchString" Placeholder="Ara..." />
        <MudButton>Filtreler</MudButton>
    </ToolbarRight>

    <Body>
        <MudTable Items="@_items">
            <!-- Tablo içeriği -->
        </MudTable>
    </Body>

</ListPageLayout>
```

### Kompleks Örnek (Students/Members sayfası gibi):

```razor
<ListPageLayout>
    
    <Header>
        <MudText Typo="Typo.h4">Öğrenciler</MudText>
    </Header>

    <Stats>
        <MudGrid>
            <MudItem xs="12" sm="4">
                <MudPaper Style="background: linear-gradient(...); border-radius: 15px;">
                    <!-- İstatistik kartı -->
                </MudPaper>
            </MudItem>
            <!-- Diğer kartlar -->
        </MudGrid>
    </Stats>

    <ToolbarLeft>
        <MudButton Color="Color.Primary" OnClick="OpenAddDialog">
            <MudIcon Icon="@Icons.Material.Filled.Add" />
            Yeni Öğrenci Ekle
        </MudButton>
        <MudButton Color="Color.Success" OnClick="ExportToExcel">
            <MudIcon Icon="@Icons.Material.Filled.Download" />
            Excel'e Aktar
        </MudButton>
        <MudButton Color="Color.Info" OnClick="ImportFromExcel">
            <MudIcon Icon="@Icons.Material.Filled.Upload" />
            Excel'den Yükle
        </MudButton>
    </ToolbarLeft>

    <ToolbarRight>
        <MudTextField @bind-Value="_searchText"
                      Placeholder="İsim, okul, sicil..."
                      Adornment="Adornment.Start"
                      AdornmentIcon="@Icons.Material.Filled.Search"
                      Immediate="true" />
        
        <MudButton Variant="Variant.Outlined"
                   OnClick="ToggleFilters">
            <MudIcon Icon="@Icons.Material.Filled.Tune" />
            Filtreler
        </MudButton>
    </ToolbarRight>

    <Tabs>
        <MudTabs>
            <MudTabPanel Text="Öğrenciler">
                <!-- Tab içeriği -->
            </MudTabPanel>
            <MudTabPanel Text="Mezun Öğrenciler">
                <!-- Tab içeriği -->
            </MudTabPanel>
        </MudTabs>
    </Tabs>

    <Body>
        <MudTable Items="@_filteredStudents">
            <!-- Tablo içeriği -->
        </MudTable>
    </Body>

</ListPageLayout>
```

## 📝 Diğer Sayfalar için Yapılması Gerekenler

### 1. **Students.razor** (ÖNCELİKLİ)

Mevcut dosyanın yapısı:
- ✅ İstatistik kartları var
- ✅ Butonlar var (Yeni Ekle, Excel'e Aktar)
- ✅ Arama ve filtreler var
- ✅ Tabs var (Öğrenciler | Mezun Öğrenciler)
- ⚠️ Çok karmaşık filtre drawer var

**Değişiklik planı:**
```razor
@page "/students"
@* ... using statements ... *@

<PageTitle>Öğrenciler - İzollu Dayanışma Merkezi</PageTitle>

<ListPageLayout>
    <Header>
        <MudText Typo="Typo.h4">Öğrenciler</MudText>
    </Header>

    <Stats>
        @* Mevcut kartları buraya taşı (satır 12-50) *@
    </Stats>

    <ToolbarLeft>
        @* Satır 54-61 arası butonlar buraya *@
    </ToolbarLeft>

    <ToolbarRight>
        @* Satır 64-83 arası arama/filtre buraya *@
    </ToolbarRight>

    <Tabs>
        @* Satır 52-530 arası MudTabs buraya *@
    </Tabs>

    <Body>
        @* Tablo zaten tabs içinde olduğu için Tabs içinde bırak *@
    </Body>
</ListPageLayout>

@* Satır 532+ Filter Drawer olduğu gibi kal *@
@* Satır 635+ @code bloğu olduğu gibi kal *@
```

### 2. **Members.razor** (ÖNCELİKLİ)

**Mevcut yapı:**
- ✅ 5 istatistik kartı (satır 13-69)
- ✅ Excel'e Aktar butonu (satır 76-82)
- ✅ Arama ve filtre (satır 85-103)
- ✅ 4 Tab (Tüm Üyeler, Bağışçılar, Mütevelli Heyeti, Yönetim Kurulu)
- ✅ Sağ tarafta filtre paneli

**Değişiklik planı:**
```razor
<ListPageLayout>
    <Header>
        <MudText Typo="Typo.h4">Üyeler</MudText>
    </Header>

    <Stats>
        @* Satır 13-69 istatistik kartları *@
    </Stats>

    <ToolbarLeft>
        <MudButton Color="Color.Primary" OnClick="OpenAddDialog">
            Üye Ekle
        </MudButton>
        <MudButton Color="Color.Success" OnClick="ExportToExcel">
            Excel'e Aktar
        </MudButton>
    </ToolbarLeft>

    <ToolbarRight>
        @* Satır 85-103 arama/filtre *@
    </ToolbarRight>

    <Tabs>
        @* Satır 139-471 MudTabs *@
    </Tabs>

    <Body>
        @* Tablolar zaten tabs içinde *@
    </Body>
</ListPageLayout>

@* Sağ tarafta ayrı filtre paneli var, onu <MudGrid> ile yan yana yerleştir *@
```

### 3. **Diğer Sayfalar**

#### Villages (Köyler):
```razor
<ListPageLayout>
    <Header>
        <MudText Typo="Typo.h4">Köyler</MudText>
    </Header>

    <Stats>
        <MudGrid>
            <MudItem xs="12" sm="3">
                <MudPaper Style="background: gradient; border-radius: 15px;">
                    Toplam Köy: @TotalVillages
                </MudPaper>
            </MudItem>
            <MudItem xs="12" sm="3">
                <MudPaper Style="background: gradient; border-radius: 15px;">
                    Toplam Nüfus: @TotalPopulation
                </MudPaper>
            </MudItem>
            <MudItem xs="12" sm="3">
                <MudPaper Style="background: gradient; border-radius: 15px;">
                    Toplam Öğrenci: @TotalStudents
                </MudPaper>
            </MudItem>
            <MudItem xs="12" sm="3">
                <MudPaper Style="background: gradient; border-radius: 15px;">
                    Ortalama Nüfus: @AveragePopulation
                </MudPaper>
            </MudItem>
        </MudGrid>
    </Stats>

    <ToolbarLeft>
        <MudButton Color="Color.Primary">Köy Ekle</MudButton>
        <MudButton Color="Color.Success">Excel'e Aktar</MudButton>
    </ToolbarLeft>

    <ToolbarRight>
        <MudTextField @bind-Value="_search" Placeholder="Köy ara..." />
    </ToolbarRight>

    @* Tabs yok *@

    <Body>
        <MudTable Items="@_villages">
            <!-- Köy tablosu -->
        </MudTable>
    </Body>
</ListPageLayout>
```

#### Aids (Yardımlar):
```razor
<ListPageLayout>
    <Header>
        <MudText Typo="Typo.h4">Yardımlar</MudText>
    </Header>

    <Stats>
        <MudGrid>
            <MudItem xs="12" sm="4">
                <MudPaper Style="background: gradient; border-radius: 15px;">
                    Toplam Yardım
                </MudPaper>
            </MudItem>
            <MudItem xs="12" sm="4">
                <MudPaper Style="background: gradient; border-radius: 15px;">
                    İzollulu
                </MudPaper>
            </MudItem>
            <MudItem xs="12" sm="4">
                <MudPaper Style="background: gradient; border-radius: 15px;">
                    Dışarıdan
                </MudPaper>
            </MudItem>
        </MudGrid>
    </Stats>

    <ToolbarLeft>
        <MudButton Color="Color.Primary">Yeni Yardım Ekle</MudButton>
        <MudButton Color="Color.Success">Excel'e Aktar</MudButton>
    </ToolbarLeft>

    <ToolbarRight>
        <MudTextField @bind-Value="_search" Placeholder="Ara..." />
        <MudButton OnClick="ToggleFilter">
            <MudIcon Icon="@Icons.Material.Filled.Tune" />
            Filtreler
        </MudButton>
    </ToolbarRight>

    @* Tabs varsa ekle *@

    <Body>
        <MudTable Items="@_aids">
            <!-- Yardım tablosu -->
        </MudTable>
    </Body>
</ListPageLayout>
```

## 🎯 Avantajlar

1. **Tutarlılık**: Tüm sayfalar aynı yapıyı kullanır
2. **Responsive**: Mobilde otomatik alt alta düşer
3. **Bakım Kolaylığı**: Tek bir yerde değişiklik yaparsın, tüm sayfalar güncellenir
4. **Temiz Kod**: Her sayfa daha kısa ve okunabilir olur
5. **Ölçeklenebilir**: Yeni sayfa eklemek çok kolay

## 📱 Responsive Davranış

- **Desktop (>768px)**: Toolbar'da butonlar ve arama yan yana
- **Mobile (<768px)**: Toolbar'daki tüm elemanlar alt alta sıralanır
- Kartlar otomatik grid yapısıyla düzenlenir
- Tablolar mobilde scroll edilebilir

## 🔥 Sonraki Adımlar

1. ✅ **Donors.razor** - Tamamlandı (örnek olarak)
2. ⏳ **Students.razor** - Karmaşık yapı, manuel refactor gerekebilir
3. ⏳ **Members.razor** - Yan panel filtreleri var, grid yapısı ayarlanmalı
4. ⏳ **Villages.razor** - Yeni sayfa, sıfırdan eklenebilir
5. ⏳ **Aids.razor** - Var olan sayfayı refactor et
6. ⏳ **Reports.razor** - Rapor sayfası, benzer yapı uygulanabilir

## 💡 İpuçları

- Mevcut dosyaları `.bak` uzantısıyla yedekle
- Önce basit sayfalardan başla (Donors gibi)
- Kompleks sayfalarda (Students, Members) parça parça taşı
- Filtreleri ve drawer'ları Body dışında tut
- @code bloğunu değiştirme, sadece HTML yapısını düzenle

## 📂 Dosya Yapısı

```
Pages/
├── Students.razor          (Refactor edilmeli)
├── Students_OLD.razor.bak (Yedek)
├── Members.razor          (Refactor edilmeli)
├── Donors.razor           (✅ Tamamlandı)
├── Donors_OLD.razor.bak  (Yedek)
└── ...

Shared/
└── ListPageLayout.razor   (✅ Ortak layout)

wwwroot/css/
└── site.css               (✅ Layout stilleri eklendi)
```

## ⚠️ Dikkat Edilmesi Gerekenler

1. **Filter Drawer**: Students ve Members'ta sağ tarafta filtre paneli var. Bunları `<MudGrid>` ile yan yana yerleştir.
2. **Tabs**: Students ve Members'ta Tabs içinde tablolar var. Bu yapıyı koru.
3. **@code bloğu**: Hiçbir sayfada C# kodunu değiştirme, sadece HTML yapısını refactor et.
4. **İstatistik Kartları**: Mevcut renkli gradient'ları koru, sadece yerini değiştir.
5. **Responsive**: Mobilde butonların alt alta düşmesini test et.

---

**Son Güncelleme**: 16 Kasım 2025
**Durum**: Donors.razor örnek olarak tamamlandı, diğer sayfalar için şablon hazır
