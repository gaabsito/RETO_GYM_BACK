using GymAPI.DTOs;
using GymAPI.Models;
using GymAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService _service;

        public ComentarioController(IComentarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> GetComentarios()
        {
            var comentarios = await _service.GetAllAsync();
            var comentariosDTO = comentarios.Select(c => new ComentarioDTO
            {
                ComentarioID = c.ComentarioID,
                EntrenamientoID = c.EntrenamientoID,
                UsuarioID = c.UsuarioID,
                Contenido = c.Contenido,
                Calificacion = c.Calificacion,
                FechaComentario = c.FechaComentario
            }).ToList();

            return Ok(comentariosDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComentarioDTO>> GetComentario(int id)
        {
            var comentario = await _service.GetByIdAsync(id);
            if (comentario == null)
                return NotFound();

            var comentarioDTO = new ComentarioDTO
            {
                ComentarioID = comentario.ComentarioID,
                EntrenamientoID = comentario.EntrenamientoID,
                UsuarioID = comentario.UsuarioID,
                Contenido = comentario.Contenido,
                Calificacion = comentario.Calificacion,
                FechaComentario = comentario.FechaComentario
            };

            return Ok(comentarioDTO);
        }

        [HttpGet("entrenamiento/{entrenamientoId}")]
        public async Task<ActionResult<List<ComentarioDTO>>> GetComentariosPorEntrenamiento(int entrenamientoId)
        {
            var comentarios = await _service.GetByEntrenamientoIdAsync(entrenamientoId);
            var comentariosDTO = comentarios.Select(c => new ComentarioDTO
            {
                ComentarioID = c.ComentarioID,
                EntrenamientoID = c.EntrenamientoID,
                UsuarioID = c.UsuarioID,
                Contenido = c.Contenido,
                Calificacion = c.Calificacion,
                FechaComentario = c.FechaComentario
            }).ToList();

            return Ok(comentariosDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ComentarioDTO>> CreateComentario([FromBody] ComentarioCreateDTO dto)
        {
            var comentario = new Comentario
            {
                EntrenamientoID = dto.EntrenamientoID,
                UsuarioID = dto.UsuarioID,
                Contenido = dto.Contenido,
                Calificacion = dto.Calificacion,
                FechaComentario = DateTime.UtcNow
            };

            await _service.AddAsync(comentario);

            var comentarioDTO = new ComentarioDTO
            {
                ComentarioID = comentario.ComentarioID,
                EntrenamientoID = comentario.EntrenamientoID,
                UsuarioID = comentario.UsuarioID,
                Contenido = comentario.Contenido,
                Calificacion = comentario.Calificacion,
                FechaComentario = comentario.FechaComentario
            };

            return CreatedAtAction(nameof(GetComentario), new { id = comentario.ComentarioID }, comentarioDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComentario(int id, [FromBody] ComentarioUpdateDTO dto)
        {
            var existingComentario = await _service.GetByIdAsync(id);
            if (existingComentario == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Contenido))
                existingComentario.Contenido = dto.Contenido;

            if (dto.Calificacion.HasValue)
                existingComentario.Calificacion = dto.Calificacion.Value;

            await _service.UpdateAsync(existingComentario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _service.GetByIdAsync(id);
            if (comentario == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}