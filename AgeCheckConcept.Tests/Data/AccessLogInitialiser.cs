using System;
using System.Linq;
using AgeCheckConcept.Data;
using AgeCheckConcept.Models;

namespace AgeCheckConcept.Tests.Data
{
    /// <summary>
    /// Sets up the InMemoryDatabase with some seed data for tests
    /// </summary>
    public class AccessLogInitialiser
    {
        public static void Initialise(AccessLogContext context)
        {
            if (context.AccessLogs.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(AccessLogContext context)
        {
            var accessLogs = new[]
            {
                new AccessLog
                {
                    SubmittedDateTime = DateTime.Now.AddHours(-1),
                    UserName = "seeded_user1",
                    EmailAddress = "seeded_user_1@test.com",
                    DOB = DateTime.Now.AddYears(-10),
                    IsSuccess = true,
                    IsLockedOut = false
                },
                new AccessLog
                {
                    SubmittedDateTime = DateTime.Now.AddHours(-2),
                    UserName = "seeded_user2",
                    EmailAddress = "seeded_user_2@test.co.uk",
                    DOB = DateTime.Now.AddYears(-20),
                    IsSuccess = false,
                    IsLockedOut = false
                },
                new AccessLog
                {
                    SubmittedDateTime = DateTime.Now.AddHours(-3),
                    UserName = "seeded_user3",
                    EmailAddress = "seeded_user_3@test.net",
                    DOB = DateTime.Now.AddYears(-30),
                    IsSuccess = true,
                    IsLockedOut = false
                }
            };

            context.AddRange(accessLogs);
            context.SaveChanges();
        }
    }
}
