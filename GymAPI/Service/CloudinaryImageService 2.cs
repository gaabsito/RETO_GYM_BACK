// GymAPI/Services/CloudinaryImageService.cs
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GymAPI.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace GymAPI.Services
{
    public class CloudinaryImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder = "")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            // Determinar la carpeta basada en el tipo de imagen
            string uploadFolder = folder;
            if (string.IsNullOrEmpty(folder))
            {
                // Por defecto, determinar la carpeta por el contexto (puede ser usuarios, entrenamientos, etc.)
                if (file.Name.Contains("perfil") || file.Name.Contains("usuario"))
                {
                    uploadFolder = "usuarios";
                }
                else if (file.Name.Contains("entrenamiento") || file.Name.Contains("workout"))
                {
                    uploadFolder = "entrenamientos";
                }
                else
                {
                    uploadFolder = "general";
                }
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = uploadFolder,
                Transformation = new Transformation()
                    .Width(800).Height(600).Crop("fill").Gravity("auto")
            };

            // Si es imagen de perfil, ajustar transformación para un recorte cuadrado enfocado en caras
            if (uploadFolder == "usuarios")
            {
                uploadParams.Transformation = new Transformation()
                    .Width(500).Height(500).Crop("fill").Gravity("face");
            }

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return false;

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }

        public Task<string> UploadImageAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}