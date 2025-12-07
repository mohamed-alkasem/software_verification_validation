using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KutuphaneProject.Controllers;
using KutuphaneProject.Models;

namespace MyApp.UnitTests.ControllersTests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_Should_Return_ViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Privacy_Should_Return_ViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Error_Should_Return_View_With_ErrorViewModel()
        {
            // Arrange
            // Simulate HttpContext for the controller
            _controller.ControllerContext = new ControllerContext();

            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<ErrorViewModel>();
        }
    }
}