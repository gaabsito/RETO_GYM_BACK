using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EjercicioController : ControllerBase
    {
        private readonly IEjercicioService _service;

        public EjercicioController(IEjercicioService service)
        {
            _service = service;
        }

        // Obtener todos los ejercicios
        [HttpGet]
        public async Task<ActionResult<List<Ejercicio>>> GetEjercicios()
        {
            var ejercicios = await _service.GetAllAsync();
            return Ok(ejercicios);
        }

        // Obtener un ejercicio por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Ejercicio>> GetEjercicio(int id)
        {
            var ejercicio = await _service.GetByIdAsync(id);
            if (ejercicio == null)
                return NotFound();

            return Ok(ejercicio);
        }

        // Crear un nuevo ejercicio
        [HttpPost]
        public async Task<ActionResult<Ejercicio>> CreateEjercicio([FromBody] Ejercicio ejercicio)
        {
            if (string.IsNullOrWhiteSpace(ejercicio.Nombre))
            {
                return BadRequest("El nombre del ejercicio es obligatorio.");
            }

            await _service.AddAsync(ejercicio);
            return CreatedAtAction(nameof(GetEjercicio), new { id = ejercicio.EjercicioID }, ejercicio);
        }

        // Actualizar un ejercicio
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEjercicio(int id, [FromBody] Ejercicio updatedEjercicio)
        {
            if (string.IsNullOrWhiteSpace(updatedEjercicio.Nombre))
            {
                return BadRequest("El nombre del ejercicio es obligatorio.");
            }

            var existingEjercicio = await _service.GetByIdAsync(id);
            if (existingEjercicio == null)
                return NotFound();

            updatedEjercicio.EjercicioID = id; // Asegurar que el ID no cambie
            await _service.UpdateAsync(updatedEjercicio);
            return NoContent();
        }

        // Eliminar un ejercicio
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEjercicio(int id)
        {
            var ejercicio = await _service.GetByIdAsync(id);
            if (ejercicio == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
