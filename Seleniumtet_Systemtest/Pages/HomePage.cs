using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // الآن سيعمل هذا السطر بعد التثبيت

namespace SeleniumSystemTests.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        // الصفحة الرئيسية
        private readonly string _url = $"{Fixtures.WebDriverFixture.BaseUrl}/";

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // --- دوال الاختبار الأول (HomeTests) ---
        public void Navigate()
        {
            _driver.Navigate().GoToUrl(_url);
        }

        public string GetPageTitle()
        {
            return _driver.Title;
        }

        // --- دوال اختبار تسجيل الدخول (OgrenciLoginTests) ---
        public bool IsLogoutButtonVisible()
        {
            try
            {
                // ننتظر زر الخروج (btn-cikis)
                var logoutBtn = _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("btn-cikis")));
                return logoutBtn.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}