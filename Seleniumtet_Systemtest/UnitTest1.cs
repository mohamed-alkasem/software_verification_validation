//using Xunit;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;

//public class MyFirstSeleniumTest : IDisposable
//{
//    private IWebDriver _driver;

//    public MyFirstSeleniumTest()
//    {
//        // المتصفح بيفتح مع بداية الكلاس
//        _driver = new ChromeDriver();
//        Console.WriteLine("🟢 المتصفح فتح!");
//    }

//    [Fact]
//    public void Test_OpenWebsite_CheckTitle()
//    {
//        // تأكد إن التطبيق شغال أولاً
//        WaitForAppToStart();

//        // الذهاب للرابط
//        string myAppUrl = "https://localhost:7123/";
//        _driver.Navigate().GoToUrl(myAppUrl);

//        // انتظر أكتر
//        System.Threading.Thread.Sleep(8000); // 8 ثواني

//        // اطبع معلومات أكتر
//        Console.WriteLine($"🔍 عنوان التبويب: {_driver.Title}");
//        Console.WriteLine($"🔍 الصفحة فيها: {_driver.PageSource.Length} حرف");
//        Console.WriteLine($"🔍 الرابط: {_driver.Url}");

//        // إذا الصفحة فاضية، في مشكلة
//        if (string.IsNullOrWhiteSpace(_driver.Title))
//        {
//            // خذ صورة للشاشة
//            TakeScreenshot("empty_page");
//            throw new Exception("الصفحة فاضية! الموقع مش شغال");
//        }

//        // التحقق
//        Assert.Contains("İSUBU", _driver.Title);

//        Console.WriteLine("✅✅✅ الاختبار نجح!");
//    }

//    [Fact]
//    public void Test_Just_Keep_Browser_Open()
//    {
//        // هذا التيست بس عشان يخلي المتصفح مفتوح
//        Console.WriteLine("👀 المتصفح مفتوح... شوف إذا الموقع ظاهر");
//        Console.WriteLine("اضغط Enter عشان تكمل...");
//        Console.ReadLine();

//        _driver.Navigate().GoToUrl("https://localhost:7123/");
//        Thread.Sleep(3000);

//        Console.WriteLine($"العنوان: {_driver.Title}");
//        Console.WriteLine("اضغط Enter تاني عشان تغلق...");
//        Console.ReadLine();
//    }

//    private void WaitForAppToStart()
//    {
//        Console.WriteLine("⏳ عم بتحقق إذا الموقع شغال...");

//        using var client = new HttpClient();
//        client.Timeout = TimeSpan.FromSeconds(5);

//        try
//        {
//            var response = client.GetAsync("https://localhost:7123/").Result;
//            if (response.IsSuccessStatusCode)
//            {
//                Console.WriteLine("✅ الموقع شغال!");
//            }
//            else
//            {
//                Console.WriteLine($"⚠️ الموقع رد بـ: {response.StatusCode}");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"❌ ما في اتصال: {ex.Message}");
//            Console.WriteLine("🚀 لازم تشغل الموقع بـ F5 أولاً!");
//            throw;
//        }
//    }

//    private void TakeScreenshot(string name)
//    {
//        try
//        {
//            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
//            var fileName = $"{name}_{DateTime.Now:HHmmss}.png";
//            screenshot.SaveAsFile(fileName);
//            Console.WriteLine($"📸 أخذت صورة: {fileName}");
//        }
//        catch
//        {
//            // تجاهل إذا ما اتعملت صورة
//        }
//    }

//    public void Dispose()
//    {
//        // هون بس تغلق المتصفح
//        Console.WriteLine("🛑 عم بقفل المتصفح...");
//        Thread.Sleep(2000);
//        _driver?.Quit();
//        Console.WriteLine("المتصفح انقفل");
//    }
//}