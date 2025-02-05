using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtiquetaController : ControllerBase
    {
        private readonly IEtiquetaService _etiquetaService;

        public EtiquetaController(IEtiquetaService etiquetaService)
        {
            _etiquetaService = etiquetaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Etiqueta>>> GetAll()
        {
            return Ok(await _etiquetaService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Etiqueta>> GetById(int id)
        {
            var etiqueta = await _etiquetaService.GetByIdAsync(id);
            if (etiqueta == null) return NotFound();
            return Ok(etiqueta);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Etiqueta etiqueta)
        {
            await _etiquetaService.AddAsync(etiqueta);
            return CreatedAtAction(nameof(GetById), new { id = etiqueta.EtiquetaID }, etiqueta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Etiqueta etiqueta)
        {
            if (id != etiqueta.EtiquetaID) return BadRequest();
            await _etiquetaService.UpdateAsync(etiqueta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _etiquetaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
