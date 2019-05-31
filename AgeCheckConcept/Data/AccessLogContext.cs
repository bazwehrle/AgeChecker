using AgeCheckConcept.Models;
using Microsoft.EntityFrameworkCore;

namespace AgeCheckConcept.Data
{
    /// <summary>
    /// The DbContext for interacting with AccessLog db records
    /// </summary>
    public class AccessLogContext : DbContext
    {
        /// <summary>The AccessLogs repository</summary>
        public DbSet<AccessLog> AccessLogs { get; set; }

        public AccessLogContext() { }

        public AccessLogContext(DbContextOptions<AccessLogContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;database=AccessLogDb;trusted_connection=true;multipleactiveresultsets=true;");
            }
        }
    }
}
