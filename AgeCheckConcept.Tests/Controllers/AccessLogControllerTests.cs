using AgeCheckConcept.Controllers;
using AgeCheckConcept.Models;
using AgeCheckConcept.Service;
using AgeCheckConcept.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AgeCheckConcept.Tests.Controllers
{
    public class AccessLogControllerTests : AccessLogTestBase
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfAccessLogs()
        {
            // Arrange
            var logs = await _context.AccessLogs.ToListAsync();
            // use the sample data from the dummy _context InMemoryDatabase
            var mockService = new Mock<IAccessLogService>();
            mockService.Setup(service => service.ListAccessLogsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(logs);

            var controller = new AccessLogController(mockService.Object, null);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AccessLog>>(viewResult.ViewData.Model);
            Assert.Equal(3, (model as List<AccessLog>).Count);
        }

        [Fact]
        public async Task Index_ReturnsErrorView_WhenExceptionIsRaised()
        {
            // Arrange
            var mockService = new Mock<IAccessLogService>();
            var mockLogger = new Mock<ILogger<AccessLogController>>();

            mockService.Setup(service => service.ListAccessLogsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            var controller = new AccessLogController(mockService.Object, mockLogger.Object);

            // Act 
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Verify_ReturnsRedirect_WithSuccessfulInput()
        {
            // Arrange
            var mockService = new Mock<IAccessLogService>();
            var mockLogger = new Mock<ILogger<AccessLogController>>();

            mockService.Setup(service => service.VerifyNewRequestAsync(It.IsAny<AccessLog>()))
                .ReturnsAsync(true);
            
            var controller = new AccessLogController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Verify(It.IsAny<AccessLog>());

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Verify_ReturnsSameView_WithUnSuccessfulInput()
        {
            // Arrange
            var mockService = new Mock<IAccessLogService>();
            var mockLogger = new Mock<ILogger<AccessLogController>>();

            var accessAttempt = new AccessLog()
            {
                UserName = "test",
                EmailAddress = "test@test.com",
                DOB = DateTime.Now.AddYears(-1)
            };

            mockService.Setup(service => service.VerifyNewRequestAsync(It.IsAny<AccessLog>()))
                .ReturnsAsync(false);

            var controller = new AccessLogController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Verify(accessAttempt);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<AccessLog>(viewResult.ViewData.Model);
        }
    }
}
