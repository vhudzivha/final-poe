using Microsoft.AspNetCore.Mvc;



using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CMCS.Models
{
    public class ClaimViewModel
    {
        [Display(Name = "Claim Period")]
        public string ClaimPeriod { get; set; } = string.Empty;

        [Display(Name = "Module/Course Code")]
        [StringLength(20)]
        public string ModuleCode { get; set; } = string.Empty;

        [Range(0.1, 200)]
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }

        [Range(1, 5000)]
        [Display(Name = "Hourly Rate (R)")]
        public decimal HourlyRate { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description of Work Performed")]
        public string WorkDescription { get; set; } = string.Empty;

        [Display(Name = "Supporting Documents")]
        public List<IFormFile>? Documents { get; set; }
    }
}