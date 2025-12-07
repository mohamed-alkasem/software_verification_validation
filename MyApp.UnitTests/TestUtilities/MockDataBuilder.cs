using System.Linq.Expressions;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using KutuphaneProject.Services.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace MyApp.UnitTests.TestUtilities
{
    public static class MockDataBuilder
    {
        // 1. إنشاء Mock لـ DbSet (مصحح)
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            // Mock للـ FindAsync (مبسط)
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                  .ReturnsAsync((object[] ids) =>
                  {
                      var id = (int)ids[0];
                      return data.FirstOrDefault(d =>
                      {
                          var prop = d.GetType().GetProperty("Id");
                          return prop != null && (int)prop.GetValue(d) == id;
                      });
                  });

            // Mock للـ AddAsync (مبسط بدون EntityEntry.Mock)
            mockSet.Setup(m => m.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                  .Callback((T entity, CancellationToken token) =>
                  {
                      data.Add(entity);
                  })
                  .ReturnsAsync((T entity, CancellationToken token) =>
                  {
                      // إنشاء EntityEntry وهمي
                      var mockEntry = new Mock<EntityEntry<T>>();
                      return mockEntry.Object;
                  });

            return mockSet;
        }

        // 2. إنشاء Mock لـ KutuphaneDbContext (مبسط)
        public static Mock<KutuphaneDbContext> CreateMockKutuphaneDbContext()
        {
            var mockContext = new Mock<KutuphaneDbContext>();

            // بيانات وهمية للكتب
            var kitaplar = new List<Kitap>
            {
                new Kitap { Id = 1, Ad = "C# Programlama", KategoriId = 1 },
                new Kitap { Id = 2, Ad = "ASP.NET Core", KategoriId = 1 },
                new Kitap { Id = 3, Ad = "Entity Framework", KategoriId = 2 }
            };

            // بيانات وهمية للطلاب
            var ogrenciler = new List<Ogrenci>
            {
                new Ogrenci { Id = 1, OgrenciNo = "20230001", AdSoyad = "Ali Yılmaz" },
                new Ogrenci { Id = 2, OgrenciNo = "20230002", AdSoyad = "Ayşe Kaya" }
            };

            // إنشاء Mock DbSets
            var mockKitapSet = CreateMockDbSet(kitaplar);
            var mockOgrenciSet = CreateMockDbSet(ogrenciler);

            // إعداد الـ DbContext
            mockContext.Setup(c => c.Kitaplar).Returns(mockKitapSet.Object);
            mockContext.Setup(c => c.Ogrenciler).Returns(mockOgrenciSet.Object);

            // Mock لـ SaveChangesAsync
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(1);

            return mockContext;
        }

        // 3. طريقة مختصرة (مبسطة أكثر)
        public static KitapService CreateMockKitapService()
        {
            var mockContext = new Mock<KutuphaneDbContext>();
            var kitaplar = new List<Kitap>
            {
                new Kitap { Id = 1, Ad = "Test Kitap 1" },
                new Kitap { Id = 2, Ad = "Test Kitap 2" }
            };

            var mockSet = new Mock<DbSet<Kitap>>();
            var queryable = kitaplar.AsQueryable();

            mockSet.As<IQueryable<Kitap>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Kitap>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Kitap>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Kitap>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockContext.Setup(c => c.Kitaplar).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            return new KitapService(mockContext.Object);
        }

        public static OgrenciService CreateMockOgrenciService()
        {
            var mockContext = new Mock<KutuphaneDbContext>();
            var ogrenciler = new List<Ogrenci>
            {
                new Ogrenci { Id = 1, OgrenciNo = "20230001", AdSoyad = "Ali Yılmaz" },
                new Ogrenci { Id = 2, OgrenciNo = "20230002", AdSoyad = "Ayşe Kaya" }
            };

            var mockSet = new Mock<DbSet<Ogrenci>>();
            var queryable = ogrenciler.AsQueryable();

            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockContext.Setup(c => c.Ogrenciler).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            return new OgrenciService(mockContext.Object);
        }
    }
}