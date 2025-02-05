using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredienteController : ControllerBase
    {
        private readonly IIngredienteService _ingredienteService;

        public IngredienteController(IIngredienteService ingredienteService)
        {
            _ingredienteService = ingredienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingrediente>>> GetAll()
        {
            return Ok(await _ingredienteService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingrediente>> GetById(int id)
        {
            var ingrediente = await _ingredienteService.GetByIdAsync(id);
            if (ingrediente == null) return NotFound();
            return Ok(ingrediente);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Ingrediente ingrediente)
        {
            await _ingredienteService.AddAsync(ingrediente);
            return CreatedAtAction(nameof(GetById), new { id = ingrediente.IngredienteID }, ingrediente);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Ingrediente ingrediente)
        {
            if (id != ingrediente.IngredienteID) return BadRequest();
            await _ingredienteService.UpdateAsync(ingrediente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _ingredienteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
