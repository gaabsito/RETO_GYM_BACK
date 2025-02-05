using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaEtiquetaController : ControllerBase
    {
        private readonly IRecetaEtiquetasService _recetaEtiquetasService;

        public RecetaEtiquetaController(IRecetaEtiquetasService recetaEtiquetasService)
        {
            _recetaEtiquetasService = recetaEtiquetasService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecetaEtiquetas>>> GetAll()
        {
            return Ok(await _recetaEtiquetasService.GetAllAsync());
        }

        [HttpGet("{recetaId}/{etiquetaId}")]
        public async Task<ActionResult<RecetaEtiquetas>> GetById(int recetaId, int etiquetaId)
        {
            var relacion = await _recetaEtiquetasService.GetByIdAsync(recetaId, etiquetaId);
            if (relacion == null) return NotFound();
            return Ok(relacion);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RecetaEtiquetas relacion)
        {
            await _recetaEtiquetasService.AddAsync(relacion);
            return CreatedAtAction(nameof(GetById), new { recetaId = relacion.RecetaID, etiquetaId = relacion.EtiquetaID }, relacion);
        }

        [HttpDelete("{recetaId}/{etiquetaId}")]
        public async Task<ActionResult> Delete(int recetaId, int etiquetaId)
        {
            await _recetaEtiquetasService.DeleteAsync(recetaId, etiquetaId);
            return NoContent();
        }
    }
}
