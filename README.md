========================================================
KÜTÜPHANE YÖNETİM SİSTEMİ - FİNAL ÖDEVİ TESLİM DOSYASI
========================================================

Proje Adı: Kütüphane Yönetim Sistemi
Versiyon: 1.0
Tarih: 2025

========================================================
DOSYA YAPISI
========================================================

Final_Proje_Teslim/
│
├── Asama1_Isveren/
│ ├── Project_Vision.pdf
│ ├── SRS.pdf
│ ├── User_Stories.pdf
│ └── Use_Case_Specs.pdf
│
├── Asama2_Gelistirici_QA/
│ ├── Software_Test_Plan.pdf
│ ├── Test_Cases.xlsx
│ ├── Traceability_Matrix.xlsx
│ ├── Unit_Test_Raporu.pdf
│ ├── Integration_Test_Raporu.pdf
│ ├── System_Test_Raporu.pdf
│ ├── UAT_Raporu.pdf
│ ├── Bug_Report.xlsx
│ └── Uygulama_Kodu/
│ ├── KutuphaneProject.sln
│ ├── KutuphaneProject/
│ ├── MyApp.UnitTests/
│ ├── KutuphaneProject.IntegrationTests/
│ └── Seleniumtet_Systemtest/
│
└── README.txt

========================================================
AŞAMA 1 - İŞVEREN (CLIENT) EKİBİ DOKÜMANLARI
========================================================

1. Project_Vision.pdf

   - Projenin amacı ve hedefleri
   - Hedef kullanıcılar
   - Ürünün sağlayacağı değer
   - İş hedefleri

2. SRS.pdf

   - IEEE 830 / IEEE 29148 uyumlu
   - Fonksiyonel gereksinimler
   - Fonksiyonel olmayan gereksinimler
   - Sistem arabirim gereksinimleri

3. User_Stories.pdf

   - 18 User Story
   - Given-When-Then acceptance criteria'ları

4. Use_Case_Specs.pdf
   - 7 Use Case detaylı açıklamaları
   - Normal ve alternatif akışlar

======================================================
AŞAMA 2 - GELİŞTİRİCİ & QA EKİBİ DOKÜMANLARI
======================================================

1. Software_Test_Plan.pdf

   - IEEE 829 uyumlu test planı
   - Test kapsamı ve stratejisi
   - Test ortamı ve risk analizi

2. Test_Cases.xlsx

   - Tüm test case'ler detaylı
   - Unit, Integration, System testleri

3. Traceability_Matrix.xlsx

   - Gereksinim → User Story → Test Case eşleştirmesi
   - Test durumu takibi

4. Unit_Test_Raporu.pdf

   - 28 birim test sonuçları
   - %53.57 başarı oranı

5. Integration_Test_Raporu.pdf

   - 28 entegrasyon test sonuçları
   - %100 başarı oranı

6. System_Test_Raporu.pdf

   - 7 Selenium sistem testi sonuçları
   - %100 başarı oranı

7. UAT_Raporu.pdf

   - 16 User Story test sonuçları
   - %100 başarı oranı
   - Kullanıcı onay formu

8. Bug_Report.xlsx
   - 4 hata raporu detaylı
   - Çözüm önerileri

=====================================================
UYGULAMA KODU
=====================================================

Uygulama_Kodu/ klasöründe:

- KutuphaneProject.sln (Solution dosyası)
- KutuphaneProject/ (Ana proje)
- MyApp.UnitTests/ (Birim testler)
- KutuphaneProject.IntegrationTests/ (Entegrasyon testleri)
- Seleniumtet_Systemtest/ (Selenium sistem testleri)

Teknoloji Stack:

- .NET 9.0
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- xUnit (Test Framework)
- Selenium WebDriver

=====================================================
ÇALIŞTIRMA TALİMATLARI
=====================================================

1. Uygulamayı Çalıştırma:

   - Visual Studio 2022 veya üzeri gerekli
   - SQL Server 2019+ gerekli
   - appsettings.json'da veritabanı bağlantı string'i ayarlanmalı
   - dotnet restore
   - dotnet build
   - dotnet run (veya F5)

2. Testleri Çalıştırma:
   - Unit Tests: dotnet test MyApp.UnitTests
   - Integration Tests: dotnet test KutuphaneProject.IntegrationTests
   - System Tests: dotnet test Seleniumtet_Systemtest
     (Önce uygulama çalışır durumda olmalı)

===================================================
NOTLAR
===================================================

- Tüm dokümanlar Türkçe olarak hazırlanmıştır
- IEEE standartlarına uyumlu dokümanlar
- Test sonuçları gerçek test çalıştırmalarından alınmıştır
- Selenium testleri uygulama çalışır durumda test edilmiştir

===================================================
İLETİŞİM
===================================================

Proje Ekibi:
MOHAMAD ALKASSEM - 2212721320
Mohammed Hayr Alemin Alvaki - 2212721311
Muhammed Sami Turhan - 2212721047

Test Ekibi:
MOHAMAD ALKASSEM
Mohammed Hayr Alemin Alvaki

====================================================
SON GÜNCELLEME: 2025
====================================================
