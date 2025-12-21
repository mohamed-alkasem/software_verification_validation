using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using KutuphaneProject.Services.Classes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KutuphaneProject.IntegrationTests.ServicesIntegrationTests
{
    public class OduncServiceIntegrationTests : IDisposable
    {
        private readonly KutuphaneDbContext _context;
        private readonly OduncService _oduncService;
        private readonly KitapService _kitapService;
        private readonly OgrenciService _ogrenciService;

        public OduncServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<KutuphaneDbContext>()
                .UseInMemoryDatabase($"OduncTest_{Guid.NewGuid()}")
                .Options;

            _context = new KutuphaneDbContext(options);

            _oduncService = new OduncService(_context);
            _kitapService = new KitapService(_context);
            _ogrenciService = new OgrenciService(_context);

            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // Eski verileri temizle
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // 1. Kategori ekle - ✅ Image EKLE
            var kategori = new Kategori
            {
                Ad = "Test Kategori",
                Image = new byte[] { 1, 2, 3, 4, 5 } // ✅ Image ekle (byte array)
            };
            _context.Kategoriler.Add(kategori);
            _context.SaveChanges();

            // 2. Öğrenci ekle
            var ogrenci = new Ogrenci
            {
                OgrenciNo = "TEST001",
                AdSoyad = "Test Öğrenci",
                BorcMiktari = 0
            };
            _context.Ogrenciler.Add(ogrenci);

            // 3. Kitap ekle - ✅ Image EKLE (eğer Kitap'ta da Image required ise)
            var kitap = new Kitap
            {
                Ad = "Test Kitap",
                Aciklama = "Test Kitap Açıklama",
                EklemeTarihi = DateTime.Now,
                KategoriId = kategori.Id,
                Image = new byte[] { 6, 7, 8, 9, 10 } // ✅ Image ekle
            };
            _context.Kitaplar.Add(kitap);

            _context.SaveChanges();
        }

        [Fact]
        public async Task OduncEkle_GecerliOduncIle_OduncOlusturur()
        {
            // Arrange
            var ogrenci = await _ogrenciService.GetOgrenciByOgrenciNo("TEST001");
            var kitaplar = await _kitapService.GetKitaplar();
            var kitap = kitaplar.First();

            var yeniOdunc = new Odunc
            {
                OgrenciId = ogrenci.Id,
                KitapId = kitap.Id,
                Name = kitap.Ad,
                KartNo = "1234567890123456",
                AlinmaTarihi = DateTime.Now,
                GeriDonusTarihi = DateTime.Now.AddDays(10),
                KalanSure = 10,
                BorcMiktari = 0,
                KontrolTarihi = DateTime.Now
            };

            // Act
            await _oduncService.OduncEkle(yeniOdunc);

            // Assert
            var oduncler = await _oduncService.GetOduncler();
            Assert.Single(oduncler);
            Assert.Equal(kitap.Ad, oduncler.First().Name);
        }

        // ... Diğer testler aynı şekilde
        // Sadece her Odunc için KartNo eklediğinizden emin olun

        public void Dispose()
        {
            _context?.Database?.EnsureDeleted();
            _context?.Dispose();
        }
    }
}