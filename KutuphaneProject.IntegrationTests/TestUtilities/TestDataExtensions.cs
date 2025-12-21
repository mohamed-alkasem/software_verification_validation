using KutuphaneProject.Models;

namespace KutuphaneProject.IntegrationTests.TestUtilities
{
    public static class TestDataExtensions
    {
        // Varsayılan resim oluşturmak için fonksiyon
        public static byte[] CreateDefaultImage()
        {
            // Çok küçük resim (1x1 pixel şeffaf PNG) base64 olarak
            return Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==");
        }

        // Test verileri ile Kategori nesnesi oluştur
        public static Kategori CreateTestKategori(int id, string ad)
        {
            return new Kategori
            {
                Id = id,
                Ad = ad,
                Image = CreateDefaultImage()
            };
        }

        // Test verileri ile Kitap nesnesi oluştur
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

        // Test verileri ile Ogrenci nesnesi oluştur
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