using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AgeCheckConcept.Data;
using AgeCheckConcept.Models;

namespace AgeCheckConcept.Service
{
    public class AccessLogService : IAccessLogService
    {
        private readonly AccessLogContext _context;

        /// <summary>Minimum age required for access to be granted</summary>
        private const int m_permissibleAge = 18;

        /// <summary>Maximum number of failed attempts before account lock-out</summary>
        private const int m_maxRepeatFailedAttempts = 3;

        /// <summary>Duration in hours that failed attempted count towards lock-out count</summary>
        private const int m_failedAttmeptLockoutWindowHours = 1;

        public AccessLogService(AccessLogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves and sorts access log records for display
        /// </summary>
        /// <param name="sortField">Field to sort on</param>
        /// <param name="sortOrder">Order to sort in</param>
        /// <returns>Sorted list of access log records</returns>
        public async Task <IEnumerable<AccessLog>> ListAccessLogsAsync(string sortField, string sortOrder)
        {
            var accessLogs = from a in _context.AccessLogs
                             select a;

            accessLogs = SortLogs(accessLogs, sortField, sortOrder);

            return await accessLogs.ToListAsync();
        }

        /// <summary>
        /// Processes and saves a new access log record
        /// </summary>
        /// <param name="newAccessLog">New access log entry</param>
        /// <returns>True if successful</returns>
        public async Task<bool> VerifyNewRequestAsync(AccessLog newAccessLog)
        {
            newAccessLog.SubmittedDateTime = DateTime.Now;
            newAccessLog.IsSuccess = IsOfPermissableAge(newAccessLog.DOB);
            newAccessLog.IsLockedOut = IsLockedOut(newAccessLog);

            try
            {
                _context.Add(newAccessLog);
                await _context.SaveChangesAsync();

                return newAccessLog.IsSuccess && !newAccessLog.IsLockedOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sorts an AccessLog collection by the requested field, in the requested order
        /// </summary>
        /// <param name="accessLogs">AccessLog collection</param>
        /// <param name="sortField">Field to sort on</param>
        /// <param name="sortOrder">Order to sort in</param>
        /// <returns>Sorted collection of access log records</returns>
        private IQueryable<AccessLog> SortLogs(IQueryable<AccessLog> accessLogs, string sortField, string sortOrder)
        {
            System.Reflection.PropertyInfo sortProp = typeof(AccessLog).GetProperty(sortField);

            accessLogs = sortOrder == "desc" ?
                accessLogs.OrderByDescending(a => sortProp.GetValue(a, null)) :
                accessLogs.OrderBy(a => sortProp.GetValue(a, null));

            return accessLogs;
        }

        /// <summary>
        /// Calculate age based on date of birth and determines whether access
        /// should be permitted
        /// </summary>
        /// <param name="dob">Date of birth provided</param>
        /// <returns>True if the calculated age is greater than the permissible age</returns>
        private bool IsOfPermissableAge(DateTime dob)
        {
            DateTime dtNow = DateTime.Now;
            int ageInYears = dtNow.Year - dob.Year;
            dob = dob.AddYears(ageInYears);

            if (dob.Date > dtNow.Date)
                ageInYears--;

            return ageInYears >= m_permissibleAge;
        }

        /// <summary>
        /// Enforce an account lock-out if the same name/email address combination
        /// has been attempted and failed more than n times within the last n hours
        /// </summary>
        /// <param name="accessLog">Part-verified (unsaved) user input</param>
        /// <returns>True if the number of previous failed attempts exceeds the permitted value</returns>
        private bool IsLockedOut(AccessLog accessLog)
        {
            int previousAttempts = _context.AccessLogs.Where(
                a => a.UserName == accessLog.UserName &&
                a.EmailAddress == accessLog.EmailAddress &&
                a.IsSuccess == false &&
                a.SubmittedDateTime > DateTime.Now.AddHours(0 - m_failedAttmeptLockoutWindowHours)).Count();

            return previousAttempts >= m_maxRepeatFailedAttempts;
        }



        /// <summary>
        /// Identify an entry requested for deletion
        /// </summary>
        /// <param name="id">Unique ID of access log record</param>
        /// <returns>The identified access log record, or null</returns>
        public async Task<AccessLog> GetEntryToDelete(long id)
        {
            AccessLog accessLog = await _context.AccessLogs
                .FirstOrDefaultAsync(a => a.AccessLogId == id);

            return accessLog;
        }

        /// <summary>
        /// Deletes the identified access log record
        /// </summary>
        /// <param name="id">Unique ID of access log record</param>
        public async Task ConfirmDelete(long id)
        {
            var accessLog = await _context.AccessLogs.FindAsync(id);
            _context.AccessLogs.Remove(accessLog);
            await _context.SaveChangesAsync();
        }
    }
}
