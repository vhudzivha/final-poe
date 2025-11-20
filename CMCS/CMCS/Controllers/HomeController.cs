

using System.Security.Claims;
using CMCS.Models;
using CMCS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly IClaimStatusService _statusService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IClaimService claimService,
            IClaimStatusService statusService,
            IFileUploadService fileUploadService,
            ILogger<HomeController> logger)
        {
            _claimService = claimService;
            _statusService = statusService;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        // GET: Home/Index - Redirects to Login
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: Home/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            // No validation for prototype - just navigate to dashboard
            return RedirectToAction("Dashboard");
        }

        // GET: Home/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Home/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegistrationModel model)
        {
            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        // GET: Home/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Home/Claims
        public IActionResult Claims()
        {
            return View();
        }

        // POST: Home/SubmitClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim(ClaimViewModel model, List<IFormFile> documents)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Please correct the errors in the form.";
                    return View("Claims", model);
                }

                var claim = new System.Security.Claims.Claim
                {
                    ClaimPeriod = model.ClaimPeriod,
                    ModuleCode = model.ModuleCode,
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    TotalAmount = model.HoursWorked * model.HourlyRate,
                    WorkDescription = model.WorkDescription,
                    Status = ClaimStatus.Submitted,
                    SubmittedAt = DateTime.UtcNow,
                    LecturerId = "user123"
                };

                TempData["Success"] = "Claim submitted successfully! Your claim ID is CM-2024-" +
                                     DateTime.Now.Ticks.ToString().Substring(0, 4);
                _logger.LogInformation("Claim submitted successfully");

                return RedirectToAction("TrackClaims");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting claim");
                TempData["Error"] = "An error occurred while submitting your claim. Please try again.";
                return View("Claims", model);
            }
        }

        // GET: Home/TrackClaims
        public IActionResult TrackClaims()
        {
            var claims = GetSampleClaims();
            return View(claims);
        }

        // GET: Home/PreApprove
        public IActionResult PreApprove()
        {
            var claims = GetSamplePreApprovalClaims();
            return View(claims);
        }

        // POST: Home/PreApproveClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PreApproveClaim(int claimId, string comments)
        {
            try
            {
                TempData["Success"] = $"Claim {claimId} has been pre-approved successfully!";
                _logger.LogInformation($"Claim {claimId} pre-approved");
                return RedirectToAction("PreApprove");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error pre-approving claim {claimId}");
                TempData["Error"] = "An error occurred while processing the claim.";
                return RedirectToAction("PreApprove");
            }
        }

        // POST: Home/RejectClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectClaim(int claimId, string reason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    TempData["Error"] = "Rejection reason is required.";
                    return RedirectToAction("PreApprove");
                }

                TempData["Success"] = $"Claim {claimId} has been rejected.";
                _logger.LogInformation($"Claim {claimId} rejected. Reason: {reason}");
                return RedirectToAction("PreApprove");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error rejecting claim {claimId}");
                TempData["Error"] = "An error occurred while processing the claim.";
                return RedirectToAction("PreApprove");
            }
        }

        // GET: Home/Approve
        public IActionResult Approve()
        {
            var claims = GetSampleFinalApprovalClaims();
            return View(claims);
        }

        // POST: Home/ApproveClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveClaim(int claimId, string comments)
        {
            try
            {
                TempData["Success"] = $"Claim {claimId} has been approved and will be processed for payment.";
                _logger.LogInformation($"Claim {claimId} approved");
                return RedirectToAction("Approve");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error approving claim {claimId}");
                TempData["Error"] = "An error occurred while processing the claim.";
                return RedirectToAction("Approve");
            }
        }

        // POST: Home/FinalRejectClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalRejectClaim(int claimId, string reason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    TempData["Error"] = "Rejection reason is required.";
                    return RedirectToAction("Approve");
                }

                TempData["Success"] = $"Claim {claimId} has been rejected at final approval stage.";
                _logger.LogInformation($"Claim {claimId} rejected at final stage. Reason: {reason}");
                return RedirectToAction("Approve");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error rejecting claim {claimId}");
                TempData["Error"] = "An error occurred while processing the claim.";
                return RedirectToAction("Approve");
            }
        }

        // GET: Home/Logout
        public IActionResult Logout()
        {
            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        // Helper methods for prototype data
        private List<System.Security.Claims.Claim> GetSampleClaims()
        {
            return new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim { ClaimId = 1, ClaimPeriod = "March 2024", ModuleCode = "PROG6212", HoursWorked = 45, HourlyRate = 100, TotalAmount = 4500, Status = ClaimStatus.Pending, SubmittedAt = DateTime.Now.AddDays(-2) },
                new System.Security.Claims.Claim { ClaimId = 2, ClaimPeriod = "February 2024", ModuleCode = "PROG6212", HoursWorked = 40, HourlyRate = 100, TotalAmount = 4000, Status = ClaimStatus.Approved, SubmittedAt = DateTime.Now.AddDays(-15) },
                new System.Security.Claims.Claim { ClaimId = 3, ClaimPeriod = "January 2024", ModuleCode = "PROG6212", HoursWorked = 50, HourlyRate = 100, TotalAmount = 5000, Status = ClaimStatus.Paid, SubmittedAt = DateTime.Now.AddDays(-45) },
                new System.Security.Claims.Claim { ClaimId = 4, ClaimPeriod = "December 2023", ModuleCode = "PROG6212", HoursWorked = 35, HourlyRate = 100, TotalAmount = 3500, Status = ClaimStatus.Rejected, SubmittedAt = DateTime.Now.AddDays(-60) }
            };
        }

        private List<System.Security.Claims.Claim> GetSamplePreApprovalClaims()
        {
            return new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim { ClaimId = 5, LecturerName = "John Smith", ClaimPeriod = "March 2024", ModuleCode = "PROG6212", HoursWorked = 45, HourlyRate = 100, TotalAmount = 4500, SubmittedAt = DateTime.Now.AddDays(-1) },
                new System.Security.Claims.Claim { ClaimId = 6, LecturerName = "Mary Johnson", ClaimPeriod = "March 2024", ModuleCode = "PROG5121", HoursWorked = 38, HourlyRate = 100, TotalAmount = 3800, SubmittedAt = DateTime.Now.AddDays(-2) },
                new System.Security.Claims.Claim { ClaimId = 7, LecturerName = "David Wilson", ClaimPeriod = "March 2024", ModuleCode = "PROG6212", HoursWorked = 42, HourlyRate = 100, TotalAmount = 4200, SubmittedAt = DateTime.Now.AddDays(-3) }
            };
        }

        private List<System.Security.Claims.Claim> GetSampleFinalApprovalClaims()
        {
            return new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim { ClaimId = 8, LecturerName = "Robert Taylor", ClaimPeriod = "February 2024", ModuleCode = "PROG6212", HoursWorked = 40, HourlyRate = 100, TotalAmount = 4000, PreApprovedBy = "Sarah Coordinator", PreApprovedDate = DateTime.Now.AddDays(-5) },
                new System.Security.Claims.Claim { ClaimId = 9, LecturerName = "Emma Davis", ClaimPeriod = "February 2024", ModuleCode = "PROG5121", HoursWorked = 35, HourlyRate = 100, TotalAmount = 3500, PreApprovedBy = "Sarah Coordinator", PreApprovedDate = DateTime.Now.AddDays(-4) },
                new System.Security.Claims.Claim { ClaimId = 10, LecturerName = "James Miller", ClaimPeriod = "February 2024", ModuleCode = "PROG6212", HoursWorked = 44, HourlyRate = 100, TotalAmount = 4400, PreApprovedBy = "Sarah Coordinator", PreApprovedDate = DateTime.Now.AddDays(-3) }
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
