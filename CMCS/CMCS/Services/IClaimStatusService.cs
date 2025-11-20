using Microsoft.AspNetCore.Mvc;



using CMCS.Models;

namespace CMCS.Services
{
    public interface IClaimStatusService
    {
        Task<bool> UpdateClaimStatusAsync(int claimId, ClaimStatus newStatus, string userId, string comments);
        Task<List<ClaimStatusHistory>> GetStatusHistoryAsync(int claimId);
        Task<int> GetProgressPercentageAsync(int claimId);
        Task<string> GetStatusBadgeClassAsync(ClaimStatus status);
    }
}