using Microsoft.EntityFrameworkCore;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;

namespace KutuphaneProject.IntegrationTests.TestUtilities
{
    public static class TestDbContextFactory
    {
        public static KutuphaneDbContext CreateContext()
        {
            // استخدم In-Memory Database تماماً
            var options = new DbContextOptionsBuilder<KutuphaneDbContext>()
                .UseInMemoryDatabase(databaseName: "KutuphaneTestDB_" + Guid.NewGuid().ToString())
                .Options;

            var context = new KutuphaneDbContext(options);

            // لا تستخدم EnsureDeleted/EnsureCreated مع In-Memory
            // فقط أضف البيانات
            SeedTestData(context);

            return context;
        }

        public static void SeedTestData(KutuphaneDbContext context)
        {
            // 1. أولاً: تنظيف أي بيانات موجودة
            context.Kitaplar.RemoveRange(context.Kitaplar);
            context.Kategoriler.RemoveRange(context.Kategoriler);
            context.Ogrenciler.RemoveRange(context.Ogrenciler);
            context.SaveChanges();

            // 2. إنشاء صورة افتراضية بسيطة
            byte[] CreateDefaultImage()
            {
                // صورة PNG صغيرة جداً (1x1 pixel)
                return Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==");
            }

            var defaultImage = CreateDefaultImage();

            // 3. إضافة الفئات أولاً (لأن الكتب تحتاجها)
            var kategori1 = new Kategori
            {
                Id = 1,
                Ad = "Roman",
                Image = defaultImage
            };

            var kategori2 = new Kategori
            {
                Id = 2,
                Ad = "Bilim Kurgu",
                Image = defaultImage
            };

            context.Kategoriler.Add(kategori1);
            context.Kategoriler.Add(kategori2);
            context.SaveChanges(); // حفظ الفئات أولاً

            // 4. إضافة الكتب
            var kitaplar = new List<Kitap>
            {
                new Kitap
                {
                    Id = 1,
                    Ad = "Suc ve Ceza",
                    KategoriId = 1, // مرتبط بـ kategori1
                    Aciklama = "Test açıklama 1",
                    Image = defaultImage,
                    EklemeTarihi = DateTime.Now.AddDays(-10)
                },
                new Kitap
                {
                    Id = 2,
                    Ad = "Savas ve Baris",
                    KategoriId = 1,
                    Aciklama = "Test açıklama 2",
                    Image = defaultImage,
                    EklemeTarihi = DateTime.Now.AddDays(-5)
                },
                new Kitap
                {
                    Id = 3,
                    Ad = "Dune",
                    KategoriId = 2, // مرتبط بـ kategori2
                    Aciklama = "Test açıklama 3",
                    Image = defaultImage,
                    EklemeTarihi = DateTime.Now
                }
            };

            context.Kitaplar.AddRange(kitaplar);

            // 5. إضافة الطلاب (لا يوجد حقول مطلوبة)
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
                    BorcMiktari = 10
                }
            };

            context.Ogrenciler.AddRange(ogrenciler);

            // 6. حفظ كل شيء
            context.SaveChanges();
        }
    }
}