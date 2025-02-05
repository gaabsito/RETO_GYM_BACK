using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaController : ControllerBase
    {
        private readonly IRecetaService _recetaService;

        public RecetaController(IRecetaService recetaService)
        {
            _recetaService = recetaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receta>>> GetAll()
        {
            return Ok(await _recetaService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Receta>> GetById(int id)
        {
            var receta = await _recetaService.GetByIdAsync(id);
            if (receta == null) return NotFound();
            return Ok(receta);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Receta receta)
        {
            await _recetaService.AddAsync(receta);
            return CreatedAtAction(nameof(GetById), new { id = receta.RecetaID }, receta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Receta receta)
        {
            if (id != receta.RecetaID) return BadRequest();
            await _recetaService.UpdateAsync(receta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _recetaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
