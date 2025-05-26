// GymAPI/Services/IImageService.cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GymAPI.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder = "");
        Task<bool> DeleteImageAsync(string publicId);
    }
}