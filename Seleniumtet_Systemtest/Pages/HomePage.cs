using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // Kurulumdan sonra bu satır çalışacak

namespace SeleniumSystemTests.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        // Ana sayfa
        private readonly string _url = $"{Fixtures.WebDriverFixture.BaseUrl}/";

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // --- İlk test fonksiyonları (HomeTests) ---
        public void Navigate()
        {
            _driver.Navigate().GoToUrl(_url);
        }

        public string GetPageTitle()
        {
            return _driver.Title;
        }

        // --- Giriş testi fonksiyonları (OgrenciLoginTests) ---
        public bool IsLogoutButtonVisible()
        {
            try
            {
                // Çıkış butonunu bekliyoruz (btn-cikis)
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