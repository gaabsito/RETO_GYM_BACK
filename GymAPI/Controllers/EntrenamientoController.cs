using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<List<Entrenamiento>>> GetEntrenamientos()
        {
            var entrenamientos = await _service.GetAllAsync();
            return Ok(entrenamientos);
        }

        // Obtener un entrenamiento por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Entrenamiento>> GetEntrenamiento(int id)
        {
            var entrenamiento = await _service.GetByIdAsync(id);
            if (entrenamiento == null)
                return NotFound();
            return Ok(entrenamiento);
        }

        // Crear un nuevo entrenamiento
        [HttpPost]
        public async Task<ActionResult<Entrenamiento>> CreateEntrenamiento([FromBody] Entrenamiento entrenamiento)
        {
            // Validar que la dificultad es una de las permitidas
            var dificultadesValidas = new HashSet<string> { "Fácil", "Media", "Difícil" };
            if (!dificultadesValidas.Contains(entrenamiento.Dificultad))
            {
                return BadRequest($"La dificultad '{entrenamiento.Dificultad}' no es válida. Usa: 'Fácil', 'Media' o 'Difícil'.");
            }

            await _service.AddAsync(entrenamiento);
            return CreatedAtAction(nameof(GetEntrenamiento), new { id = entrenamiento.EntrenamientoID }, entrenamiento);
        }

        // Actualizar un entrenamiento
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntrenamiento(int id, [FromBody] Entrenamiento updatedEntrenamiento)
        {
            var existingEntrenamiento = await _service.GetByIdAsync(id);
            if (existingEntrenamiento == null)
                return NotFound();

            // Validar que la dificultad es una de las permitidas
            var dificultadesValidas = new HashSet<string> { "Fácil", "Media", "Difícil" };
            if (!dificultadesValidas.Contains(updatedEntrenamiento.Dificultad))
            {
                return BadRequest($"La dificultad '{updatedEntrenamiento.Dificultad}' no es válida. Usa: 'Fácil', 'Media' o 'Difícil'.");
            }

            await _service.UpdateAsync(updatedEntrenamiento);
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
