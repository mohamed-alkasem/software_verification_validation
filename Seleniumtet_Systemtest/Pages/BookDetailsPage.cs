using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumSystemTests.Pages
{
    public class BookDetailsPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public BookDetailsPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void GoToBook(int kitapId)
        {
            // نذهب لصفحة الكتاب مباشرة
            // تأكد أن الرابط يطابق الرابط في متصفحك
            string url = $"{Fixtures.WebDriverFixture.BaseUrl}/Ogrenci/KitapOgrenci?kitapId={kitapId}";
            _driver.Navigate().GoToUrl(url);
        }

        // --- العناصر ---

        // 1. زر الاستعارة
        // بما أن الزر داخل Helper، سنبحث عن أي رابط (a) يحتوي الـ href تبعه على كلمة "OduncAl"
        private IWebElement BorrowButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href*='OduncAl']")));

        // 2. نافذة SweetAlert (رسالة النجاح)
        // SweetAlert دائماً تملك كلاس اسمه 'swal2-popup'
        private IWebElement SweetAlertPopup => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));

        // العنوان داخل الرسالة (كلمة "Başarılı!")
        private IWebElement SweetAlertTitle => _driver.FindElement(By.ClassName("swal2-title"));

        // ----------------

        public void ClickBorrow()
        {
            try
            {
                BorrowButton.Click();
            }
            catch (Exception)
            {
                // في حال كان الزر مغطى أو غير قابل للنقر العادي
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].click();", BorrowButton);
            }
        }

        public bool IsSuccessPopupDisplayed()
        {
            try
            {
                return SweetAlertPopup.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetPopupTitle()
        {
            return SweetAlertTitle.Text;
        }
    }
}