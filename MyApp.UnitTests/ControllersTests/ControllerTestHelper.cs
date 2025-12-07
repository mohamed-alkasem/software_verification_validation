using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace MyApp.UnitTests.ControllersTests
{
    public static class ControllerTestHelper
    {
        // إنشاء HttpContext مع مستخدم مصادق
        public static HttpContext CreateAuthenticatedHttpContext(string userName, string role = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            };

            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            return httpContext;
        }

        // إعداد ControllerContext للمتحكم
        public static TController SetupControllerContext<TController>(TController controller, HttpContext httpContext = null)
            where TController : Controller
        {
            if (httpContext == null)
            {
                httpContext = new DefaultHttpContext();
            }

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }

        // إعداد TempData للمتحكم
        public static TController SetupTempData<TController>(TController controller)
            where TController : Controller
        {
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(
                new DefaultHttpContext(),
                tempDataProvider.Object);

            controller.TempData = tempDataDictionary;

            return controller;
        }

        // إنشاء ModelState غير صالح
        public static void AddModelError(Controller controller, string key, string errorMessage)
        {
            controller.ModelState.AddModelError(key, errorMessage);
        }

        // التحقق من أن النتيجة هي RedirectToActionResult
        public static void ShouldBeRedirectToActionResult(IActionResult result, string actionName, string controllerName = null)
        {
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;

            redirectResult.ActionName.Should().Be(actionName);

            if (!string.IsNullOrEmpty(controllerName))
            {
                redirectResult.ControllerName.Should().Be(controllerName);
            }
        }

        // التحقق من أن النتيجة هي ViewResult
        public static ViewResult ShouldBeViewResult(IActionResult result)
        {
            result.Should().BeOfType<ViewResult>();
            return result as ViewResult;
        }

        // التحقق من أن Model في ViewResult من نوع معين
        public static T ShouldHaveModelOfType<T>(ViewResult viewResult)
        {
            viewResult.Model.Should().BeOfType<T>();
            return (T)viewResult.Model;
        }
    }
}