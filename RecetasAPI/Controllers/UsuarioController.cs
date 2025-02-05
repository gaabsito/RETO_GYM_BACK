using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var usuarios = await _service.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            await _service.AddAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioID }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario updatedUsuario)
        {
            var existingUsuario = await _service.GetByIdAsync(id);
            if (existingUsuario == null)
                return NotFound();

            existingUsuario.Nombre = updatedUsuario.Nombre;
            existingUsuario.Apellido = updatedUsuario.Apellido;
            existingUsuario.Email = updatedUsuario.Email;
            existingUsuario.Password = updatedUsuario.Password;

            await _service.UpdateAsync(existingUsuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}