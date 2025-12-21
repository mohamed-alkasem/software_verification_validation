using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSystemTests.Fixtures;
using SeleniumSystemTests.Pages;
using System;
using System.IO;
using System.Threading;

namespace SeleniumSystemTests.Tests
{
    public class AdminTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;
        private readonly LoginPage _loginPage;
        private readonly WebDriverWait _wait;

        public AdminTests(WebDriverFixture fixture)
        {
            _fixture = fixture;
            _loginPage = new LoginPage(_fixture.Driver);
            _wait = new WebDriverWait(_fixture.Driver, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void Admin_AddCategory_Test()
        {
            // 1. Giriş yapıldığından emin ol
            EnsureLogin();

            // 2. Kategori ekleme sayfasına git
            _fixture.Driver.Navigate().GoToUrl($"{WebDriverFixture.BaseUrl}/Adminler/Admin/KategoriEkle");

            // 3. Verileri doldur ve kategoriyi kaydet
            CreateCategory("xUnit Final Kategori");
        }

        // --- Helper Methods ---

        private void EnsureLogin()
        {
            if (!_fixture.Driver.Url.Contains("Adminler"))
            {
                _loginPage.LoginAsAdmin("12345678901", "Admin123!");
                try
                {
                    _wait.Until(ExpectedConditions.UrlContains("Adminler"));
                }
                catch (WebDriverTimeoutException)
                {
                    throw new Exception($"Giriş başarısız! Link: {_fixture.Driver.Url}");
                }
            }
        }

        private void CreateCategory(string categoryName)
        {
            // Resim yükleme
            string imagePath = CreateDummyImage();
            var fileInput = _wait.Until(ExpectedConditions.ElementExists(By.Id("file-input1")));
            fileInput.SendKeys(imagePath);

            // İsim
            _fixture.Driver.FindElement(By.CssSelector(".ad-input")).SendKeys(categoryName);

            // Kaydet
            var saveBtn = _fixture.Driver.FindElement(By.CssSelector(".btn-kategori-ekle"));
            ClickWithJs(saveBtn);

            // Sayfanın yüklenmesini bekle
            _wait.Until(ExpectedConditions.UrlContains("KategoriEkle"));

            // Başarı mesajını kontrol et
            VerifySuccessMessage();
        }

        private void ClickWithJs(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_fixture.Driver;
            js.ExecuteScript("arguments[0].click();", element);
        }

        private string CreateDummyImage()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(projectPath, "test.jpg");
            if (!File.Exists(imagePath)) File.WriteAllText(imagePath, "dummy content");
            return imagePath;
        }

        private void VerifySuccessMessage()
        {
            try
            {
                // SweetAlert'in görünmesini bekle (daha uzun süre)
                var swalTitle = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("swal2-title")));
                Assert.Equal("Başarılı!", swalTitle.Text);

                // Test temizliği için uyarıyı kapat
                var confirmBtn = _fixture.Driver.FindElement(By.CssSelector(".swal2-confirm"));
                if (confirmBtn.Displayed) ClickWithJs(confirmBtn);
            }
            catch (WebDriverTimeoutException)
            {
                Assert.True(false, $"Başarı mesajı görünmedi. Link: {_fixture.Driver.Url}");
            }
        }
        [Fact]
        public void Security_StudentCannotAccessAdminPage()
        {
            // 1. "Öğrenci" olarak giriş yap (admin değil)
            // Veritabanında mevcut bir öğrenci numarası kullan
            _loginPage.Login("2212721320", "Alkasem.00");

            // 2. "Gizlice" kategori ekleme sayfasına (admin linki) girmeyi dene
            string adminUrl = $"{WebDriverFixture.BaseUrl}/Adminler/Admin/KategoriEkle";
            _fixture.Driver.Navigate().GoToUrl(adminUrl);

            // 3. Kontrol (Güvenlik Kontrolü)
            // Sistem onu engellemeli, yani mevcut link *admin linki olmamalı*
            // Genellikle Login veya AccessDenied sayfasına yönlendirir

            string currentUrl = _fixture.Driver.Url;

            // Koşul: Mevcut link, girmeye çalıştığımız admin linki ile eşit olmamalı
            Assert.NotEqual(adminUrl, currentUrl);
        }
    }

}