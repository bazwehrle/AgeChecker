using System.Collections.Generic;
using System.Threading.Tasks;
using AgeCheckConcept.Models;

namespace AgeCheckConcept.Service
{
    public interface IAccessLogService
    {
        Task<IEnumerable<AccessLog>> ListAccessLogsAsync(string sortField, string sortOrder);
        Task<bool> VerifyNewRequestAsync(AccessLog accessLog);
        Task<AccessLog> GetEntryToDelete(long id);
        Task ConfirmDelete(long id);
    }
}
