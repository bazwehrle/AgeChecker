using System;
using Microsoft.EntityFrameworkCore;
using AgeCheckConcept.Data;

namespace AgeCheckConcept.Tests.Data
{
    /// <summary>
    /// Utilises EFCore InMemoryDatabase for test data
    /// </summary>
    public class AccessLogTestBase : IDisposable
    {
        protected readonly AccessLogContext _context;

        public AccessLogTestBase()
        {
            var options = new DbContextOptionsBuilder<AccessLogContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AccessLogContext(options);

            _context.Database.EnsureCreated();

            AccessLogInitialiser.Initialise(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
