using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            return Ok(await _usuarioService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Usuario usuario)
        {
            await _usuarioService.AddAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.UsuarioID }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.UsuarioID) return BadRequest();
            await _usuarioService.UpdateAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }
    }
}
