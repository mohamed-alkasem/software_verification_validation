using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

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
            // Doğrudan kitap sayfasına git
            // Linkin tarayıcınızdaki linkle eşleştiğinden emin olun
            string url = $"{Fixtures.WebDriverFixture.BaseUrl}/Ogrenci/KitapOgrenci?kitapId={kitapId}";
            _driver.Navigate().GoToUrl(url);
        }

        // --- Elementler ---

        // 1. Ödünç alma butonu
        // Form içinde submit butonu olduğu için, form içindeki submit butonunu buluyoruz
        private IWebElement BorrowButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("form[action*='OduncAl'] button[type='submit']")));

        // 2. SweetAlert penceresi (başarı mesajı)
        // SweetAlert her zaman 'swal2-popup' sınıfına sahiptir
        private IWebElement SweetAlertPopup => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));

        // Mesaj içindeki başlık ("Başarılı!" kelimesi)
        private IWebElement SweetAlertTitle => _driver.FindElement(By.ClassName("swal2-title"));

        // ----------------

        public void ClickBorrow()
        {
            try
            {
                // Form submit butonunu bul ve tıkla
                var form = _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("form[action*='OduncAl']")));
                var submitButton = form.FindElement(By.CssSelector("button[type='submit']"));
                
                // Tarih alanının dolu olduğundan emin ol
                var dateInput = form.FindElement(By.Id("geriDonusTarihi"));
                if (string.IsNullOrEmpty(dateInput.GetAttribute("value")))
                {
                    // Varsayılan tarihi ayarla
                    IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                    js.ExecuteScript("arguments[0].value = arguments[1];", dateInput, DateTime.Today.AddDays(10).ToString("yyyy-MM-dd"));
                }
                
                submitButton.Click();
                
                // Form gönderiminden sonra sayfanın yüklenmesini bekle
                _wait.Until(ExpectedConditions.UrlContains("KitapOgrenci"));
            }
            catch (Exception)
            {
                // Eğer buton gizli veya normal tıklamaya uygun değilse
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                var form = _driver.FindElement(By.CssSelector("form[action*='OduncAl']"));
                var submitButton = form.FindElement(By.CssSelector("button[type='submit']"));
                js.ExecuteScript("arguments[0].click();", submitButton);
                
                // Form gönderiminden sonra sayfanın yüklenmesini bekle
                _wait.Until(ExpectedConditions.UrlContains("KitapOgrenci"));
            }
        }

        public bool IsSuccessPopupDisplayed()
        {
            try
            {
                // SweetAlert'in görünmesi için biraz daha uzun bekle
                var extendedWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
                extendedWait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
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