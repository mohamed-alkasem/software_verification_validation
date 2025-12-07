using KutuphaneProject.Models;
using System.Collections.Generic;

namespace MyApp.UnitTests.TestData
{
    public static class TestDataBuilder
    {
        // 1. بيانات اختبارية للكتب
        public static Kitap CreateTestKitap(
            int id = 1,
            string ad = "Test Kitap",
            string aciklama = "Test Açıklama",
            int kategoriId = 1)
        {
            return new Kitap
            {
                Id = id,
                Ad = ad,
                Aciklama = aciklama,
                EklemeTarihi = DateTime.Now,
                KategoriId = kategoriId,
                Image = new byte[] { 0x20, 0x20, 0x20, 0x20 }
            };
        }

        // 2. بيانات اختبارية للطلاب
        public static Ogrenci CreateTestOgrenci(
            int id = 1,
            string ogrenciNo = "20230001",
            string adSoyad = "Test Öğrenci",
            int borcMiktari = 0)
        {
            return new Ogrenci
            {
                Id = id,
                OgrenciNo = ogrenciNo,
                AdSoyad = adSoyad,
                BorcMiktari = borcMiktari
            };
        }

        // 3. قائمة كتب اختبارية
        public static List<Kitap> CreateTestKitaplar(int count = 3)
        {
            var kitaplar = new List<Kitap>();

            for (int i = 1; i <= count; i++)
            {
                kitaplar.Add(CreateTestKitap(
                    id: i,
                    ad: $"Test Kitap {i}",
                    kategoriId: (i % 2) + 1 // KategoriId: 1 veya 2
                ));
            }

            return kitaplar;
        }

        // 4. قائمة طلاب اختبارية
        public static List<Ogrenci> CreateTestOgrenciler(int count = 3)
        {
            var ogrenciler = new List<Ogrenci>();

            for (int i = 1; i <= count; i++)
            {
                ogrenciler.Add(CreateTestOgrenci(
                    id: i,
                    ogrenciNo: $"2023{i:D4}",
                    adSoyad: $"Öğrenci {i}"
                ));
            }

            return ogrenciler;
        }

        // 5. بيانات للاختبارات المتقدمة
        public static Kitap CreateKitapWithImage()
        {
            var kitap = CreateTestKitap();

            // صورة اختبارية (صورة PNG صغيرة)
            kitap.Image = new byte[] {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
                0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52
            };

            return kitap;
        }

        // 6. طالب مع دين
        public static Ogrenci CreateOgrenciWithBorc(int borc = 50)
        {
            var ogrenci = CreateTestOgrenci();
            ogrenci.BorcMiktari = borc;
            return ogrenci;
        }
    }
}