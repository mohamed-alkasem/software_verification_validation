using Xunit;
using FluentAssertions;
using KutuphaneProject.Models;

namespace MyApp.UnitTests.ModelsTests
{
    public class KitapModelTests
    {
        [Fact]
        public void Kitap_Properties_Should_Be_Set_Correctly()
        {
            // Arrange
            var kitap = new Kitap
            {
                Id = 1,
                Ad = "C# Programlama",
                Aciklama = "C# öğrenmek için harika bir kitap",
                EklemeTarihi = new DateTime(2023, 1, 1),
                KategoriId = 1,
                Image = new byte[] { 0x20, 0x20, 0x20 }
            };

            // Assert
            kitap.Id.Should().Be(1);
            kitap.Ad.Should().Be("C# Programlama");
            kitap.Aciklama.Should().Be("C# öğrenmek için harika bir kitap");
            kitap.EklemeTarihi.Should().Be(new DateTime(2023, 1, 1));
            kitap.KategoriId.Should().Be(1);
            kitap.Image.Should().NotBeNull();
        }

        [Fact]
        public void Kitap_ImageSrc_Should_Return_Base64_String_When_Image_Exists()
        {
            // Arrange
            var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // PNG başlığı
            var kitap = new Kitap { Image = imageBytes };

            // Act
            var imageSrc = kitap.ImageSrc;

            // Assert
            imageSrc.Should().StartWith("data:image/jpg;base64,");
            imageSrc.Should().NotBeEmpty();
        }

        [Fact]
        public void Kitap_ImageSrc_Should_Return_Empty_String_When_Image_Is_Null()
        {
            // Arrange
            var kitap = new Kitap { Image = null };

            // Act
            var imageSrc = kitap.ImageSrc;

            // Assert
            imageSrc.Should().BeEmpty();
        }
    }
}