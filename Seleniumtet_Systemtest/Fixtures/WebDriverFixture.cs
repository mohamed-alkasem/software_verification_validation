using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumSystemTests.Fixtures
{
    public class WebDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; private set; }
        // تأكد أن هذا الرابط هو نفس الرابط اللي بيشتغل عليه مشروعك
        public const string BaseUrl = "https://localhost:7123";

        public WebDriverFixture()
        {
            var options = new ChromeOptions();
            // options.AddArgument("--headless"); // لو بدك يشتغل بالخلفية

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Dispose()
        {
            // ضروري جداً تشيل الكومنت عن هدول السطرين
            Driver.Quit();
            Driver.Dispose();
        }
    }
}