using Xunit;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using KutuphaneProject.Services.Classes;
using KutuphaneProject.IntegrationTests.TestUtilities;

namespace KutuphaneProject.IntegrationTests.ServiceIntegrationTests
{
    public class OgrenciServiceIntegrationTests : IDisposable
    {
        private readonly KutuphaneDbContext _context;
        private readonly OgrenciService _ogrenciService;

        public OgrenciServiceIntegrationTests()
        {
            // إنشاء In-Memory Database مباشرة
            var options = new DbContextOptionsBuilder<KutuphaneDbContext>()
                .UseInMemoryDatabase(databaseName: $"OgrenciTest_{Guid.NewGuid()}")
                .Options;

            _context = new KutuphaneDbContext(options);
            SeedTestData(_context);
            _ogrenciService = new OgrenciService(_context);
        }

        private void SeedTestData(KutuphaneDbContext context)
        {
            // تنظيف أي بيانات قديمة
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // 1. أولاً: إضافة Kategoriler (مطلوبة حتى لو ما استخدمناها هنا)
            var kategori = new Kategori
            {
                Id = 1,
                Ad = "Roman",
                Image = TestDataExtensions.CreateDefaultImage()
            };
            context.Kategoriler.Add(kategori);
            context.SaveChanges();

            // 2. ثانياً: إضافة Ogrenciler
            var ogrenciler = new List<Ogrenci>
            {
                new Ogrenci
                {
                    Id = 1,
                    AdSoyad = "Ahmet Yılmaz",
                    OgrenciNo = "2023001",
                    BorcMiktari = 0
                },
                new Ogrenci
                {
                    Id = 2,
                    AdSoyad = "Mehmet Demir",
                    OgrenciNo = "2023002",
                    BorcMiktari = 10  // تم التعديل من 10.5m إلى 10 لأن BorcMiktari من نوع int
                }
            };

            context.Ogrenciler.AddRange(ogrenciler);
            context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // ------------------- TEST CASES -------------------

        [Fact]
        public async Task GetOgrenciById_WithValidId_ShouldReturnOgrenci()
        {
            // Arrange
            int validId = 1;

            // Act
            var result = await _ogrenciService.GetOgrenciById(validId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validId, result.Id);
            Assert.Equal("Ahmet Yılmaz", result.AdSoyad);
            Assert.Equal(0, result.BorcMiktari);
        }

        [Fact]
        public async Task GetOgrenciById_WithInvalidId_ShouldReturnEmptyOgrenci()
        {
            // Arrange
            int invalidId = 999;

            // Act
            var result = await _ogrenciService.GetOgrenciById(invalidId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id); // Ogrenci فارغ جديد
            Assert.Null(result.AdSoyad); // أو string.Empty
            Assert.Null(result.OgrenciNo);
        }

        [Fact]
        public async Task GetOgrenciByOgrenciNo_WithValidNo_ShouldReturnOgrenci()
        {
            // Arrange
            string validOgrenciNo = "2023002";

            // Act
            var result = await _ogrenciService.GetOgrenciByOgrenciNo(validOgrenciNo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Mehmet Demir", result.AdSoyad);
            Assert.Equal(10, result.BorcMiktari); // تم التعديل من 10.5m إلى 10
            Assert.Equal("2023002", result.OgrenciNo);
        }

        [Fact]
        public async Task GetOgrenciByOgrenciNo_WithInvalidNo_ShouldReturnEmptyOgrenci()
        {
            // Arrange
            string invalidOgrenciNo = "999999";

            // Act
            var result = await _ogrenciService.GetOgrenciByOgrenciNo(invalidOgrenciNo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
        }

        [Fact]
        public async Task GetOgrenciler_ShouldReturnAllStudents()
        {
            // Act
            var result = await _ogrenciService.GetOgrenciler();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            // التأكد من وجود البيانات الصحيحة
            var ogrenci1 = result.First(o => o.Id == 1);
            var ogrenci2 = result.First(o => o.Id == 2);

            Assert.Equal("Ahmet Yılmaz", ogrenci1.AdSoyad);
            Assert.Equal("Mehmet Demir", ogrenci2.AdSoyad);
        }

        [Fact]
        public async Task GetOgrenciler_ShouldReturnEmptyList_WhenNoStudents()
        {
            // Arrange
            _context.Ogrenciler.RemoveRange(_context.Ogrenciler);
            await _context.SaveChangesAsync();

            // Act
            var result = await _ogrenciService.GetOgrenciler();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task OgrenciEkle_ShouldAddNewStudentToDatabase()
        {
            // Arrange
            var yeniOgrenci = new Ogrenci
            {
                Id = 3,
                AdSoyad = "Ayşe Kaya",
                OgrenciNo = "2023003",
                BorcMiktari = 0
            };

            var initialCount = (await _ogrenciService.GetOgrenciler()).Count;

            // Act
            await _ogrenciService.OgrenciEkle(yeniOgrenci);

            // Assert
            var ogrenciInDb = await _context.Ogrenciler.FindAsync(3);
            Assert.NotNull(ogrenciInDb);
            Assert.Equal("Ayşe Kaya", ogrenciInDb.AdSoyad);
            Assert.Equal("2023003", ogrenciInDb.OgrenciNo);
            Assert.Equal(0, ogrenciInDb.BorcMiktari);

            // التأكد من زيادة العدد
            var tumOgrenciler = await _ogrenciService.GetOgrenciler();
            Assert.Equal(initialCount + 1, tumOgrenciler.Count);
        }

        [Fact]
        public async Task OgrenciEkle_WithDuplicateOgrenciNo_ShouldStillAdd()
        {
            // Arrange
            var duplicateOgrenci = new Ogrenci
            {
                Id = 4,
                AdSoyad = "Ali Veli",
                OgrenciNo = "2023001", // نفس رقم طالب موجود
                BorcMiktari = 0
            };

            // Act
            await _ogrenciService.OgrenciEkle(duplicateOgrenci);

            // Assert
            var ogrenciler = await _ogrenciService.GetOgrenciler();
            var ogrenciWithDuplicateNo = ogrenciler.Where(o => o.OgrenciNo == "2023001").ToList();

            // قد يكون مسموحاً بتكرار رقم الطالب أو لا، حسب منطق تطبيقك
            // هذا الاختبار فقط للتأكد من عدم حدوث exception
            Assert.True(ogrenciler.Count > 2);
        }

        [Fact]
        public async Task OgrenciyiGuncelle_ShouldUpdateExistingStudent()
        {
            // Arrange
            var guncellenecekOgrenci = new Ogrenci
            {
                Id = 1,
                AdSoyad = "Ahmet Yılmaz (Güncellendi)",
                OgrenciNo = "2023001",
                BorcMiktari = 5
            };

            // Act
            await _ogrenciService.OgrenciyiGuncelle(guncellenecekOgrenci);

            // Assert
            var updatedOgrenci = await _context.Ogrenciler.FindAsync(1);
            Assert.NotNull(updatedOgrenci);
            Assert.Equal("Ahmet Yılmaz (Güncellendi)", updatedOgrenci.AdSoyad);
            Assert.Equal(5, updatedOgrenci.BorcMiktari);
            Assert.Equal("2023001", updatedOgrenci.OgrenciNo); // يجب أن يبقى كما هو
        }

        [Fact]
        public async Task OgrenciyiGuncelle_WithInvalidId_ShouldNotUpdateAnything()
        {
            // Arrange
            var originalOgrenci = await _context.Ogrenciler.FindAsync(1);
            var originalAdSoyad = originalOgrenci.AdSoyad;
            var originalBorc = originalOgrenci.BorcMiktari;

            var invalidOgrenci = new Ogrenci
            {
                Id = 999,
                AdSoyad = "Var Olmayan Öğrenci",
                OgrenciNo = "999999",
                BorcMiktari = 100
            };

            // Act
            await _ogrenciService.OgrenciyiGuncelle(invalidOgrenci);

            // Assert - التأكد من أن البيانات الأصلية لم تتغير
            var sameOgrenci = await _context.Ogrenciler.FindAsync(1);
            Assert.Equal(originalAdSoyad, sameOgrenci.AdSoyad);
            Assert.Equal(originalBorc, sameOgrenci.BorcMiktari);
            Assert.Equal("2023001", sameOgrenci.OgrenciNo);
        }

        [Fact]
        public async Task OgrenciyiGuncelle_ShouldOnlyUpdateProvidedFields()
        {
            // Arrange
            // أولاً: احفظ البيانات الأصلية
            var originalOgrenci = await _ogrenciService.GetOgrenciById(2);

            var guncellenecekOgrenci = new Ogrenci
            {
                Id = 2,
                AdSoyad = "Mehmet Demir (Güncellendi)",
                // لم نزود OgrenciNo
                BorcMiktari = 20
            };

            // Act
            await _ogrenciService.OgrenciyiGuncelle(guncellenecekOgrenci);

            // Assert
            var updatedOgrenci = await _context.Ogrenciler.FindAsync(2);
            Assert.NotNull(updatedOgrenci);
            Assert.Equal("Mehmet Demir (Güncellendi)", updatedOgrenci.AdSoyad);
            Assert.Equal(20, updatedOgrenci.BorcMiktari);
            // OgrenciNo يجب أن يبقى كما هو لأننا لم نزوده
            Assert.Equal("2023002", updatedOgrenci.OgrenciNo);
        }

        [Fact]
        public async Task GetOgrenciByOgrenciNo_WithEmptyString_ShouldReturnEmptyOgrenci()
        {
            // Arrange
            string emptyOgrenciNo = "";

            // Act
            var result = await _ogrenciService.GetOgrenciByOgrenciNo(emptyOgrenciNo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
        }

        [Fact]
        public async Task GetOgrenciByOgrenciNo_WithNull_ShouldReturnEmptyOgrenci()
        {
            // Arrange
            string nullOgrenciNo = null;

            // Act
            var result = await _ogrenciService.GetOgrenciByOgrenciNo(nullOgrenciNo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
        }
    }
}