using Microsoft.AspNetCore.Mvc;



using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        public string? LecturerId { get; set; }

        public string? LecturerName { get; set; }

        [Required]
        [Display(Name = "Claim Period")]
        public string ClaimPeriod { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Module Code")]
        [StringLength(20)]
        public string ModuleCode { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 200)]
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [Required]
        [Range(1, 5000)]
        [Display(Name = "Hourly Rate (R)")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Work Description")]
        public string WorkDescription { get; set; } = string.Empty;

        public ClaimStatus Status { get; set; } = ClaimStatus.Draft;

        [Display(Name = "Submitted Date")]
        public DateTime? SubmittedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // For Pre-Approval tracking
        public string? PreApprovedBy { get; set; }
        public DateTime? PreApprovedDate { get; set; }

        // For Final Approval tracking
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        // Method to calculate total
        public decimal CalculateTotalAmount()
        {
            return HoursWorked * HourlyRate;
        }

        // Method to get status badge class
        public string GetStatusBadgeClass()
        {
            return Status switch
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
        }

        // Method to get status display text
        public string GetStatusDisplayText()
        {
            return Status switch
            {
                ClaimStatus.Draft => "Draft",
                ClaimStatus.Submitted => "Submitted",
                ClaimStatus.Pending => "Pending Review",
                ClaimStatus.UnderReview => "Under Review",
                ClaimStatus.PreApproved => "Pre-Approved",
                ClaimStatus.Approved => "Approved",
                ClaimStatus.Paid => "Paid",
                ClaimStatus.Rejected => "Rejected",
                ClaimStatus.Cancelled => "Cancelled",
                _ => "Unknown"
            };
        }
    }
}