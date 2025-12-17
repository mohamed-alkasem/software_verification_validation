using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSystemTests.Fixtures;
using System;

namespace SeleniumSystemTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // --- 1. دوال التوجيه (لحل مشكلة CS1061) ---

        // هذه الدالة تطلبها ملفات BorrowProcessTests
        public void Navigate()
        {
            _driver.Navigate().GoToUrl($"{WebDriverFixture.BaseUrl}/Ogrenci/OgrenciGiris");
        }

        // هذه الدالة تطلبها ملفات AdminTests
        public void GoToLogin()
        {
            _driver.Navigate().GoToUrl($"{WebDriverFixture.BaseUrl}/Ogrenci/AdminGiris");
        }

        // --- 2. دوال تسجيل الدخول ---

        // للطلاب (تحافظ على عمل الكود القديم)
        public void Login(string studentNo, string password)
        {
            if (!_driver.Url.Contains("OgrenciGiris")) Navigate();

            var studentInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ogrenciNo-giris")));
            var passInput = _driver.FindElement(By.Id("password-giris"));
            var btn = _driver.FindElement(By.CssSelector(".btn-ogrenci-giris"));

            studentInput.Clear();
            studentInput.SendKeys(studentNo);

            passInput.Clear();
            passInput.SendKeys(password);

            ClickWithRetry(btn);
        }

        // للأدمن (للكود الجديد)
        public void LoginAsAdmin(string tcNo, string password)
        {
            if (!_driver.Url.Contains("AdminGiris")) GoToLogin();

            var tcInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tcNo-giris")));
            var passInput = _driver.FindElement(By.Id("password-giris"));
            var btn = _driver.FindElement(By.CssSelector(".btn-admin-giris"));

            tcInput.Clear();
            tcInput.SendKeys(tcNo);

            passInput.Clear();
            passInput.SendKeys(password);

            ClickWithRetry(btn);
        }

        // دالة مساعدة للنقر
        private void ClickWithRetry(IWebElement element)
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(element)).Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
            }
            catch (StaleElementReferenceException)
            {
                // محاولة أخيرة لو العنصر اختفى ورجع
                _wait.Until(ExpectedConditions.ElementToBeClickable(element)).Click();
            }
        }
    }
}