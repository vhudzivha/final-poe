using Microsoft.AspNetCore.Mvc;


using Microsoft.AspNetCore.Http;

namespace CMCS.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, int claimId);
        bool ValidateFile(IFormFile file);
        string GetErrorMessage(IFormFile file);
    }
}