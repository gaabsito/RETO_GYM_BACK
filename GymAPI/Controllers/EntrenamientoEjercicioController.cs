using GymAPI.DTOs;
using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenamientoEjercicioController : ControllerBase
    {
        private readonly IEntrenamientoEjercicioService _service;

        public EntrenamientoEjercicioController(IEntrenamientoEjercicioService service)
        {
            _service = service;
        }

        // ✅ GET: Obtener ejercicios por entrenamiento
        [HttpGet("{entrenamientoID}")]
        public async Task<ActionResult<List<EntrenamientoEjercicioDTO>>> GetEjerciciosPorEntrenamiento(int entrenamientoID)
        {
            var ejercicios = await _service.GetByEntrenamientoAsync(entrenamientoID);
            var ejerciciosDTO = ejercicios.Select(e => new EntrenamientoEjercicioDTO
            {
                EntrenamientoID = e.EntrenamientoID,
                EjercicioID = e.EjercicioID,
                Series = e.Series,
                Repeticiones = e.Repeticiones,
                DescansoSegundos = e.DescansoSegundos,
                Notas = e.Notas
            }).ToList();

            return Ok(ejerciciosDTO);
        }

        // ✅ POST: Agregar un ejercicio a un entrenamiento
       [HttpPost]
public async Task<IActionResult> AddEjercicio([FromBody] EntrenamientoEjercicioCreateDTO dto)
{
    Console.WriteLine("📩 Recibiendo solicitud para asignar ejercicio...");
    
    if (dto == null)
    {
        Console.WriteLine("❌ Error: DTO recibido es nulo.");
        return BadRequest("El cuerpo de la solicitud es nulo.");
    }

    Console.WriteLine($"➡️ Datos recibidos - EntrenamientoID: {dto.EntrenamientoID}, EjercicioID: {dto.EjercicioID}");
    Console.WriteLine($"➡️ Series: {dto.Series}, Repeticiones: {dto.Repeticiones}, Descanso: {dto.DescansoSegundos}");
    Console.WriteLine($"➡️ Notas: {dto.Notas}");

    try
    {
        var entrenamientoEjercicio = new EntrenamientoEjercicio
        {
            EntrenamientoID = dto.EntrenamientoID,
            EjercicioID = dto.EjercicioID,
            Series = dto.Series,
            Repeticiones = dto.Repeticiones,
            DescansoSegundos = dto.DescansoSegundos,
            Notas = dto.Notas
        };

        Console.WriteLine("🔄 Guardando en la base de datos...");
        await _service.AddAsync(entrenamientoEjercicio);
        Console.WriteLine("✅ Ejercicio asignado correctamente.");

        return CreatedAtAction(nameof(GetEjerciciosPorEntrenamiento), new { entrenamientoID = entrenamientoEjercicio.EntrenamientoID }, entrenamientoEjercicio);
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error en la API: " + ex.Message);
        return StatusCode(500, "Error interno del servidor: " + ex.Message);
    }
}


        // ✅ PUT: Actualizar un ejercicio dentro de un entrenamiento
        [HttpPut("{entrenamientoID}/{ejercicioID}")]
        public async Task<IActionResult> UpdateEjercicio(int entrenamientoID, int ejercicioID, [FromBody] EntrenamientoEjercicioUpdateDTO dto)
        {
            var existingEjercicio = await _service.GetByIdAsync(entrenamientoID, ejercicioID);
            if (existingEjercicio == null)
                return NotFound();

            if (dto.Series.HasValue)
                existingEjercicio.Series = dto.Series.Value;
            if (dto.Repeticiones.HasValue)
                existingEjercicio.Repeticiones = dto.Repeticiones.Value;
            if (dto.DescansoSegundos.HasValue)
                existingEjercicio.DescansoSegundos = dto.DescansoSegundos.Value;
            if (!string.IsNullOrWhiteSpace(dto.Notas))
                existingEjercicio.Notas = dto.Notas;

            await _service.UpdateAsync(entrenamientoID, ejercicioID, existingEjercicio);
            return NoContent();
        }

        // ✅ DELETE: Eliminar un ejercicio de un entrenamiento
        [HttpDelete("{entrenamientoID}/{ejercicioID}")]
        public async Task<IActionResult> RemoveEjercicio(int entrenamientoID, int ejercicioID)
        {
            var existingEjercicio = await _service.GetByIdAsync(entrenamientoID, ejercicioID);
            if (existingEjercicio == null)
                return NotFound();

            await _service.RemoveAsync(entrenamientoID, ejercicioID);
            return NoContent();
        }
    }
}
