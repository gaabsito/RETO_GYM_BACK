using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetasFavoritasController : ControllerBase
    {
        private readonly IRecetasFavoritasService _recetasFavoritasService;

        public RecetasFavoritasController(IRecetasFavoritasService recetasFavoritasService)
        {
            _recetasFavoritasService = recetasFavoritasService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecetasFavoritas>>> GetAll()
        {
            return Ok(await _recetasFavoritasService.GetAllAsync());
        }

        [HttpGet("{usuarioId}/{recetaId}")]
        public async Task<ActionResult<RecetasFavoritas>> GetById(int usuarioId, int recetaId)
        {
            var favorito = await _recetasFavoritasService.GetByIdAsync(usuarioId, recetaId);
            if (favorito == null) return NotFound();
            return Ok(favorito);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RecetasFavoritas favorito)
        {
            await _recetasFavoritasService.AddAsync(favorito);
            return CreatedAtAction(nameof(GetById), new { usuarioId = favorito.UsuarioID, recetaId = favorito.RecetaID }, favorito);
        }

        [HttpDelete("{usuarioId}/{recetaId}")]
        public async Task<ActionResult> Delete(int usuarioId, int recetaId)
        {
            await _recetasFavoritasService.DeleteAsync(usuarioId, recetaId);
            return NoContent();
        }
    }
}
