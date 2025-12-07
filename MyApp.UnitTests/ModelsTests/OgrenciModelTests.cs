using Xunit;
using FluentAssertions;
using KutuphaneProject.Models;

namespace MyApp.UnitTests.ModelsTests
{
    public class OgrenciModelTests
    {
        [Fact]
        public void Ogrenci_Properties_Should_Be_Set_Correctly()
        {
            // Arrange
            var ogrenci = new Ogrenci
            {
                Id = 1,
                OgrenciNo = "20230001",
                AdSoyad = "Ali Yılmaz",
                BorcMiktari = 0
            };

            // Assert
            ogrenci.Id.Should().Be(1);
            ogrenci.OgrenciNo.Should().Be("20230001");
            ogrenci.AdSoyad.Should().Be("Ali Yılmaz");
            ogrenci.BorcMiktari.Should().Be(0);
        }

        [Fact]
        public void Ogrenci_BorcMiktari_Default_Should_Be_Zero()
        {
            // Arrange & Act
            var ogrenci = new Ogrenci();

            // Assert
            ogrenci.BorcMiktari.Should().Be(0);
        }
    }
}