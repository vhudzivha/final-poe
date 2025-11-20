using Microsoft.AspNetCore.Mvc;


namespace CMCS.Models
{
    public enum ClaimStatus
    {
        Draft = 1,
        Submitted = 2,
        Pending = 3,
        UnderReview = 4,
        PreApproved = 5,
        Approved = 6,
        Rejected = 7,
        Paid = 8,
        Cancelled = 9
    }
}