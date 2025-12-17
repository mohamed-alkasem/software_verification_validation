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
            // 1. Arrange (بيانات الدخول والكتاب)
            string user = "2212721320";
            string pass = "Alkasem.00";
            int kitapId = 2; // تأكد أن لديك كتاب في الداتابيز بهذا الآيدي

            // 2. Act (تنفيذ الخطوات)
            // أ) تسجيل الدخول
            _loginPage.Navigate();
            _loginPage.Login(user, pass);

            // ب) الذهاب للكتاب
            _bookPage.GoToBook(kitapId);

            // ج) الضغط على زر الاستعارة
            _bookPage.ClickBorrow();

            // 3. Assert (التحقق)
            // التحقق من ظهور نافذة SweetAlert
            Assert.True(_bookPage.IsSuccessPopupDisplayed(), "فشل! لم تظهر نافذة التنبيه (SweetAlert).");

            // التحقق من عنوان الرسالة (يجب أن يكون 'Başarılı!')
            string alertTitle = _bookPage.GetPopupTitle();
            Assert.Contains("Başarılı", alertTitle);
        }
    }
}