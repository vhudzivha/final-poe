using Microsoft.AspNetCore.Mvc;


using Microsoft.AspNetCore.Http;

namespace CMCS.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly string[] _allowedExtensions = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public Task<string> UploadFileAsync(IFormFile file, int claimId)
        {
            // Stub implementation
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            return Task.FromResult($"/uploads/{claimId}/{fileName}");
        }

        public bool ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > MaxFileSize)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return false;

            return true;
        }

        public string GetErrorMessage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "Please select a file to upload.";

            if (file.Length > MaxFileSize)
                return $"File size ({file.Length / 1024 / 1024}MB) exceeds maximum limit of 10MB.";

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return $"File type '{extension}' is not supported. Allowed: PDF, DOC, DOCX, JPG, PNG.";

            return "Invalid file.";
        }
    }
}