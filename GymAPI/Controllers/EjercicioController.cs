using GymAPI.DTOs;
using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<List<EjercicioDTO>>> GetEjercicios()
        {
            var ejercicios = await _service.GetAllAsync();
            var ejerciciosDTO = ejercicios.Select(e => new EjercicioDTO
            {
                EjercicioID = e.EjercicioID,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                GrupoMuscular = e.GrupoMuscular,
                ImagenURL = e.ImagenURL,
                EquipamientoNecesario = e.EquipamientoNecesario
            }).ToList();

            return Ok(ejerciciosDTO);
        }

        // Obtener un ejercicio por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EjercicioDTO>> GetEjercicio(int id)
        {
            var ejercicio = await _service.GetByIdAsync(id);
            if (ejercicio == null)
                return NotFound();

            var ejercicioDTO = new EjercicioDTO
            {
                EjercicioID = ejercicio.EjercicioID,
                Nombre = ejercicio.Nombre,
                Descripcion = ejercicio.Descripcion,
                GrupoMuscular = ejercicio.GrupoMuscular,
                ImagenURL = ejercicio.ImagenURL,
                EquipamientoNecesario = ejercicio.EquipamientoNecesario
            };

            return Ok(ejercicioDTO);
        }

        // Crear un nuevo ejercicio
        [HttpPost]
        public async Task<ActionResult<EjercicioDTO>> CreateEjercicio([FromBody] EjercicioCreateDTO dto)
        {
            var ejercicio = new Ejercicio
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                GrupoMuscular = dto.GrupoMuscular,
                ImagenURL = dto.ImagenURL,
                EquipamientoNecesario = dto.EquipamientoNecesario
            };

            await _service.AddAsync(ejercicio);

            var ejercicioDTO = new EjercicioDTO
            {
                EjercicioID = ejercicio.EjercicioID,
                Nombre = ejercicio.Nombre,
                Descripcion = ejercicio.Descripcion,
                GrupoMuscular = ejercicio.GrupoMuscular,
                ImagenURL = ejercicio.ImagenURL,
                EquipamientoNecesario = ejercicio.EquipamientoNecesario
            };

            return CreatedAtAction(nameof(GetEjercicio), new { id = ejercicio.EjercicioID }, ejercicioDTO);
        }

        // Actualizar un ejercicio
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEjercicio(int id, [FromBody] EjercicioUpdateDTO dto)
        {
            var existingEjercicio = await _service.GetByIdAsync(id);
            if (existingEjercicio == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                existingEjercicio.Nombre = dto.Nombre;

            if (!string.IsNullOrWhiteSpace(dto.Descripcion))
                existingEjercicio.Descripcion = dto.Descripcion;

            if (!string.IsNullOrWhiteSpace(dto.GrupoMuscular))
                existingEjercicio.GrupoMuscular = dto.GrupoMuscular;

            if (!string.IsNullOrWhiteSpace(dto.ImagenURL))
                existingEjercicio.ImagenURL = dto.ImagenURL;

            if (dto.EquipamientoNecesario.HasValue)
                existingEjercicio.EquipamientoNecesario = dto.EquipamientoNecesario.Value;

            await _service.UpdateAsync(existingEjercicio);
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
