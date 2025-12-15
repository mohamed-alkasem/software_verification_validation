using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using KutuphaneProject.IntegrationTests.TestUtilities;

namespace KutuphaneProject.IntegrationTests.DatabaseTests
{
    public class GercekSqlServerIntegrationTests : IDisposable
    {
        private readonly KutuphaneDbContext _context;
        private readonly string _testDbName;

        public GercekSqlServerIntegrationTests()
        {
            // 1. appsettings.Test.json dosyasından bağlantı dizesini oku
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
                .Build();

            var baseConnectionString = configuration.GetConnectionString("TestConnection");

            // 2. Her test için benzersiz bir veritabanı adı oluştur
            _testDbName = $"Kutuphane_Test_{Guid.NewGuid().ToString("N").Substring(0, 8)}";

            // 3. Bağlantı dizesindeki veritabanı adını değiştir
            var testConnectionString = baseConnectionString.Replace(
                "Database=Kutuphane_Test",
                $"Database={_testDbName}");

            // 4. Test için DbContext oluştur
            var options = new DbContextOptionsBuilder<KutuphaneDbContext>()
                .UseSqlServer(testConnectionString)
                .Options;

            _context = new KutuphaneDbContext(options);

            // 5. Veritabanı ve tabloları oluştur
            _context.Database.EnsureDeleted(); // Önce sil
            _context.Database.EnsureCreated(); // Sonra oluştur

            // 6. Temel test verilerini ekle
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // ✅ DÜZELTME: Id'leri SİL! SQL Server otomatik atayacak
            var kategori1 = new Kategori
            {
                // Id = 1, ❌ SİL BUNU!
                Ad = "Roman",
                Image = TestDataExtensions.CreateDefaultImage()
            };

            var kategori2 = new Kategori
            {
                // Id = 2, ❌ SİL BUNU!
                Ad = "Bilim Kurgu",
                Image = TestDataExtensions.CreateDefaultImage()
            };

            _context.Kategoriler.AddRange(kategori1, kategori2);
            _context.SaveChanges(); // SQL Server Id'leri otomatik atayacak: 1, 2, ...
        }

        [Fact]
        public void SQLServer_Baglantisi_Kurulabiliyor_Mu()
        {
            // Arrange & Act
            var baglantiKuruldu = _context.Database.CanConnect();

            // Assert
            Assert.True(baglantiKuruldu, "SQL Server'a bağlanılamadı.");
        }

        [Fact]
        public void Kitap_Tablosu_Olusturulup_Veri_Ekleme_Yapilabiliyor_Mu()
        {
            // Arrange
            var kategoriler = _context.Kategoriler.ToList();
            Assert.NotEmpty(kategoriler); // Kategori olmalı

            var yeniKitap = new Kitap
            {
                Ad = "Integration Test Kitabı",
                Aciklama = "Bu kitap sadece test amaçlıdır",
                KategoriId = kategoriler.First().Id, // ✅ Mevcut kategori ID'si
                EklemeTarihi = DateTime.Now,
                Image = TestDataExtensions.CreateDefaultImage()
            };

            // Act
            _context.Kitaplar.Add(yeniKitap);
            var kaydedilenSayi = _context.SaveChanges();

            // Assert
            Assert.Equal(1, kaydedilenSayi);

            var veritabanindakiKitaplar = _context.Kitaplar.ToList();
            Assert.Single(veritabanindakiKitaplar);
            Assert.Equal("Integration Test Kitabı", veritabanindakiKitaplar[0].Ad);
        }

        [Fact]
        public void Kitap_Uzerinde_CRUD_Islemleri_Yapilabiliyor_Mu()
        {
            // Arrange - Kategori olmalı
            var kategori = _context.Kategoriler.First();

            // OLUŞTURMA (Create)
            var yeniKitap = new Kitap
            {
                Ad = "Yeni Kitap",
                KategoriId = kategori.Id,
                Aciklama = "Test açıklama",
                EklemeTarihi = DateTime.Now,
                Image = TestDataExtensions.CreateDefaultImage()
            };
            _context.Kitaplar.Add(yeniKitap);
            _context.SaveChanges();

            var baslangicSayisi = _context.Kitaplar.Count();

            // OKUMA (Read)
            var kitaplar = _context.Kitaplar.Where(k => k.Ad.Contains("Yeni")).ToList();
            Assert.Single(kitaplar);

            var kitapId = kitaplar.First().Id; // ✅ SQL Server'ın atadığı ID

            // GÜNCELLEME (Update)
            var guncellenecekKitap = _context.Kitaplar.Find(kitapId);
            Assert.NotNull(guncellenecekKitap);

            guncellenecekKitap.Ad = "Güncellenmiş Kitap";
            guncellenecekKitap.Aciklama = "Güncellenmiş açıklama";
            _context.SaveChanges();

            var guncellenmisKitap = _context.Kitaplar.Find(kitapId);
            Assert.Equal("Güncellenmiş Kitap", guncellenmisKitap.Ad);

            // SİLME (Delete)
            _context.Kitaplar.Remove(guncellenmisKitap);
            _context.SaveChanges();

            var sonSayi = _context.Kitaplar.Count();
            Assert.Equal(baslangicSayisi - 1, sonSayi);
        }

        [Fact]
        public void LINQ_Sorgulari_Calisiyor_Mu()
        {
            // Arrange - Kategori olmalı
            var kategori = _context.Kategoriler.First();

            // Test verileri ekle
            _context.Kitaplar.AddRange(
                new Kitap
                {
                    Ad = "Test Kitap 1",
                    KategoriId = kategori.Id,
                    Aciklama = "Açıklama 1",
                    EklemeTarihi = DateTime.Now,
                    Image = TestDataExtensions.CreateDefaultImage()
                },
                new Kitap
                {
                    Ad = "Test Kitap 2",
                    KategoriId = kategori.Id,
                    Aciklama = "Açıklama 2",
                    EklemeTarihi = DateTime.Now,
                    Image = TestDataExtensions.CreateDefaultImage()
                }
            );
            _context.SaveChanges();

            // Act - LINQ sorgusu çalıştır
            var kitaplar = _context.Kitaplar
                .Where(k => k.KategoriId == kategori.Id)
                .OrderBy(k => k.Ad)
                .ToList();

            // Assert
            Assert.Equal(2, kitaplar.Count);
            Assert.Equal("Test Kitap 1", kitaplar[0].Ad);
            Assert.Equal("Test Kitap 2", kitaplar[1].Ad);
        }

        [Fact]
        public void Ogrenci_Ve_Kitap_Iliskisi_Calisiyor_Mu()
        {
            // Arrange
            var kategori = _context.Kategoriler.First();

            var ogrenci = new Ogrenci
            {
                OgrenciNo = "TEST2024001",
                AdSoyad = "Test Öğrenci",
                BorcMiktari = 0
            };

            var kitap = new Kitap
            {
                Ad = "Ödünç Test Kitabı",
                KategoriId = kategori.Id,
                Aciklama = "Test kitabı açıklaması",
                EklemeTarihi = DateTime.Now,
                Image = TestDataExtensions.CreateDefaultImage()
            };

            _context.Ogrenciler.Add(ogrenci);
            _context.Kitaplar.Add(kitap);
            _context.SaveChanges();

            // Act
            var kayitliOgrenci = _context.Ogrenciler
                .FirstOrDefault(o => o.OgrenciNo == "TEST2024001");

            var kayitliKitap = _context.Kitaplar
                .FirstOrDefault(k => k.Ad == "Ödünç Test Kitabı");

            // Assert
            Assert.NotNull(kayitliOgrenci);
            Assert.NotNull(kayitliKitap);
            Assert.Equal("Test Öğrenci", kayitliOgrenci.AdSoyad);
            Assert.Equal("Ödünç Test Kitabı", kayitliKitap.Ad);
        }

        [Fact]
        public void Kategori_Tablosu_Calisiyor_Mu()
        {
            // Arrange
            var mevcutKategoriSayisi = _context.Kategoriler.Count();

            var yeniKategori = new Kategori
            {
                // Id = 3, ❌ SİL BUNU!
                Ad = "Tarih",
                Image = TestDataExtensions.CreateDefaultImage()
            };

            // Act
            _context.Kategoriler.Add(yeniKategori);
            _context.SaveChanges();

            // Assert
            var kategoriler = _context.Kategoriler.ToList();
            Assert.Equal(mevcutKategoriSayisi + 1, kategoriler.Count);
            Assert.Contains(kategoriler, k => k.Ad == "Tarih");
        }

        [Fact]
        public void Kitap_Kategori_Iliskisi_Calisiyor_Mu()
        {
            // Arrange
            var kategori = _context.Kategoriler.First(k => k.Ad == "Bilim Kurgu");

            var kitap = new Kitap
            {
                Ad = "İlişki Test Kitabı",
                KategoriId = kategori.Id,
                Aciklama = "Kategori ilişkisi testi",
                EklemeTarihi = DateTime.Now,
                Image = TestDataExtensions.CreateDefaultImage()
            };
            _context.Kitaplar.Add(kitap);
            _context.SaveChanges();

            // Act
            var kayitliKitap = _context.Kitaplar
                .Include(k => k.KategoriFK)
                .FirstOrDefault(k => k.Ad == "İlişki Test Kitabı");

            // Assert
            Assert.NotNull(kayitliKitap);
            Assert.NotNull(kayitliKitap.KategoriFK);
            Assert.Equal("Bilim Kurgu", kayitliKitap.KategoriFK.Ad);
        }

        [Fact]
        public void Transaction_Calisiyor_Mu()
        {
            // Arrange
            var kategori = _context.Kategoriler.First();
            var baslangicKitapSayisi = _context.Kitaplar.Count();

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Act - İki kitap ekle
                _context.Kitaplar.Add(new Kitap
                {
                    Ad = "Transaction Kitap 1",
                    KategoriId = kategori.Id,
                    Aciklama = "Test",
                    EklemeTarihi = DateTime.Now,
                    Image = TestDataExtensions.CreateDefaultImage()
                });

                _context.Kitaplar.Add(new Kitap
                {
                    Ad = "Transaction Kitap 2",
                    KategoriId = kategori.Id,
                    Aciklama = "Test",
                    EklemeTarihi = DateTime.Now,
                    Image = TestDataExtensions.CreateDefaultImage()
                });

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            // Assert
            var sonKitapSayisi = _context.Kitaplar.Count();
            Assert.Equal(baslangicKitapSayisi + 2, sonKitapSayisi);
        }

        public void Dispose()
        {
            try
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Uyarı: Test veritabanı silinemedi: {ex.Message}");
            }
        }
    }
}