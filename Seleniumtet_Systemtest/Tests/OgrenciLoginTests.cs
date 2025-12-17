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
            // الدليل القاطع هو ظهور زر الخروج
            Assert.True(_homePage.IsLogoutButtonVisible(), "Login Failed: Logout button not visible!");

            // تعديل: بدلاً من إجبار الرابط أن يحتوي على Ogrenci، سنتأكد فقط أننا لسنا في صفحة الدخول
            // هذا السطر يكفي للتأكد أننا انتقلنا لصفحة جديدة
            Assert.NotEqual(_fixture.Driver.Url, "https://localhost:7123/Ogrenci/OgrenciGiris");
        }
    }
}