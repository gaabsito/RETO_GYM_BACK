<<<<<<< HEAD
=======
using GymAPI.DTOs;
>>>>>>> Ejercicio
using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Linq;
>>>>>>> Ejercicio
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
<<<<<<< HEAD
        public async Task<ActionResult<List<Ejercicio>>> GetEjercicios()
        {
            var ejercicios = await _service.GetAllAsync();
            return Ok(ejercicios);
=======
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
>>>>>>> Ejercicio
        }

        // Obtener un ejercicio por ID
        [HttpGet("{id}")]
<<<<<<< HEAD
        public async Task<ActionResult<Ejercicio>> GetEjercicio(int id)
=======
        public async Task<ActionResult<EjercicioDTO>> GetEjercicio(int id)
>>>>>>> Ejercicio
        {
            var ejercicio = await _service.GetByIdAsync(id);
            if (ejercicio == null)
                return NotFound();

<<<<<<< HEAD
            return Ok(ejercicio);
=======
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
>>>>>>> Ejercicio
        }

        // Crear un nuevo ejercicio
        [HttpPost]
<<<<<<< HEAD
        public async Task<ActionResult<Ejercicio>> CreateEjercicio([FromBody] Ejercicio ejercicio)
        {
            if (string.IsNullOrWhiteSpace(ejercicio.Nombre))
            {
                return BadRequest("El nombre del ejercicio es obligatorio.");
            }

            await _service.AddAsync(ejercicio);
            return CreatedAtAction(nameof(GetEjercicio), new { id = ejercicio.EjercicioID }, ejercicio);
=======
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
>>>>>>> Ejercicio
        }

        // Actualizar un ejercicio
        [HttpPut("{id}")]
<<<<<<< HEAD
        public async Task<IActionResult> UpdateEjercicio(int id, [FromBody] Ejercicio updatedEjercicio)
        {
            if (string.IsNullOrWhiteSpace(updatedEjercicio.Nombre))
            {
                return BadRequest("El nombre del ejercicio es obligatorio.");
            }

=======
        public async Task<IActionResult> UpdateEjercicio(int id, [FromBody] EjercicioUpdateDTO dto)
        {
>>>>>>> Ejercicio
            var existingEjercicio = await _service.GetByIdAsync(id);
            if (existingEjercicio == null)
                return NotFound();

<<<<<<< HEAD
            updatedEjercicio.EjercicioID = id; // Asegurar que el ID no cambie
            await _service.UpdateAsync(updatedEjercicio);
=======
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
>>>>>>> Ejercicio
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
