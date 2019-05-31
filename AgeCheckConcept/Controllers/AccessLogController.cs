using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AgeCheckConcept.Models;
using AgeCheckConcept.Service;
using Microsoft.Extensions.Logging;
using System;

namespace AgeCheckConcept.Controllers
{
    public class AccessLogController : Controller
    {
        private readonly IAccessLogService _accessLogService;
        private readonly ILogger _logger;

        public AccessLogController(IAccessLogService service, ILogger<AccessLogController> logger)
        {
            _accessLogService = service;
            _logger = logger;
        }

        /// <summary>
        /// GET: AccessLog
        /// Presents a list of log entries
        /// </summary>
        /// <param name="sortField">Field to sort on, default SubmittedDateTime</param>
        /// <param name="sortOrder">Order to sort in, default desc</param>
        /// <returns>View: AccessLog/Index</returns>
        public async Task<IActionResult> Index(string sortField = "SubmittedDateTime", string sortOrder = "desc")
        {
            try
            {
                return View(await _accessLogService.ListAccessLogsAsync(sortField, sortOrder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(nameof(Error), new ErrorViewModel(ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// GET: AccessLog/Verify
        /// Presents the landing page with user inputs
        /// </summary>
        /// <returns>View: AccessLog/Verify</returns>
        public IActionResult Verify()
        {
            return View();
        }

        /// <summary>
        /// POST: AccessLog/Verify
        /// Verifies user input and grants/denies access
        /// </summary>
        /// <param name="accessLog">User input for verification</param>
        /// <returns>View AccessLog/Index if successful, or current view with validation feedback if not</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify([Bind("UserName,EmailAddress,DOB")] AccessLog accessLog)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _accessLogService.VerifyNewRequestAsync(accessLog))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return View(nameof(Error), new ErrorViewModel(ex.Message, ex.StackTrace));
                }
            }
            return View(accessLog);
        }

        /// <summary>
        /// Presents an error page
        /// </summary>
        /// <param name="ex">Exception thrown</param>
        /// <returns>Error view</returns>
        private IActionResult Error(Exception ex)
        {
            var errorModel = new ErrorViewModel(ex.Message, ex.StackTrace);
            return View(errorModel);
        }



        /// <summary>
        /// GET: AccessLog/Delete/5
        /// Retained deletion action methods for UI clean-up convenience
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessLog = await _accessLogService.GetEntryToDelete((long)id);

            if (accessLog == null)
            {
                return NotFound();
            }

            return View(accessLog);
        }

        /// <summary>
        /// POST: AccessLog/Delete/5
        /// Retained deletion action methods for UI clean-up convenience
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _accessLogService.ConfirmDelete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
