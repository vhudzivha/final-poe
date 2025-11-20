using Microsoft.AspNetCore.Mvc;

namespace CMCS.Models
{
    public class ClaimStatusHistory
    {
        public int StatusHistoryId { get; set; }
        public int ClaimId { get; set; }
        public ClaimStatus OldStatus { get; set; }
        public ClaimStatus NewStatus { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public string Comments { get; set; } = string.Empty;
    }
}
