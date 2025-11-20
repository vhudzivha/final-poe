

using CMCS.Models;

namespace CMCS.Services
{
    public interface IApprovalAutomationService
    {
        Task<bool> AutoValidateClaimAsync(Claim claim);
        Task<string> GetValidationResultAsync(Claim claim);
        Task<bool> CheckApprovalCriteriaAsync(Claim claim);
    }

    public class ApprovalAutomationService : IApprovalAutomationService
    {
        public async Task<bool> AutoValidateClaimAsync(Claim claim)
        {
            // AUTOMATED VALIDATION CHECKS
            var validationResults = new List<bool>
            {
                // Check 1: Hours worked is within acceptable range
                claim.HoursWorked >= 0.5m && claim.HoursWorked <= 200m,
                
                // Check 2: Hourly rate is within policy limits
                claim.HourlyRate >= 1m && claim.HourlyRate <= 5000m,
                
                // Check 3: Total amount is reasonable
                claim.TotalAmount <= 1000000m,
                
                // Check 4: Module code follows format
                !string.IsNullOrEmpty(claim.ModuleCode) && claim.ModuleCode.Length == 8,
                
                // Check 5: Work description is adequate
                !string.IsNullOrEmpty(claim.WorkDescription) && claim.WorkDescription.Length >= 20
            };

            return await Task.FromResult(validationResults.All(x => x));
        }

        public async Task<string> GetValidationResultAsync(Claim claim)
        {
            var issues = new List<string>();

            // Automated checks with specific feedback
            if (claim.HoursWorked < 0.5m || claim.HoursWorked > 200m)
                issues.Add($"Hours worked ({claim.HoursWorked}) is outside acceptable range (0.5-200)");

            if (claim.HourlyRate < 1m || claim.HourlyRate > 5000m)
                issues.Add($"Hourly rate (R{claim.HourlyRate}) is outside policy limits (R1-R5000)");

            if (claim.TotalAmount > 1000000m)
                issues.Add($"Total amount (R{claim.TotalAmount}) exceeds maximum limit (R1,000,000)");

            if (string.IsNullOrEmpty(claim.ModuleCode) || claim.ModuleCode.Length != 8)
                issues.Add("Module code format is invalid");

            if (string.IsNullOrEmpty(claim.WorkDescription) || claim.WorkDescription.Length < 20)
                issues.Add("Work description is insufficient (minimum 20 characters)");

            // Auto-approve criteria
            if (issues.Count == 0)
            {
                // Additional auto-approval checks
                if (claim.TotalAmount <= 5000m && claim.HoursWorked <= 50m)
                {
                    return await Task.FromResult("✓ RECOMMENDED FOR AUTO-APPROVAL: Low-risk claim within standard parameters");
                }
                else
                {
                    return await Task.FromResult("✓ Valid claim - Requires manual review due to amount/hours");
                }
            }

            return await Task.FromResult("❌ Validation Issues:\n" + string.Join("\n", issues));
        }

        public async Task<bool> CheckApprovalCriteriaAsync(Claim claim)
        {
            // AUTOMATED APPROVAL CRITERIA
            // Auto-approve if all conditions met:
            var autoApprovalCriteria = new[]
            {
                claim.TotalAmount <= 5000m,          // Low value claims
                claim.HoursWorked <= 50m,            // Standard hours
                claim.HourlyRate <= 150m,            // Standard rate
                !string.IsNullOrEmpty(claim.WorkDescription)
            };

            return await Task.FromResult(autoApprovalCriteria.All(x => x));
        }
    }
}