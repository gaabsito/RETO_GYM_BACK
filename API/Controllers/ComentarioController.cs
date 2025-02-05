using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentarioController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetAll()
        {
            return Ok(await _comentarioService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetById(int id)
        {
            var comentario = await _comentarioService.GetByIdAsync(id);
            if (comentario == null) return NotFound();
            return Ok(comentario);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Comentario comentario)
        {
            await _comentarioService.AddAsync(comentario);
            return CreatedAtAction(nameof(GetById), new { id = comentario.ComentarioID }, comentario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Comentario comentario)
        {
            if (id != comentario.ComentarioID) return BadRequest();
            await _comentarioService.UpdateAsync(comentario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _comentarioService.DeleteAsync(id);
            return NoContent();
        }
    }
}
