using GymAPI.DTOs;
using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenamientoController : ControllerBase
    {
        private readonly IEntrenamientoService _service;

        public EntrenamientoController(IEntrenamientoService service)
        {
            _service = service;
        }

        // Obtener todos los entrenamientos
        [HttpGet]
        public async Task<ActionResult<List<EntrenamientoDTO>>> GetEntrenamientos()
        {
            var entrenamientos = await _service.GetAllAsync();
            var entrenamientosDTO = new List<EntrenamientoDTO>();
            
            // Obtener el ID del usuario autenticado (si existe)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = null;
            if (int.TryParse(userIdClaim, out int parsedId))
            {
                userId = parsedId;
            }
            
            // Filtrar los entrenamientos según la autenticación
            foreach (var e in entrenamientos)
            {
                // Incluir el entrenamiento si:
                // 1. Es público, o
                // 2. El usuario está autenticado y es el autor del entrenamiento
                if (e.Publico || (userId.HasValue && e.AutorID == userId.Value))
                {
                    entrenamientosDTO.Add(new EntrenamientoDTO
                    {
                        EntrenamientoID = e.EntrenamientoID,
                        Titulo = e.Titulo,
                        Descripcion = e.Descripcion,
                        DuracionMinutos = e.DuracionMinutos,
                        Dificultad = e.Dificultad,
                        ImagenURL = e.ImagenURL,
                        FechaCreacion = e.FechaCreacion,
                        Publico = e.Publico,
                        AutorID = e.AutorID
                    });
                }
            }

            return Ok(entrenamientosDTO);
        }

        // Obtener un entrenamiento por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EntrenamientoDTO>> GetEntrenamiento(int id)
        {
            var entrenamiento = await _service.GetByIdAsync(id);
            if (entrenamiento == null)
                return NotFound();

            // Obtener el ID del usuario autenticado (si existe)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = null;
            if (int.TryParse(userIdClaim, out int parsedId))
            {
                userId = parsedId;
            }
            
            // Verificar si el usuario puede acceder a este entrenamiento
            if (!entrenamiento.Publico && (userId == null || entrenamiento.AutorID != userId.Value))
            {
                return Forbid(); // El usuario no tiene permiso para ver este entrenamiento
            }

            var entrenamientoDTO = new EntrenamientoDTO
            {
                EntrenamientoID = entrenamiento.EntrenamientoID,
                Titulo = entrenamiento.Titulo,
                Descripcion = entrenamiento.Descripcion,
                DuracionMinutos = entrenamiento.DuracionMinutos,
                Dificultad = entrenamiento.Dificultad,
                ImagenURL = entrenamiento.ImagenURL,
                FechaCreacion = entrenamiento.FechaCreacion,
                Publico = entrenamiento.Publico,
                AutorID = entrenamiento.AutorID
            };

            return Ok(entrenamientoDTO);
        }

        // Crear un nuevo entrenamiento
        [HttpPost]
        public async Task<ActionResult<EntrenamientoDTO>> CreateEntrenamiento([FromBody] EntrenamientoCreateDTO entrenamientoDTO)
        {
            var entrenamiento = new Entrenamiento
            {
                Titulo = entrenamientoDTO.Titulo,
                Descripcion = entrenamientoDTO.Descripcion,
                DuracionMinutos = entrenamientoDTO.DuracionMinutos,
                Dificultad = entrenamientoDTO.Dificultad,
                ImagenURL = entrenamientoDTO.ImagenURL,
                Publico = entrenamientoDTO.Publico,
                AutorID = entrenamientoDTO.AutorID
            };

            await _service.AddAsync(entrenamiento);

            return CreatedAtAction(nameof(GetEntrenamiento), new { id = entrenamiento.EntrenamientoID }, new EntrenamientoDTO
            {
                EntrenamientoID = entrenamiento.EntrenamientoID,
                Titulo = entrenamiento.Titulo,
                Descripcion = entrenamiento.Descripcion,
                DuracionMinutos = entrenamiento.DuracionMinutos,
                Dificultad = entrenamiento.Dificultad,
                ImagenURL = entrenamiento.ImagenURL,
                FechaCreacion = entrenamiento.FechaCreacion,
                Publico = entrenamiento.Publico,
                AutorID = entrenamiento.AutorID
            });
        }

        // Actualizar un entrenamiento
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntrenamiento(int id, [FromBody] EntrenamientoUpdateDTO entrenamientoDTO)
        {
            var existingEntrenamiento = await _service.GetByIdAsync(id);
            if (existingEntrenamiento == null)
                return NotFound();

            // Verificar que el usuario autenticado sea el autor (para mayor seguridad)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId) && existingEntrenamiento.AutorID != userId)
            {
                return Forbid(); // Solo el autor puede modificar el entrenamiento
            }

            if (!string.IsNullOrWhiteSpace(entrenamientoDTO.Titulo))
                existingEntrenamiento.Titulo = entrenamientoDTO.Titulo;

            if (!string.IsNullOrWhiteSpace(entrenamientoDTO.Descripcion))
                existingEntrenamiento.Descripcion = entrenamientoDTO.Descripcion;

            if (entrenamientoDTO.DuracionMinutos.HasValue)
                existingEntrenamiento.DuracionMinutos = entrenamientoDTO.DuracionMinutos.Value;

            if (!string.IsNullOrWhiteSpace(entrenamientoDTO.Dificultad))
                existingEntrenamiento.Dificultad = entrenamientoDTO.Dificultad;

            if (entrenamientoDTO.ImagenURL != null)
                existingEntrenamiento.ImagenURL = entrenamientoDTO.ImagenURL;

            if (entrenamientoDTO.Publico.HasValue)
                existingEntrenamiento.Publico = entrenamientoDTO.Publico.Value;

            await _service.UpdateAsync(existingEntrenamiento);
            return NoContent();
        }

        // Eliminar un entrenamiento
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntrenamiento(int id)
        {
            var entrenamiento = await _service.GetByIdAsync(id);
            if (entrenamiento == null)
                return NotFound();

            // Verificar que el usuario autenticado sea el autor (para mayor seguridad)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId) && entrenamiento.AutorID != userId)
            {
                return Forbid(); // Solo el autor puede eliminar el entrenamiento
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}