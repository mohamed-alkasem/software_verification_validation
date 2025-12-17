using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSystemTests.Fixtures;
using SeleniumSystemTests.Pages;
using System;
using System.IO;
using System.Threading;

namespace SeleniumSystemTests.Tests
{
    public class AdminTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;
        private readonly LoginPage _loginPage;
        private readonly WebDriverWait _wait;

        public AdminTests(WebDriverFixture fixture)
        {
            _fixture = fixture;
            _loginPage = new LoginPage(_fixture.Driver);
            _wait = new WebDriverWait(_fixture.Driver, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void Admin_AddCategory_Test()
        {
            // 1. ضمان تسجيل الدخول
            EnsureLogin();

            // 2. الانتقال لصفحة إضافة التصنيف
            _fixture.Driver.Navigate().GoToUrl($"{WebDriverFixture.BaseUrl}/Adminler/Admin/KategoriEkle");

            // 3. تعبئة البيانات وحفظ التصنيف
            CreateCategory("تصنيف xUnit نهائي");
        }

        // --- Helper Methods ---

        private void EnsureLogin()
        {
            if (!_fixture.Driver.Url.Contains("Adminler"))
            {
                _loginPage.LoginAsAdmin("12345678901", "Admin123!");
                try
                {
                    _wait.Until(ExpectedConditions.UrlContains("Adminler"));
                }
                catch (WebDriverTimeoutException)
                {
                    throw new Exception($"فشل الدخول! الرابط: {_fixture.Driver.Url}");
                }
            }
        }

        private void CreateCategory(string categoryName)
        {
            // رفع الصورة
            string imagePath = CreateDummyImage();
            var fileInput = _wait.Until(ExpectedConditions.ElementExists(By.Id("file-input1")));
            fileInput.SendKeys(imagePath);

            // الاسم
            _fixture.Driver.FindElement(By.CssSelector(".ad-input")).SendKeys(categoryName);

            // الحفظ
            var saveBtn = _fixture.Driver.FindElement(By.CssSelector(".btn-kategori-ekle"));
            ClickWithJs(saveBtn);

            // التحقق من النجاح
            VerifySuccessMessage();
        }

        private void ClickWithJs(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_fixture.Driver;
            js.ExecuteScript("arguments[0].click();", element);
        }

        private string CreateDummyImage()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(projectPath, "test.jpg");
            if (!File.Exists(imagePath)) File.WriteAllText(imagePath, "dummy content");
            return imagePath;
        }

        private void VerifySuccessMessage()
        {
            try
            {
                var swalTitle = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("swal2-title")));
                Assert.Equal("Başarılı!", swalTitle.Text);

                // إغلاق التنبيه لضمان نظافة التيست
                var confirmBtn = _fixture.Driver.FindElement(By.CssSelector(".swal2-confirm"));
                if (confirmBtn.Displayed) ClickWithJs(confirmBtn);
            }
            catch (WebDriverTimeoutException)
            {
                Assert.True(false, $"لم تظهر رسالة النجاح. الرابط: {_fixture.Driver.Url}");
            }
        }
        [Fact]
        public void Security_StudentCannotAccessAdminPage()
        {
            // 1. تسجيل الدخول كـ "طالب" (وليس أدمن)
            // استخدم رقم طالب موجود عندك بالداتا بيز
            _loginPage.Login("2212721320", "Alkasem.00");

            // 2. محاولة الدخول "خلسة" إلى صفحة إضافة التصنيف (رابط الأدمن)
            string adminUrl = $"{WebDriverFixture.BaseUrl}/Adminler/Admin/KategoriEkle";
            _fixture.Driver.Navigate().GoToUrl(adminUrl);

            // 3. التحقق (Security Check)
            // النظام لازم يمنعه، يعني الرابط الحالي *يجب ألا يكون* رابط الأدمن
            // غالباً رح يرجعه لصفحة Login أو AccessDenied

            string currentUrl = _fixture.Driver.Url;

            // الشرط: الرابط الحالي لا يساوي رابط الأدمن الذي حاولنا دخوله
            Assert.NotEqual(adminUrl, currentUrl);
        }
    }

}