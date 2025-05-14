using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> GetUsuarios()
        {
            var usuarios = await _service.GetAllAsync();
            var usuariosDTO = usuarios.Select(u => new UsuarioDTO
            {
                UsuarioID = u.UsuarioID,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                FechaRegistro = u.FechaRegistro,
                EstaActivo = u.EstaActivo,
                FotoPerfilURL = u.FotoPerfilURL
            }).ToList();

            return Ok(usuariosDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            var usuarioDTO = new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                EstaActivo = usuario.EstaActivo,
                FotoPerfilURL = usuario.FotoPerfilURL
            };

            return Ok(usuarioDTO);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CreateUsuario(UsuarioCreateDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Email = usuarioDTO.Email,
                Password = usuarioDTO.Password
            };

            await _service.AddAsync(usuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioID }, new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                EstaActivo = usuario.EstaActivo,
                FotoPerfilURL = usuario.FotoPerfilURL
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, ProfileUpdateDTO usuarioDTO)
        {
            // Verificar que el usuario solo pueda modificar su propio perfil
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId) || userId != id)
            {
                return Forbid();
            }

            var existingUsuario = await _service.GetByIdAsync(id);
            if (existingUsuario == null)
                return NotFound();

            // Si se está intentando cambiar la contraseña
            if (!string.IsNullOrEmpty(usuarioDTO.CurrentPassword) && !string.IsNullOrEmpty(usuarioDTO.NewPassword))
            {
                // Verificar la contraseña actual
                if (!BCrypt.Net.BCrypt.Verify(usuarioDTO.CurrentPassword, existingUsuario.Password))
                {
                    return BadRequest(new { message = "La contraseña actual es incorrecta" });
                }

                // Actualizar la contraseña
                existingUsuario.Password = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.NewPassword);
            }

            // Actualizar los demás campos si se proporcionaron
            if (!string.IsNullOrWhiteSpace(usuarioDTO.Nombre))
                existingUsuario.Nombre = usuarioDTO.Nombre.Trim();

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Apellido))
                existingUsuario.Apellido = usuarioDTO.Apellido.Trim();

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Email))
            {
                // Verificar si el email ya existe para otro usuario
                var existingUserWithEmail = await _service.GetByEmailAsync(usuarioDTO.Email.ToLower().Trim());
                if (existingUserWithEmail != null && existingUserWithEmail.UsuarioID != id)
                {
                    return BadRequest(new { message = "El email ya está en uso por otro usuario" });
                }
                existingUsuario.Email = usuarioDTO.Email.ToLower().Trim();
            }

            // Asignar los campos nuevos si vienen en el DTO
            if (usuarioDTO.Edad.HasValue)
                existingUsuario.Edad = usuarioDTO.Edad.Value;

            if (usuarioDTO.Altura.HasValue)
                existingUsuario.Altura = usuarioDTO.Altura.Value;

            if (usuarioDTO.Peso.HasValue)
                existingUsuario.Peso = usuarioDTO.Peso.Value;

            // Guardar cambios
            await _service.UpdateAsync(existingUsuario);

            return Ok(new { message = "Perfil actualizado correctamente" });
        }

        [Authorize]
        [HttpPost("{id}/foto")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateProfilePhoto(int id, IFormFile file)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId) || userId != id)
                {
                    return Forbid();
                }

                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    });
                }

                // Validar el archivo
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "No se ha proporcionado ningún archivo"
                    });
                }

                // Validar formato (solo imágenes)
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "El formato de archivo no es válido. Use JPEG, PNG o GIF."
                    });
                }

                // Validar tamaño (máximo 2MB)
                if (file.Length > 2 * 1024 * 1024)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "El archivo es demasiado grande. Máximo 2MB."
                    });
                }

                // Eliminar la foto anterior si existe
                if (!string.IsNullOrEmpty(user.FotoPerfilURL))
                {
                    string oldFileName = Path.GetFileName(user.FotoPerfilURL);
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldFileName);
                    
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Generar un nombre de archivo único
                string fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
                
                // Crear el directorio si no existe
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Guardar el archivo
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Actualizar la URL de la foto de perfil en la base de datos
                string fileUrl = $"/uploads/profiles/{fileName}";
                user.FotoPerfilURL = fileUrl;
                await _service.UpdateAsync(user);

                // Devolver la URL de la foto
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = fileUrl,
                    Message = "Foto de perfil actualizada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error al actualizar la foto de perfil: {ex.Message}"
                });
            }
        }

        [Authorize]
        [HttpDelete("{id}/foto")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveProfilePhoto(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId) || userId != id)
                {
                    return Forbid();
                }

                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    });
                }

                // Si el usuario no tiene foto de perfil, no hacer nada
                if (string.IsNullOrEmpty(user.FotoPerfilURL))
                {
                    return Ok(new ApiResponse<bool>
                    {
                        Success = true,
                        Data = true,
                        Message = "El usuario no tiene foto de perfil"
                    });
                }

                // Eliminar el archivo si existe
                string fileName = Path.GetFileName(user.FotoPerfilURL);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles", fileName);
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Actualizar el usuario en la base de datos
                user.FotoPerfilURL = null;
                await _service.UpdateAsync(user);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Foto de perfil eliminada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar la foto de perfil: {ex.Message}"
                });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> GetProfile()
        {
            try
            {
                // Log el token recibido
                var authHeader = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Token recibido: {authHeader}");

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"UserIdClaim: {userIdClaim}"); // Ver si se está extrayendo el claim

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    Console.WriteLine("Error al parsear el userId"); // Log si falla el parsing
                    return Unauthorized(new ApiResponse<UsuarioDTO>
                    {
                        Success = false,
                        Message = "Token inválido"
                    });
                }

                Console.WriteLine($"Buscando usuario con ID: {userId}"); // Log del ID que vamos a buscar
                var usuario = await _service.GetByIdAsync(userId);

                if (usuario == null)
                {
                    Console.WriteLine("Usuario no encontrado"); // Log si no se encuentra el usuario
                    return NotFound(new ApiResponse<UsuarioDTO>
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    });
                }

                Console.WriteLine($"Usuario encontrado: {usuario.Email}"); // Log si encontramos el usuario

                var usuarioDTO = new UsuarioDTO
                {
                    UsuarioID = usuario.UsuarioID,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    EstaActivo = usuario.EstaActivo,
                    FotoPerfilURL = usuario.FotoPerfilURL
                };

                return Ok(new ApiResponse<UsuarioDTO>
                {
                    Success = true,
                    Data = usuarioDTO
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); // Log de cualquier excepción
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new ApiResponse<UsuarioDTO>
                {
                    Success = false,
                    Message = "Error interno del servidor"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}