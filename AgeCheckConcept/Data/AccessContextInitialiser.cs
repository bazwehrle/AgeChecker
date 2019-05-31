using AgeCheckConcept.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgeCheckConcept.Data
{
    /// <summary>
    /// Called from Startup
    /// Used to perform initial database creation & population
    /// </summary>
    public class AccessLogInitialiser
    {
        /// <summary>
        /// Create and, if empty, seed database
        /// </summary>
        /// <param name="context">The DbContext</param>
        public static void Initialise(AccessLogContext context)
        {
            context.Database.EnsureCreated();

            if (context.AccessLogs.Any())
            {
                return;
            }

            Seed(context);
        }

        /// <summary>
        /// Add some initial seed data to the database
        /// </summary>
        /// <param name="context">The DbContext</param>
        public static void Seed(AccessLogContext context)
        {
            List<AccessLog> accessLogs = new List<AccessLog>
            {
                new AccessLog
                {
                    SubmittedDateTime = DateTime.Now.AddHours(-1),
                    UserName = "seeded_user1",
                    EmailAddress = "seeded_user1@test.com",
                    DOB = DateTime.Now.AddYears(-20),
                    IsSuccess = true,
                    IsLockedOut = false
                },
                new AccessLog
                {
                    SubmittedDateTime = DateTime.Now.AddHours(-2),
                    UserName = "seeded_user2",
                    EmailAddress = "seeded_user2@test.co.uk",
                    DOB = DateTime.Now.AddYears(-10),
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

            context.AccessLogs.AddRange(accessLogs);
            context.SaveChanges();
        }
    }
}
