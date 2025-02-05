using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadMedidaController : ControllerBase
    {
        private readonly IUnidadMedidaService _unidadMedidaService;

        public UnidadMedidaController(IUnidadMedidaService unidadMedidaService)
        {
            _unidadMedidaService = unidadMedidaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadMedida>>> GetAll()
        {
            return Ok(await _unidadMedidaService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UnidadMedida>> GetById(int id)
        {
            var unidad = await _unidadMedidaService.GetByIdAsync(id);
            if (unidad == null) return NotFound();
            return Ok(unidad);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UnidadMedida unidad)
        {
            await _unidadMedidaService.AddAsync(unidad);
            return CreatedAtAction(nameof(GetById), new { id = unidad.UnidadID }, unidad);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UnidadMedida unidad)
        {
            if (id != unidad.UnidadID) return BadRequest();
            await _unidadMedidaService.UpdateAsync(unidad);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _unidadMedidaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
