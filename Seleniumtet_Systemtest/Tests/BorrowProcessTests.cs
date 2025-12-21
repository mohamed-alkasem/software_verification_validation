using SeleniumSystemTests.Fixtures;
using SeleniumSystemTests.Pages;
using Xunit;

namespace SeleniumSystemTests.Tests
{
    public class BorrowProcessTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;
        private readonly LoginPage _loginPage;
        private readonly BookDetailsPage _bookPage;

        public BorrowProcessTests(WebDriverFixture fixture)
        {
            _fixture = fixture;
            _loginPage = new LoginPage(_fixture.Driver);
            _bookPage = new BookDetailsPage(_fixture.Driver);
        }

        [Fact]
        public void FullScenario_Login_And_BorrowBook_ShouldShowSweetAlert()
        {
            // 1. Arrange (Giriş ve kitap bilgileri)
            string user = "2212721320";
            string pass = "Alkasem.00";
            int kitapId = 2; // Veritabanında bu ID'ye sahip bir kitap olduğundan emin olun

            // 2. Act (Adımları uygula)
            // a) Giriş yap
            _loginPage.Navigate();
            _loginPage.Login(user, pass);

            // b) Kitaba git
            _bookPage.GoToBook(kitapId);

            // c) Ödünç alma butonuna tıkla
            _bookPage.ClickBorrow();

            // 3. Assert (Kontrol)
            // SweetAlert penceresinin göründüğünü kontrol et
            Assert.True(_bookPage.IsSuccessPopupDisplayed(), "Başarısız! Uyarı penceresi (SweetAlert) görünmedi.");

            // Mesaj başlığını kontrol et ('Başarılı!' olmalı)
            string alertTitle = _bookPage.GetPopupTitle();
            Assert.Contains("Başarılı", alertTitle);
        }
    }
}