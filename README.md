# Travelio – Tur Rezervasyon Platformu

ASP.NET Core MVC ve MongoDB ile geliştirilmiş, tur, rezervasyon, yorum, soru-cevap ve raporlama süreçlerini yöneten dinamik bir seyahat platformu (Bootcamp Case Çalışması).

## Kullanılan Teknolojiler

- ASP.NET Core MVC (.NET 8)
- MongoDB + MongoDB.Driver
- ASP.NET Core Identity (MongoDB store) + Cookie Authentication
- FluentValidation ve AutoMapper
- Chart.js
- ClosedXML (Excel) ve QuestPDF (PDF)
- ASP.NET Core Localization (TR / EN)

## Kurulum

1. MongoDB'nin `mongodb://localhost:27017` adresinde çalıştığından emin olun (ayar: `Travel.Web/appsettings.json`).
2. Projeyi çalıştırın:

   ```bash
   cd Travel.Web
   dotnet run
   ```

3. Uygulama ilk açılışta rolleri ve iki kullanıcıyı otomatik oluşturur:

   | Rol   | E-posta                 | Şifre         |
   |-------|-------------------------|---------------|
   | Admin | korayhan@travelio.com   | Password12*   |
   | User  | aras@travelio.com       | Password12*   |

## MongoDB Koleksiyonları

`Tours`, `Destinations`, `Categories`, `Reservations`, `Users` (Identity), `Reviews` (yorumlar), `Questions`, `Favorites`

Tur tarihleri (her tarihe özel kontenjan), gün gün program, galeri ve öne çıkan özellikler **Tour dokümanının içinde nested (embedded) olarak** tutulur.

---

## Yapılan Geliştirmeler

### 1. Rezervasyon Sistemi
- Tur detayında tur tarihi seçimi, yetişkin / çocuk sayısı ve **otomatik toplam ücret** hesaplama (çocuk fiyatı yetişkin fiyatının %60'ı).
- Rezervasyon sonrası ilgili tarihin kontenjanı **atomik olarak** düşürülür (aynı anda gelen istekler kontenjanı aşamaz). Kontenjan 0 olduğunda rezervasyon alınmaz.
- Durumlar: **Bekliyor / Onaylandı / İptal Edildi**. İptal edilen rezervasyonun kontenjanı iade edilir.
- Kullanıcı başlamamış rezervasyonunu profilinden iptal edebilir.

### 2. Tur Detay Sayfası (tamamen veritabanından)
- Tur bilgileri, fiyat, süre, rota ve görsel galeri
- Gelecekteki tur tarihleri ve **kalan kontenjan**
- Öne çıkanlar, dahil olan / olmayan hizmetler, tur bilgileri
- Gün gün program (**accordion** yapısı; başlık, açıklama, şehir, ulaşım, yemek)
- Kullanıcı yorumları, **ortalama puan** ve **1-5 yıldız dağılımı (%)**
- Cevaplanmış sorular ve soru gönderme formu
- Favoriye ekleme / çıkarma, benzer turlar

### 3. Yorum, Puan ve Favoriler
- Giriş yapan kullanıcı tura 1-5 yıldız puan verip yorum yapabilir (tur başına bir yorum).
- Ortalama puan ve yorum sayısı Aggregation Pipeline ile hesaplanıp Tour dokümanına yazılır.
- Favoriler `Favorites` koleksiyonunda tutulur.

### 4. Tur Hakkında Sorular
- Kullanıcı tur detayından soru gönderir.
- Cevaplanmamış sorular admin panelinde en üstte listelenir; admin için **soru cevaplama ekranı** eklendi.
- Cevap verildiğinde tur detay sayfasında ve kullanıcının profilinde görünür.

### 5. Ana Sayfa ve Tur Listeleme
- **En çok rezervasyon alan turlar** ve **popüler destinasyonlar** dinamik olarak gelir.
- Toplam tur, destinasyon ve kullanıcı sayıları veritabanından gösterilir.
- Ana sayfa arama kutusu tur listesine filtre parametreleriyle yönlendirir.
- Tur listesi **destinasyon, kategori, fiyat, tarih aralığı, kişi sayısı ve süreye** göre filtrelenebilir; sıralama seçenekleri vardır.

### 6. Kullanıcı Profil Alanı
- Genel bakış (yaklaşan seyahat, istatistikler)
- Profil bilgilerini güncelleme
- **Rezervasyonlarım** (yaklaşan / geçmiş, iptal)
- **Favori Turlarım**
- **Yorumlarım** (silme)
- **Sorularım** (cevap durumu)

### 7. Admin Dashboard
- Toplam / aktif / pasif tur, toplam rezervasyon ve kullanıcı, bekleyen soru sayısı
- Bu ay yapılan rezervasyon sayısı
- En çok rezervasyon alan tur ve en yüksek gelir sağlayan tur
- **Son 6 aylık rezervasyonlar Chart.js grafiği**
- Kategori bazında tur sayısı ve destinasyon bazında rezervasyon grafikleri
- Son rezervasyonlar, son yorumlar, yanıt bekleyen sorular

### 8. MongoDB Aggregation
Aşağıdaki istatistikler `Find()` yerine **Aggregation Pipeline** ile hazırlanır (`StatisticService`):
- En çok rezervasyon alan 5 tur
- Kategori bazında tur sayısı
- Destinasyon bazında rezervasyon sayısı (`$lookup` ile)
- Aylık rezervasyon sayıları
- En yüksek gelir sağlayan tur
- Ortalama tur puanı

### 9. Tur ve Rezervasyon Yönetimi (Admin)
- Turlar için **aktif / pasif yapma** butonu
- Yeni **Rezervasyonlar** sayfası: tur, tur tarihi, kullanıcı, tarih aralığı ve duruma göre filtreleme; onaylama ve iptal etme

### 10. Tur Katılımcı Raporu
- Admin tur listesinde ve rezervasyon sayfasında **Rapor Al** (Excel / PDF) butonları
- Rapor alanları: Ad Soyad, E-posta, Telefon, Tur, Tur Tarihi, Yetişkin, Çocuk, Toplam Kişi, Rezervasyon Tarihi, Toplam Ücret, Durum
- Excel **ClosedXML**, PDF **QuestPDF** ile üretilir
- Dosya sunucuda saklanmadan **MemoryStream** üzerinden doğrudan indirilir
- Rapor tüm tur için veya **seçilen tur tarihine göre** alınabilir

### 11. Çoklu Dil Desteği (TR / EN)
- ASP.NET Core Localization; menüdeki **TR / EN** seçicisi ile dil çerezde saklanır
- Menüler, butonlar, başlıklar ve sistem mesajları seçilen dile göre değişir (`Resources/SharedResource.en.resx`)
- Tur adı ve açıklamaları MongoDB'de çok dilli alanlarda tutulur (`NameEn`, `ShortDescriptionEn`, `DescriptionEn`); admin tur ekleme/düzenleme formlarından girilir

### 12. Validasyon
FluentValidation ile: rezervasyon, yorum, soru ve soru cevabı formları (mevcut tur, kategori, destinasyon vb. validasyonlarına ek olarak).

### 13. Veri Uyumluluğu
- Eski kayıtlarda metin olarak tutulan durumlar (`"Published"`, `"Draft"` ...) okunabilir hale getirildi; uygulama açılışta bunları sayısal değere normalize eder.
- Eski kayıtlarda `Id` alanında tutulan tur tarihi kimlikleri de okunur ve kontenjan sorgularında eşleştirilir.

---

## Bilinen Eksikler
- Bazı eski admin CRUD sayfaları (Hakkımızda, İletişim vb.) yalnızca Türkçe.
- Yorumlar case'teki `Comments` adı yerine projede mevcut olan `Reviews` koleksiyonunda tutulur.

## Proje Yapısı (öne çıkanlar)

```
Travel.Web/
├── Areas/Admin/            # Admin paneli (Dashboard, Tour, Reservation, Question ...)
├── Controllers/            # Tours, Profile, Auth, Culture ...
├── DTOs/                   # Reservation, Statistic, Tour, Review, Question ...
├── Entities/               # Tour (nested TourDate / DayProgram), Reservation, Favorite ...
├── Helpers/                # TourLocalizer, FlexibleEnumSerializer
├── Resources/              # SharedResource.en.resx (İngilizce çeviriler)
├── Services/               # Reservation, Favorite, Statistic (aggregation), Report (Excel/PDF) ...
├── Validations/            # FluentValidation kuralları
└── Views/                  # Razor view'lar ve ViewComponent'ler
```
