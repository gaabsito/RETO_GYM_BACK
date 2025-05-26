using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;
using GymAPI.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenamientoController : ControllerBase
    {
        private readonly IEntrenamientoService _entrenamientoService;
        private readonly IEntrenamientoEjercicioService _entrenamientoEjercicioService;
        private readonly IEjercicioService _ejercicioService;
        private readonly IImageService _imageService;

        public EntrenamientoController(
            IEntrenamientoService entrenamientoService,
            IEntrenamientoEjercicioService entrenamientoEjercicioService,
            IEjercicioService ejercicioService,
            IImageService imageService)
        {
            _entrenamientoService = entrenamientoService;
            _entrenamientoEjercicioService = entrenamientoEjercicioService;
            _ejercicioService = ejercicioService;
            _imageService = imageService;
        }

        // GET: api/Entrenamiento
        [HttpGet]
        public async Task<ActionResult<List<Entrenamiento>>> GetEntrenamientos()
        {
            var entrenamientos = await _entrenamientoService.GetAllAsync();
            return Ok(entrenamientos);
        }

        // GET: api/Entrenamiento/public
        [HttpGet("public")]
        public async Task<ActionResult<List<Entrenamiento>>> GetPublicEntrenamientos()
        {
            var entrenamientos = await _entrenamientoService.GetAllAsync();
            var publicEntrenamientos = entrenamientos.Where(e => e.Publico).ToList();
            return Ok(publicEntrenamientos);
        }

        // GET: api/Entrenamiento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entrenamiento>> GetEntrenamiento(int id)
        {
            var entrenamiento = await _entrenamientoService.GetByIdAsync(id);
            
            if (entrenamiento == null)
            {
                return NotFound(new ApiResponse<Entrenamiento>
                {
                    Success = false,
                    Message = "Entrenamiento no encontrado"
                });
            }

            return Ok(entrenamiento);
        }

        // POST: api/Entrenamiento
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Entrenamiento>>> CreateEntrenamiento([FromForm] EntrenamientoCreateRequest request)
        {
            try
            {
                // Obtener el ID del usuario del token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<Entrenamiento>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }
                
                // Crear el entrenamiento base
                var entrenamiento = new Entrenamiento
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion ?? "",
                    DuracionMinutos = request.DuracionMinutos,
                    Dificultad = request.Dificultad,
                    FechaCreacion = DateTime.Now,
                    Publico = request.Publico,
                    AutorID = userId
                };

                // Procesar la imagen si se proporciona
                if (request.Imagen != null)
                {
                    if (!FileValidationHelper.ValidateImageFile(request.Imagen, out string errorMessage))
                    {
                        return BadRequest(new ApiResponse<Entrenamiento>
                        {
                            Success = false,
                            Message = errorMessage
                        });
                    }

                    // Subir imagen a Cloudinary
                    string imageUrl = await _imageService.UploadImageAsync(request.Imagen);
                    entrenamiento.ImagenURL = imageUrl;
                }

                // Guardar el entrenamiento en la base de datos
                await _entrenamientoService.AddAsync(entrenamiento);

                // Procesar los ejercicios relacionados
                if (request.Ejercicios != null && request.Ejercicios.Any())
                {
                    foreach (var ejercicio in request.Ejercicios)
                    {
                        var entrenamientoEjercicio = new EntrenamientoEjercicio
                        {
                            EntrenamientoID = entrenamiento.EntrenamientoID,
                            EjercicioID = ejercicio.EjercicioID,
                            Series = ejercicio.Series,
                            Repeticiones = ejercicio.Repeticiones,
                            DescansoSegundos = ejercicio.DescansoSegundos,
                            Notas = ejercicio.Notas
                        };

                        await _entrenamientoEjercicioService.AddAsync(entrenamientoEjercicio);
                    }
                }

                return Ok(new ApiResponse<Entrenamiento>
                {
                    Success = true,
                    Data = entrenamiento,
                    Message = "Entrenamiento creado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Entrenamiento>
                {
                    Success = false,
                    Message = $"Error al crear el entrenamiento: {ex.Message}"
                });
            }
        }

        // PUT: api/Entrenamiento/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateEntrenamiento(int id, Entrenamiento entrenamiento)
        {
            if (id != entrenamiento.EntrenamientoID)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "ID de entrenamiento no coincide"
                });
            }

            try
            {
                // Verificar que el usuario solo pueda modificar sus propios entrenamientos
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var existingEntrenamiento = await _entrenamientoService.GetByIdAsync(id);
                if (existingEntrenamiento == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Entrenamiento no encontrado"
                    });
                }

                // Verificar que el usuario es el autor del entrenamiento
                if (existingEntrenamiento.AutorID != userId)
                {
                    return Forbid();
                }

                await _entrenamientoService.UpdateAsync(entrenamiento);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Entrenamiento actualizado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al actualizar el entrenamiento: {ex.Message}"
                });
            }
        }

        // POST: api/Entrenamiento/5/imagen
        [Authorize]
        [HttpPost("{id}/imagen")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateEntrenamientoImage(int id, IFormFile file)
        {
            try
            {
                // Verificar que el usuario solo pueda modificar sus propios entrenamientos
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var entrenamiento = await _entrenamientoService.GetByIdAsync(id);
                if (entrenamiento == null)
                {
                    return NotFound(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Entrenamiento no encontrado"
                    });
                }

                // Verificar que el usuario es el autor del entrenamiento
                if (entrenamiento.AutorID != userId)
                {
                    return Forbid();
                }

                // Validar el archivo
                if (!FileValidationHelper.ValidateImageFile(file, out string errorMessage))
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = errorMessage
                    });
                }

                // Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(entrenamiento.ImagenURL))
                {
                    try
                    {
                        // Extraer el publicId de la URL
                        var uri = new Uri(entrenamiento.ImagenURL);
                        var pathSegments = uri.PathAndQuery.Split('/');
                        string fileName = pathSegments.Last().Split('.')[0]; // Obtener el nombre sin extensión
                        string publicId = $"entrenamientos/{fileName}";
                        
                        await _imageService.DeleteImageAsync(publicId);
                    }
                    catch (Exception ex)
                    {
                        // Log el error pero continúa con la actualización
                        Console.WriteLine($"Error al eliminar imagen anterior: {ex.Message}");
                    }
                }

                // Subir nueva imagen a Cloudinary
                string imageUrl = await _imageService.UploadImageAsync(file);

                // Actualizar la URL de la imagen en la base de datos
                entrenamiento.ImagenURL = imageUrl;
                await _entrenamientoService.UpdateAsync(entrenamiento);

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = imageUrl,
                    Message = "Imagen del entrenamiento actualizada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error al actualizar la imagen: {ex.Message}"
                });
            }
        }

        // DELETE: api/Entrenamiento/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEntrenamiento(int id)
        {
            try
            {
                // Verificar que el usuario solo pueda eliminar sus propios entrenamientos
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var entrenamiento = await _entrenamientoService.GetByIdAsync(id);
                if (entrenamiento == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Entrenamiento no encontrado"
                    });
                }

                // Verificar que el usuario es el autor del entrenamiento
                if (entrenamiento.AutorID != userId)
                {
                    return Forbid();
                }

                // Eliminar la imagen si existe
                if (!string.IsNullOrEmpty(entrenamiento.ImagenURL))
                {
                    try
                    {
                        // Extraer el publicId de la URL
                        var uri = new Uri(entrenamiento.ImagenURL);
                        var pathSegments = uri.PathAndQuery.Split('/');
                        string fileName = pathSegments.Last().Split('.')[0]; // Obtener el nombre sin extensión
                        string publicId = $"entrenamientos/{fileName}";
                        
                        await _imageService.DeleteImageAsync(publicId);
                    }
                    catch (Exception ex)
                    {
                        // Log el error pero continúa con la eliminación
                        Console.WriteLine($"Error al eliminar imagen: {ex.Message}");
                    }
                }

                // Eliminar los ejercicios relacionados
                var ejercicios = await _entrenamientoEjercicioService.GetByEntrenamientoAsync(id);
                foreach (var ejercicio in ejercicios)
                {
                    await _entrenamientoEjercicioService.RemoveAsync(ejercicio.EntrenamientoID, ejercicio.EjercicioID);
                }

                // Eliminar el entrenamiento
                await _entrenamientoService.DeleteAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Entrenamiento eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar el entrenamiento: {ex.Message}"
                });
            }
        }

        // GET: api/Entrenamiento/ejercicios/5
        [HttpGet("ejercicios/{id}")]
        public async Task<ActionResult<ApiResponse<List<object>>>> GetEjerciciosDeEntrenamiento(int id)
        {
            try
            {
                var entrenamiento = await _entrenamientoService.GetByIdAsync(id);
                if (entrenamiento == null)
                {
                    return NotFound(new ApiResponse<List<object>>
                    {
                        Success = false,
                        Message = "Entrenamiento no encontrado"
                    });
                }

                var entrenamientoEjercicios = await _entrenamientoEjercicioService.GetByEntrenamientoAsync(id);
                
                var result = new List<object>();
                
                foreach (var ee in entrenamientoEjercicios)
                {
                    var ejercicio = await _ejercicioService.GetByIdAsync(ee.EjercicioID);
                    
                    if (ejercicio != null)
                    {
                        var ejercicioInfo = new
                        {
                            ejercicio.EjercicioID,
                            ejercicio.Nombre,
                            ejercicio.Descripcion,
                            ejercicio.GrupoMuscular,
                            ejercicio.ImagenURL,
                            ejercicio.VideoURL,
                            ee.Series,
                            ee.Repeticiones,
                            ee.DescansoSegundos,
                            ee.Notas
                        };
                        
                        result.Add(ejercicioInfo);
                    }
                }
                
                return Ok(new ApiResponse<List<object>>
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<object>>
                {
                    Success = false,
                    Message = $"Error al obtener los ejercicios del entrenamiento: {ex.Message}"
                });
            }
        }
    }

    // DTO para la solicitud de creación de entrenamiento
    public class EntrenamientoCreateRequest
    {
        public string Titulo { get; set; } = "";
        public string? Descripcion { get; set; }
        public int DuracionMinutos { get; set; }
        public string Dificultad { get; set; } = "";
        public bool Publico { get; set; }
        public IFormFile? Imagen { get; set; }
        public List<EntrenamientoEjercicioCreateDTO>? Ejercicios { get; set; }
    }

    public class EntrenamientoEjercicioCreateDTO
    {
        public int EjercicioID { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public int DescansoSegundos { get; set; }
        public string? Notas { get; set; }
    }
}