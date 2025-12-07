using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using KutuphaneProject.Services.Classes;

namespace MyApp.UnitTests.ServicesTests
{
    public class KitapServiceTests
    {
        private readonly Mock<KutuphaneDbContext> _mockContext;
        private readonly KitapService _service;

        public KitapServiceTests()
        {
            _mockContext = new Mock<KutuphaneDbContext>();
            _service = new KitapService(_mockContext.Object);
        }

        [Fact]
        public async Task GetKitapById_ExistingId_Should_Return_Kitap()
        {
            // Arrange
            var kitapId = 1;
            var expectedKitap = new Kitap { Id = kitapId, Ad = "Test Kitap" };
            var mockSet = new Mock<DbSet<Kitap>>();

            mockSet.Setup(m => m.FindAsync(kitapId)).ReturnsAsync(expectedKitap);
            _mockContext.Setup(c => c.Kitaplar).Returns(mockSet.Object);

            // Act
            var result = await _service.GetKitapById(kitapId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(kitapId);
            result.Ad.Should().Be("Test Kitap");
        }

        [Fact]
        public async Task GetKitapById_NonExistingId_Should_Return_Empty_Kitap()
        {
            // Arrange
            var kitapId = 999;
            var mockSet = new Mock<DbSet<Kitap>>();

            mockSet.Setup(m => m.FindAsync(kitapId)).ReturnsAsync((Kitap)null);
            _mockContext.Setup(c => c.Kitaplar).Returns(mockSet.Object);

            // Act
            var result = await _service.GetKitapById(kitapId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Kitap>();
            result.Id.Should().Be(0); // Default empty Kitap
        }

        [Fact]
        public async Task GetKitaplar_Should_Return_All_Kitaplar()
        {
            // Arrange
            var kitaplar = new List<Kitap>
            {
                new Kitap { Id = 1, Ad = "Kitap 1" },
                new Kitap { Id = 2, Ad = "Kitap 2" }
            };

            var mockSet = new Mock<DbSet<Kitap>>();
            mockSet.As<IQueryable<Kitap>>().Setup(m => m.Provider).Returns(kitaplar.AsQueryable().Provider);
            mockSet.As<IQueryable<Kitap>>().Setup(m => m.Expression).Returns(kitaplar.AsQueryable().Expression);
            mockSet.As<IQueryable<Kitap>>().Setup(m => m.ElementType).Returns(kitaplar.AsQueryable().ElementType);
            mockSet.As<IQueryable<Kitap>>().Setup(m => m.GetEnumerator()).Returns(kitaplar.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Kitaplar).Returns(mockSet.Object);

            // Act
            var result = await _service.GetKitaplar();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(k => k.Ad == "Kitap 1");
        }

        [Fact]
        public async Task KitapEkle_Should_Add_Kitap_And_SaveChanges()
        {
            // Arrange
            var yeniKitap = new Kitap { Ad = "Yeni Kitap" };
            var mockSet = new Mock<DbSet<Kitap>>();

            _mockContext.Setup(c => c.Kitaplar).Returns(mockSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _service.KitapEkle(yeniKitap);

            // Assert
            mockSet.Verify(m => m.AddAsync(yeniKitap, default), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task KitapSil_ExistingId_Should_Remove_Kitap()
        {
            // Arrange
            var kitapId = 1;
            var kitap = new Kitap { Id = kitapId, Ad = "Silinecek Kitap" };
            var mockSet = new Mock<DbSet<Kitap>>();

            mockSet.Setup(m => m.Find(kitapId)).Returns(kitap);
            _mockContext.Setup(c => c.Kitaplar).Returns(mockSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _service.KitapSil(kitapId);

            // Assert
            mockSet.Verify(m => m.Remove(kitap), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}