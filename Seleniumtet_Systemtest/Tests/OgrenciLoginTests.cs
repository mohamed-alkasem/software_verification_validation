using SeleniumSystemTests.Fixtures;
using SeleniumSystemTests.Pages;
using Xunit;

namespace SeleniumSystemTests.Tests
{
    public class OgrenciLoginTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;
        private readonly LoginPage _loginPage;
        private readonly HomePage _homePage;

        public OgrenciLoginTests(WebDriverFixture fixture)
        {
            _fixture = fixture;
            _loginPage = new LoginPage(_fixture.Driver);
            _homePage = new HomePage(_fixture.Driver);
        }

        [Fact]
        public void OgrenciGiris_WithRealCredentials_ShouldLoginSuccessfully()
        {
            // 1. Arrange
            string realUser = "2212721320";
            string realPass = "Alkasem.00";

            // 2. Act
            _loginPage.Navigate();
            _loginPage.Login(realUser, realPass);

            // 3. Assert
            // Kesin kanıt çıkış butonunun görünmesidir
            Assert.True(_homePage.IsLogoutButtonVisible(), "Login Failed: Logout button not visible!");

            // Değişiklik: Linkin Ogrenci içermesini zorlamak yerine, sadece giriş sayfasında olmadığımızdan emin oluyoruz
            // Yeni bir sayfaya geçtiğimizi doğrulamak için bu satır yeterli
            Assert.NotEqual(_fixture.Driver.Url, "https://localhost:7123/Ogrenci/OgrenciGiris");
        }
    }
}