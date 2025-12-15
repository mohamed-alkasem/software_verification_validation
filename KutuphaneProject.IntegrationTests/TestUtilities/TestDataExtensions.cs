using KutuphaneProject.Models;

namespace KutuphaneProject.IntegrationTests.TestUtilities
{
    public static class TestDataExtensions
    {
        // دالة لإنشاء صورة افتراضية
        public static byte[] CreateDefaultImage()
        {
            // صورة صغيرة جداً (1x1 pixel transparent PNG) كـ base64
            return Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==");
        }

        // إنشاء كائن Kategori مع بيانات اختبارية
        public static Kategori CreateTestKategori(int id, string ad)
        {
            return new Kategori
            {
                Id = id,
                Ad = ad,
                Image = CreateDefaultImage()
            };
        }

        // إنشاء كائن Kitap مع بيانات اختبارية
        public static Kitap CreateTestKitap(int id, string ad, int kategoriId)
        {
            return new Kitap
            {
                Id = id,
                Ad = ad,
                KategoriId = kategoriId,
                Aciklama = $"Test açıklama {id}",
                Image = CreateDefaultImage(),
                EklemeTarihi = DateTime.Now.AddDays(-id)
            };
        }

        // إنشاء كائن Ogrenci مع بيانات اختبارية
        public static Ogrenci CreateTestOgrenci(int id, string adSoyad, string ogrenciNo, int borc = 0)
        {
            return new Ogrenci
            {
                Id = id,
                AdSoyad = adSoyad,
                OgrenciNo = ogrenciNo,
                BorcMiktari = borc
            };
        }
    }
}