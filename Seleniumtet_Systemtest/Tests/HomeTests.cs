using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSystemTests.Fixtures;
using SeleniumSystemTests.Pages;
using System;
using System.Diagnostics;

namespace SeleniumSystemTests.Tests
{
    public class HomeTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;
        private readonly HomePage _homePage;

        public HomeTests(WebDriverFixture fixture)
        {
            _fixture = fixture;
            _homePage = new HomePage(_fixture.Driver);
        }

        // 1. Usability Test (Title Check)
        [Fact]
        public void HomePage_Title_ShouldContain_ISUBU()
        {
            // Arrange & Act
            _homePage.Navigate();

            // Assert
            string title = _homePage.GetPageTitle();
            Assert.Contains("İSUBU", title);
        }

        // 2. Usability Test (Logo Check)
        [Fact]
        public void Usability_Logo_ShouldBeVisible()
        {
            // Arrange
            _homePage.Navigate();

            // Act
            bool isLogoDisplayed = _fixture.Driver.FindElement(By.CssSelector(".navbar-brand img")).Displayed;

            // Assert
            // الترجمة هنا فقط (رسالة الخطأ)
            Assert.True(isLogoDisplayed, "Ana sayfada logo görüntülenmiyor! (Logo is not visible)");
        }

        // 3. Performance Test (Load Time)
        [Fact]
        public void Performance_HomePage_LoadTime_ShouldBeFast()
        {
            // Start stopwatch
            var stopwatch = Stopwatch.StartNew();

            // Act
            _homePage.Navigate();

            var wait = new WebDriverWait(_fixture.Driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".navbar-brand")));

            // Stop stopwatch
            stopwatch.Stop();
            long loadTime = stopwatch.ElapsedMilliseconds;

            // Assert
            // الترجمة هنا لرسالة الخطأ فقط
            Assert.True(loadTime < 2000, $"Sayfa çok yavaş! Yüklenme süresi: {loadTime}ms. Beklenen: < 2000ms");
        }
    }
}