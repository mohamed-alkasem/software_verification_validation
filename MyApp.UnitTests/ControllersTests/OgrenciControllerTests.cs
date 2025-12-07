using System.Security.Claims;
using FluentAssertions;
using KutuphaneProject.Controllers;
using KutuphaneProject.Models;
using KutuphaneProject.Models.UserModels;
using KutuphaneProject.Services.Interfaces;
using KutuphaneProject.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MyApp.UnitTests.ControllersTests
{
    public class OgrenciControllerTests
    {
        // Mocks for all dependencies
        private readonly Mock<IOduncService> _mockOduncService;
        private readonly Mock<IKitapService> _mockKitapService;
        private readonly Mock<IMesajService> _mockMesajService;
        private readonly Mock<IOgrenciService> _mockOgrenciService;
        private readonly Mock<IKategoriService> _mockKategoriService;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
        private readonly OgrenciController _controller;

        public OgrenciControllerTests()
        {
            // Initialize all mocks
            _mockOduncService = new Mock<IOduncService>();
            _mockKitapService = new Mock<IKitapService>();
            _mockMesajService = new Mock<IMesajService>();
            _mockOgrenciService = new Mock<IOgrenciService>();
            _mockKategoriService = new Mock<IKategoriService>();

            // Mock UserManager
            var userStore = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            // Mock SignInManager
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _mockSignInManager = new Mock<SignInManager<AppUser>>(
                _mockUserManager.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                null, null, null, null);

            // Create controller with mocked dependencies
            _controller = new OgrenciController(
                _mockSignInManager.Object,
                _mockUserManager.Object,
                _mockOgrenciService.Object,
                _mockMesajService.Object,
                _mockKategoriService.Object,
                _mockKitapService.Object,
                _mockOduncService.Object,
                null // GecikmeKontrolService - optional
            );
        }

        [Fact]
        public async Task Index_Should_Return_View_With_Kitaplar()
        {
            // Arrange
            var kitaplar = new List<Kitap>
            {
                new Kitap { Id = 1, Ad = "Kitap 1" },
                new Kitap { Id = 2, Ad = "Kitap 2" }
            };

            _mockKitapService.Setup(s => s.GetKitaplar())
                            .ReturnsAsync(kitaplar);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<List<Kitap>>();
            var model = result.Model as List<Kitap>;
            model.Should().HaveCount(2);
        }

        [Fact]
        public void OgrenciGiris_Get_Should_Return_View()
        {
            // Act
            var result = _controller.OgrenciGiris() as ViewResult;

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task OgrenciGiris_Post_ValidModel_Should_Redirect_To_Index()
        {
            // Arrange
            var loginModel = new LoginUser
            {
                OgrenciNo = "20230001",
                OgrenciSifre = "Password123"
            };

            var user = new AppUser { OgrenciNo = "20230001", UserName = "20230001" };

            _mockUserManager.Setup(m => m.Users)
                           .Returns(new List<AppUser> { user }.AsQueryable());

            _mockSignInManager.Setup(s => s.PasswordSignInAsync(
                loginModel.OgrenciNo!, loginModel.OgrenciSifre!, true, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await _controller.OgrenciGiris(loginModel) as RedirectToActionResult;

            // Assert
            result.Should().NotBeNull();
            result.ActionName.Should().Be("Index");
        }

        [Fact]
        public void OgrenciUye_Get_Should_Return_View()
        {
            // Act
            var result = _controller.OgrenciUye() as ViewResult;

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task OgrenciUye_Post_ValidModel_Should_Add_Ogrenci_And_Redirect()
        {
            // Arrange
            var registerModel = new RegisterUser
            {
                OgrenciNo = "20230003",
                OgrenciAdSoyad = "Yeni Öğrenci",
                OgrenciSifre = "Password123"
            };

            var user = new AppUser
            {
                UserName = registerModel.OgrenciNo,
                OgrenciNo = registerModel.OgrenciNo,
                OgrenciAdSoyad = registerModel.OgrenciAdSoyad
            };

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

            _mockOgrenciService.Setup(s => s.OgrenciEkle(It.IsAny<Ogrenci>()))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.OgrenciUye(registerModel) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            _mockOgrenciService.Verify(s => s.OgrenciEkle(It.IsAny<Ogrenci>()), Times.Once);
        }

        [Fact]
        public async Task Kategori_ValidId_Should_Return_View_With_Kitaplar()
        {
            // Arrange
            var kategoriId = 1;
            var kategori = new Kategori { Id = kategoriId, Ad = "Programlama" };
            var kitaplar = new List<Kitap>
            {
                new Kitap { Id = 1, Ad = "C# Kitap", KategoriId = kategoriId }
            };

            _mockKategoriService.Setup(s => s.GetKategoriById(kategoriId))
                               .ReturnsAsync(kategori);

            _mockKitapService.Setup(s => s.GetKitaplarByKategoriId(kategoriId))
                            .ReturnsAsync(kitaplar);

            // Act
            var result = await _controller.Kategori(kategoriId) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<List<Kitap>>();
            result.ViewData["KategoriAdi"].Should().Be("Programlama");
        }

        [Fact]
        public async Task KitapOgrenci_ValidId_Should_Return_View_With_Kitap()
        {
            // Arrange
            var kitapId = 1;
            var kitap = new Kitap { Id = kitapId, Ad = "Test Kitap" };

            _mockKitapService.Setup(s => s.GetKitapById(kitapId))
                            .ReturnsAsync(kitap);

            // Act
            var result = await _controller.KitapOgrenci(kitapId) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<Kitap>();
            (result.Model as Kitap).Id.Should().Be(kitapId);
        }

        [Fact]
        public async Task OduncAl_ValidKitapId_With_Authenticated_User_Should_Create_Odunc()
        {
            // Arrange
            var kitapId = 1;
            var kitap = new Kitap { Id = kitapId, Ad = "Test Kitap" };
            var ogrenciNo = "20230001";
            var ogrenci = new Ogrenci { Id = 1, OgrenciNo = ogrenciNo };

            // Setup authenticated user
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, ogrenciNo) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _mockKitapService.Setup(s => s.GetKitapById(kitapId))
                            .ReturnsAsync(kitap);

            _mockOgrenciService.Setup(s => s.GetOgrenciByOgrenciNo(ogrenciNo))
                              .ReturnsAsync(ogrenci);

            _mockOduncService.Setup(s => s.OduncEkle(It.IsAny<Odunc>()))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.OduncAl(kitapId) as RedirectToActionResult;

            // Assert
            result.Should().NotBeNull();
            result.ActionName.Should().Be("KitapOgrenci");
            _mockOduncService.Verify(s => s.OduncEkle(It.IsAny<Odunc>()), Times.Once);
        }

        [Fact]
        public async Task Kategoriler_Should_Return_View_With_Kategoriler()
        {
            // Arrange
            var kategoriler = new List<Kategori>
            {
                new Kategori { Id = 1, Ad = "Programlama" },
                new Kategori { Id = 2, Ad = "Roman" }
            };

            _mockKategoriService.Setup(s => s.GetKategoriler())
                               .ReturnsAsync(kategoriler);

            // Act
            var result = await _controller.Kategoriler() as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<List<Kategori>>();
            var model = result.Model as List<Kategori>;
            model.Should().HaveCount(2);
        }

        [Fact]
        public void Hakkinda_Should_Return_View()
        {
            // Act
            var result = _controller.Hakkinda() as ViewResult;

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Iletisim_Get_Should_Return_View()
        {
            // Act
            var result = _controller.Iletisim() as ViewResult;

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CikisYap_Should_Call_SignOut_And_Redirect()
        {
            // Arrange
            _mockSignInManager.Setup(s => s.SignOutAsync())
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CikisYap() as RedirectToActionResult;

            // Assert
            result.Should().NotBeNull();
            result.ControllerName.Should().Be("Ogrenci");
            result.ActionName.Should().Be("Index");
            _mockSignInManager.Verify(s => s.SignOutAsync(), Times.Once);
        }
    }
}