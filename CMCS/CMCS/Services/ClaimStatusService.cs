using Microsoft.AspNetCore.Mvc;



using CMCS.Models;

namespace CMCS.Services
{
    public class ClaimStatusService : IClaimStatusService
    {
        public Task<bool> UpdateClaimStatusAsync(int claimId, ClaimStatus newStatus, string userId, string comments)
        {
            // Stub implementation
            return Task.FromResult(true);
        }

        public Task<List<ClaimStatusHistory>> GetStatusHistoryAsync(int claimId)
        {
            // Stub implementation - returns sample data
            var history = new List<ClaimStatusHistory>
            {
                new ClaimStatusHistory
                {
                    StatusHistoryId = 1,
                    ClaimId = claimId,
                    OldStatus = ClaimStatus.Draft,
                    NewStatus = ClaimStatus.Submitted,
                    ChangedBy = "John Lecturer",
                    ChangedAt = DateTime.Now.AddDays(-5),
                    Comments = "Initial submission"
                }
            };
            return Task.FromResult(history);
        }

        public Task<int> GetProgressPercentageAsync(int claimId)
        {
            // Stub implementation
            return Task.FromResult(60);
        }

        public Task<string> GetStatusBadgeClassAsync(ClaimStatus status)
        {
            var badgeClass = status switch
            {
                ClaimStatus.Draft => "badge bg-secondary",
                ClaimStatus.Submitted => "badge bg-info",
                ClaimStatus.Pending => "badge bg-warning",
                ClaimStatus.UnderReview => "badge bg-warning",
                ClaimStatus.PreApproved => "badge bg-primary",
                ClaimStatus.Approved => "badge bg-success",
                ClaimStatus.Paid => "badge bg-dark",
                ClaimStatus.Rejected => "badge bg-danger",
                ClaimStatus.Cancelled => "badge bg-secondary",
                _ => "badge bg-light"
            };
            return Task.FromResult(badgeClass);
        }
    }
}
