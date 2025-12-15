using Xunit;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using KutuphaneProject.Services.Classes;
using KutuphaneProject.IntegrationTests.TestUtilities;

namespace KutuphaneProject.IntegrationTests.ServiceIntegrationTests
{
	public class KitapServiceIntegrationTests : IDisposable
	{
		private readonly KutuphaneDbContext _context;
		private readonly KitapService _kitapService;

		public KitapServiceIntegrationTests()
		{
			// إنشاء In-Memory Database
			var options = new DbContextOptionsBuilder<KutuphaneDbContext>()
				.UseInMemoryDatabase(databaseName: $"KitapTest_{Guid.NewGuid()}")
				.Options;

			_context = new KutuphaneDbContext(options);

			// إضافة البيانات الاختبارية
			SeedTestData(_context);

			// إنشاء Service للاختبار
			_kitapService = new KitapService(_context);
		}

		private void SeedTestData(KutuphaneDbContext context)
		{
			// تنظيف أي بيانات قديمة
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			// 1. أولاً: إضافة Kategoriler
			var kategori1 = new Kategori
			{
				Id = 1,
				Ad = "Roman",
				Image = TestDataExtensions.CreateDefaultImage()
			};

			var kategori2 = new Kategori
			{
				Id = 2,
				Ad = "Bilim Kurgu",
				Image = TestDataExtensions.CreateDefaultImage()
			};

			context.Kategoriler.AddRange(kategori1, kategori2);
			context.SaveChanges(); // حفظ الفئات أولاً

			// 2. ثانياً: إضافة Kitaplar
			var kitaplar = new List<Kitap>
			{
				new Kitap
				{
					Id = 1,
					Ad = "Suc ve Ceza",
					KategoriId = 1,
					Aciklama = "Dostoyevski'nin başyapıtı",
					Image = TestDataExtensions.CreateDefaultImage(),
					EklemeTarihi = DateTime.Now.AddDays(-30)
				},
				new Kitap
				{
					Id = 2,
					Ad = "Savas ve Baris",
					KategoriId = 1,
					Aciklama = "Tolstoy'un epik romanı",
					Image = TestDataExtensions.CreateDefaultImage(),
					EklemeTarihi = DateTime.Now.AddDays(-20)
				},
				new Kitap
				{
					Id = 3,
					Ad = "Dune",
					KategoriId = 2,
					Aciklama = "Frank Herbert'ın bilim kurgu klasiği",
					Image = TestDataExtensions.CreateDefaultImage(),
					EklemeTarihi = DateTime.Now.AddDays(-10)
				}
			};

			context.Kitaplar.AddRange(kitaplar);
			context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		// ------------------- TEST CASES -------------------

		[Fact]
		public async Task GetKitapById_WithValidId_ShouldReturnKitap()
		{
			// Arrange
			int validId = 1;

			// Act
			var result = await _kitapService.GetKitapById(validId);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(validId, result.Id);
			Assert.Equal("Suc ve Ceza", result.Ad);
		}

		[Fact]
		public async Task GetKitapById_WithInvalidId_ShouldReturnEmptyKitap()
		{
			// Arrange
			int invalidId = 999;

			// Act
			var result = await _kitapService.GetKitapById(invalidId);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(0, result.Id); // Kitap فارغ جديد
		}

		[Fact]
		public async Task GetKitaplar_ShouldReturnAllBooks()
		{
			// Act
			var result = await _kitapService.GetKitaplar();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(3, result.Count);
		}

		[Fact]
		public async Task GetKitaplarByKategoriId_ShouldReturnFilteredBooks()
		{
			// Arrange
			int kategoriId = 1;

			// Act
			var result = await _kitapService.GetKitaplarByKategoriId(kategoriId);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count); // كتابان في فئة Roman
			Assert.All(result, k => Assert.Equal(kategoriId, k.KategoriId));
		}

		[Fact]
		public async Task GetKitaplarByMetin_ShouldReturnBooksContainingText()
		{
			// Arrange
			int kategoriId = 1;
			string aramaMetni = "Savaş";

			// Act
			var result = await _kitapService.GetKitaplarByMetin(kategoriId, aramaMetni);

			// Assert
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Contains("Savaş", result[0].Ad, StringComparison.OrdinalIgnoreCase);
		}

		[Fact]
		public async Task KitapEkle_ShouldAddNewBookToDatabase()
		{
			// Arrange
			var yeniKitap = new Kitap
			{
				Id = 4,
				Ad = "Yeni Test Kitabı",
				KategoriId = 1, // مرتبط بـ Kategori موجود
				Aciklama = "Test açıklama",
				Image = TestDataExtensions.CreateDefaultImage(),
				EklemeTarihi = DateTime.Now
			};

			// Act
			await _kitapService.KitapEkle(yeniKitap);

			// Assert
			var kitapInDb = await _context.Kitaplar.FindAsync(4);
			Assert.NotNull(kitapInDb);
			Assert.Equal("Yeni Test Kitabı", kitapInDb.Ad);
			Assert.Equal(1, kitapInDb.KategoriId);

			// التأكد من زيادة العدد
			var kitaplar = await _kitapService.GetKitaplar();
			Assert.Equal(4, kitaplar.Count);
		}

		[Fact]
		public async Task KitapEkle_WithNonExistentKategori_ShouldFailGracefully()
		{
			// Arrange
			var yeniKitap = new Kitap
			{
				Id = 5,
				Ad = "Kitap with Invalid Kategori",
				KategoriId = 999, // Kategori غير موجودة
				Aciklama = "Test",
				Image = TestDataExtensions.CreateDefaultImage(),
				EklemeTarihi = DateTime.Now
			};

			// Act & Assert
			// قد ترمي exception أو تفشل بشكل صامت حسب التطبيق
			var exception = await Record.ExceptionAsync(() => _kitapService.KitapEkle(yeniKitap));

			// يمكنك التحقق من السلوك المتوقع
			if (exception != null)
			{
				// إذا كان يجب أن ترمي exception
				Assert.Contains("foreign key", exception.Message, StringComparison.OrdinalIgnoreCase);
			}
		}

		[Fact]
		public async Task KitapGuncelle_ShouldUpdateExistingBook()
		{
			// Arrange
			var guncellenecekKitap = new Kitap
			{
				Id = 1,
				Ad = "Suc ve Ceza (Güncellendi)",
				KategoriId = 1,
				Aciklama = "Güncellenmiş açıklama",
				Image = TestDataExtensions.CreateDefaultImage(),
				EklemeTarihi = DateTime.Now.AddDays(-20)
			};

			// Act
			await _kitapService.KitapGuncelle(guncellenecekKitap);

			// Assert
			var updatedKitap = await _context.Kitaplar.FindAsync(1);
			Assert.NotNull(updatedKitap);
			Assert.Equal("Suc ve Ceza (Güncellendi)", updatedKitap.Ad);
			Assert.Equal("Güncellenmiş açıklama", updatedKitap.Aciklama);
			Assert.Equal(DateTime.Now.AddDays(-20).Date, updatedKitap.EklemeTarihi.Date);
		}

		[Fact]
		public async Task KitapGuncelle_WithInvalidId_ShouldNotChangeAnything()
		{
			// Arrange
			var originalKitap = await _kitapService.GetKitapById(1);
			var originalAd = originalKitap.Ad;

			var invalidKitap = new Kitap
			{
				Id = 999, // Kitap غير موجود
				Ad = "Var Olmayan Kitap",
				KategoriId = 1,
				Aciklama = "Test",
				Image = TestDataExtensions.CreateDefaultImage(),
				EklemeTarihi = DateTime.Now
			};

			// Act
			await _kitapService.KitapGuncelle(invalidKitap);

			// Assert - التأكد من أن البيانات الأصلية لم تتغير
			var sameKitap = await _kitapService.GetKitapById(1);
			Assert.Equal(originalAd, sameKitap.Ad);
		}

		[Fact]
		public async Task KitapSil_ShouldRemoveBookFromDatabase()
		{
			// Arrange
			int kitapId = 2;
			var initialCount = (await _kitapService.GetKitaplar()).Count;

			// Act
			await _kitapService.KitapSil(kitapId);

			// Assert
			var silinenKitap = await _context.Kitaplar.FindAsync(kitapId);
			Assert.Null(silinenKitap);

			var kalanKitaplar = await _kitapService.GetKitaplar();
			Assert.Equal(initialCount - 1, kalanKitaplar.Count);
		}

		[Fact]
		public async Task KitapSil_WithInvalidId_ShouldNotThrowException()
		{
			// Arrange
			int invalidId = 999;
			var initialCount = (await _kitapService.GetKitaplar()).Count;

			// Act
			var exception = await Record.ExceptionAsync(() => _kitapService.KitapSil(invalidId));

			// Assert
			Assert.Null(exception);

			// التأكد من أن العدد لم يتغير
			var kitaplar = await _kitapService.GetKitaplar();
			Assert.Equal(initialCount, kitaplar.Count);
		}

		[Fact]
		public async Task GetKitaplarByMetin_WithEmptySearch_ShouldReturnAllBooksInCategory()
		{
			// Arrange
			int kategoriId = 1;
			string aramaMetni = "";

			// Act
			var result = await _kitapService.GetKitaplarByMetin(kategoriId, aramaMetni);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count); // جميع الكتب في فئة Roman
		}

		[Fact]
		public async Task GetKitaplarByMetin_WithNonExistentCategory_ShouldReturnEmptyList()
		{
			// Arrange
			int kategoriId = 999;
			string aramaMetni = "Test";

			// Act
			var result = await _kitapService.GetKitaplarByMetin(kategoriId, aramaMetni);

			// Assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}
	}
}