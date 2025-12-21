using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumSystemTests.Fixtures
{
    public class WebDriverFixture : IDisposable
    {
        public IWebDriver Driver { get; private set; }
        // Bu linkin projenizin çalıştığı linkle aynı olduğundan emin olun
        public const string BaseUrl = "https://localhost:7123";

        public WebDriverFixture()
        {
            var options = new ChromeOptions();
            // options.AddArgument("--headless"); // Arka planda çalıştırmak isterseniz

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void Dispose()
        {
            // Bu iki satırın yorumunu kaldırmak çok önemlidir
            Driver.Quit();
            Driver.Dispose();
        }
    }
}