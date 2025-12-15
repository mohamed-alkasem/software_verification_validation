using KutuphaneProject.EFCore;
using KutuphaneProject.Models;

namespace KutuphaneProject.IntegrationTests.TestUtilities
{
    public static class TestDataSeeder
    {
        public static void SeedTestData(KutuphaneDbContext context)
        {
            // إضافة فئة
            var kategori = new Kategori { Id = 1, Ad = "Roman" };
            context.Kategoriler.Add(kategori);

            // إضافة كتب باستخدام الـ extension method
            var kitaplar = new List<Kitap>
    {
        TestDataExtensions.CreateTestKitap(1, "Suc ve Ceza", 1),
        TestDataExtensions.CreateTestKitap(2, "Savas ve Baris", 1),
        TestDataExtensions.CreateTestKitap(3, "Anna Karenina", 1)
    };
            context.Kitaplar.AddRange(kitaplar);

            // إضافة طلاب
            var ogrenciler = new List<Ogrenci>
    {
        TestDataExtensions.CreateTestOgrenci(1, "Ahmet Yılmaz", "2023001"),
        TestDataExtensions.CreateTestOgrenci(2, "Mehmet Demir", "2023002", 10)
    };
            context.Ogrenciler.AddRange(ogrenciler);

            context.SaveChanges();
        }
    }
}