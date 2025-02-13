using GymAPI.DTOs;
using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
            var entrenamientosDTO = entrenamientos.Select(e => new EntrenamientoDTO
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
            }).ToList();

            return Ok(entrenamientosDTO);
        }

        // Obtener un entrenamiento por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EntrenamientoDTO>> GetEntrenamiento(int id)
        {
            var entrenamiento = await _service.GetByIdAsync(id);
            if (entrenamiento == null)
                return NotFound();

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

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
