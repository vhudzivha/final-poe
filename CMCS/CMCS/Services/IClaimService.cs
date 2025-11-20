using Microsoft.AspNetCore.Mvc;



using CMCS.Models;

namespace CMCS.Services
{
    public interface IClaimService
    {
        Task<int> CreateClaimAsync(Claim claim);
        Task<Claim?> GetClaimByIdAsync(int claimId);
        Task<List<Claim>> GetUserClaimsAsync(string userId);
        Task<bool> UpdateClaimAsync(Claim claim);
        Task<bool> DeleteClaimAsync(int claimId);
        Task<bool> AddDocumentAsync(Document document);
    }
}

