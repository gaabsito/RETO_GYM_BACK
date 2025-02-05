using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
        {
            return Ok(await _categoriaService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Categoria categoria)
        {
            await _categoriaService.AddAsync(categoria);
            return CreatedAtAction(nameof(GetById), new { id = categoria.CategoriaID }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaID) return BadRequest();
            await _categoriaService.UpdateAsync(categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _categoriaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
