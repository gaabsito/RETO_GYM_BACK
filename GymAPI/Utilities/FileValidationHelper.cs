// GymAPI/Utilities/FileValidationHelper.cs
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace GymAPI.Utilities
{
    public static class FileValidationHelper
    {
        public static bool ValidateImageFile(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Verificar si hay un archivo
            if (file == null || file.Length == 0)
            {
                errorMessage = "No se ha proporcionado ningún archivo";
                return false;
            }

            // Verificar tamaño (máximo 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                errorMessage = "El archivo es demasiado grande. Máximo 5MB.";
                return false;
            }

            // Verificar extensión de archivo
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                errorMessage = "Formato de archivo no válido. Use JPG, JPEG, PNG, WEBP o GIF.";
                return false;
            }

            // Verificar tipo MIME
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            
            if (!allowedMimeTypes.Contains(file.ContentType))
            {
                errorMessage = "Tipo de contenido no válido. Use imágenes JPG, PNG, WEBP o GIF.";
                return false;
            }

            return true;
        }
    }
}