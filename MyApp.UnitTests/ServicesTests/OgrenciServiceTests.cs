using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using KutuphaneProject.Services.Classes;

namespace MyApp.UnitTests.ServicesTests
{
    public class OgrenciServiceTests
    {
        private readonly Mock<KutuphaneDbContext> _mockContext;
        private readonly OgrenciService _service;

        public OgrenciServiceTests()
        {
            _mockContext = new Mock<KutuphaneDbContext>();
            _service = new OgrenciService(_mockContext.Object);
        }

        [Fact]
        public async Task GetOgrenciById_ExistingId_Should_Return_Ogrenci()
        {
            // Arrange
            var ogrenciId = 1;
            var ogrenciler = new List<Ogrenci>
            {
                new Ogrenci { Id = 1, OgrenciNo = "20230001" },
                new Ogrenci { Id = 2, OgrenciNo = "20230002" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Ogrenci>>();
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.Provider).Returns(ogrenciler.Provider);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.Expression).Returns(ogrenciler.Expression);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.ElementType).Returns(ogrenciler.ElementType);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.GetEnumerator()).Returns(ogrenciler.GetEnumerator());

            _mockContext.Setup(c => c.Ogrenciler).Returns(mockSet.Object);

            // Act
            var result = await _service.GetOgrenciById(ogrenciId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.OgrenciNo.Should().Be("20230001");
        }

        [Fact]
        public async Task GetOgrenciByOgrenciNo_ExistingNo_Should_Return_Ogrenci()
        {
            // Arrange
            var ogrenciNo = "20230001";
            var ogrenciler = new List<Ogrenci>
            {
                new Ogrenci { Id = 1, OgrenciNo = "20230001" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Ogrenci>>();
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.Provider).Returns(ogrenciler.Provider);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.Expression).Returns(ogrenciler.Expression);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.ElementType).Returns(ogrenciler.ElementType);
            mockSet.As<IQueryable<Ogrenci>>().Setup(m => m.GetEnumerator()).Returns(ogrenciler.GetEnumerator());

            _mockContext.Setup(c => c.Ogrenciler).Returns(mockSet.Object);

            // Act
            var result = await _service.GetOgrenciByOgrenciNo(ogrenciNo);

            // Assert
            result.Should().NotBeNull();
            result.OgrenciNo.Should().Be(ogrenciNo);
        }

        [Fact]
        public async Task OgrenciEkle_Should_Add_Ogrenci_And_SaveChanges()
        {
            // Arrange
            var yeniOgrenci = new Ogrenci { OgrenciNo = "20230003", AdSoyad = "Yeni Öğrenci" };
            var mockSet = new Mock<DbSet<Ogrenci>>();

            _mockContext.Setup(c => c.Ogrenciler).Returns(mockSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _service.OgrenciEkle(yeniOgrenci);

            // Assert
            mockSet.Verify(m => m.AddAsync(yeniOgrenci, default), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}