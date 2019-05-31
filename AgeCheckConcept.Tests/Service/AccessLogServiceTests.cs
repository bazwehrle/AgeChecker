using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgeCheckConcept.Models;
using AgeCheckConcept.Service;
using AgeCheckConcept.Tests.Data;
using System;

namespace AgeCheckConcept.Tests
{
    [TestClass]
    public class AccessLogServiceTests : AccessLogTestBase
    {
        [TestMethod]
        public async Task ListAccessLogs_ReturnsCorrectType()
        {
            // Arrange
            var service = new AccessLogService(_context);

            // Act
            var result = await service.ListAccessLogsAsync("SubmittedDateTime", "desc");

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<AccessLog>));
        }

        [TestMethod]
        public async Task ListAccessLogs_ReturnsAllAccessLogs()
        {
            // Arrange
            var service = new AccessLogService(_context);

            // Act
            var result = await service.ListAccessLogsAsync("SubmittedDateTime", "desc");

            // Assert
            Assert.AreEqual((result as List<AccessLog>).Count, 3);
        }

        [TestMethod]
        public async Task ListAccessLogs_ReturnsAccessLogsInExpectedOrder_SortedOnUserName()
        {
            // Arrange
            var service = new AccessLogService(_context);

            // Act
            var result = await service.ListAccessLogsAsync("UserName", "desc");

            // Assert
            Assert.AreEqual((result as List<AccessLog>)[0].UserName, "seeded_user3");
            Assert.AreEqual((result as List<AccessLog>)[1].UserName, "seeded_user2");
            Assert.AreEqual((result as List<AccessLog>)[2].UserName, "seeded_user1");
        }

        [TestMethod]
        public async Task ListAccessLogs_ReturnsAccessLogsInExpectedOrder_SortedOnEmail()
        {
            // Arrange
            var service = new AccessLogService(_context);

            // Act
            var result = await service.ListAccessLogsAsync("EmailAddress", "asc");

            // Assert
            Assert.AreEqual((result as List<AccessLog>)[0].UserName, "seeded_user1");
            Assert.AreEqual((result as List<AccessLog>)[1].UserName, "seeded_user2");
            Assert.AreEqual((result as List<AccessLog>)[2].UserName, "seeded_user3");
        }

        [TestMethod]
        public async Task VerifyNewRequestAsync_ReturnsTrue_WhenPermissible()
        {
            // Arrange
            var service = new AccessLogService(_context);

            var accessLog = new AccessLog
            {
                UserName = "test",
                EmailAddress = "test@email.com",
                DOB = DateTime.Now.AddYears(-20)
            };

            // Act
            var result = await service.VerifyNewRequestAsync(accessLog);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task VerifyNewRequestAsync_ReturnsFalse_WhenNotPermissible()
        {
            // Arrange
            var service = new AccessLogService(_context);

            var accessLog = new AccessLog
            {
                UserName = "test",
                EmailAddress = "test@email.com",
                DOB = DateTime.Now.AddYears(-10)
            };

            // Act
            var result = await service.VerifyNewRequestAsync(accessLog);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task VerifyNewRequestAsync_ReturnsFalse_WhenLockedOut()
        {
            // Arrange
            var service = new AccessLogService(_context);

            var previousAttempts = new List<AccessLog>()
            {
                new AccessLog
                {
                    IsSuccess = false,
                    IsLockedOut = false,
                    SubmittedDateTime = DateTime.Now.AddMinutes(-3),
                    UserName = "lockmeout",
                    EmailAddress = "lockmeout@email.com",
                    DOB = DateTime.Now.AddYears(-10)
                },
                new AccessLog
                {
                    IsSuccess = false,
                    IsLockedOut = false,
                    SubmittedDateTime = DateTime.Now.AddMinutes(-2),
                    UserName = "lockmeout",
                    EmailAddress = "lockmeout@email.com",
                    DOB = DateTime.Now.AddYears(-10)
                },
                new AccessLog
                {
                    IsSuccess = false,
                    IsLockedOut = false,
                    SubmittedDateTime = DateTime.Now.AddMinutes(-1),
                    UserName = "lockmeout",
                    EmailAddress = "lockmeout@email.com",
                    DOB = DateTime.Now.AddYears(-10)
                }
            };

            _context.AddRange(previousAttempts);
            _context.SaveChanges();


            var lockoutAttempt = new AccessLog
            {
                UserName = "lockmeout",
                EmailAddress = "lockmeout@email.com",
                DOB = DateTime.Now.AddYears(-20)
            };

            // Act
            var result = await service.VerifyNewRequestAsync(lockoutAttempt);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
