using Microsoft.AspNetCore.Mvc;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Controllers
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
        public async Task<ActionResult<List<UsuarioDTO>>> GetUsuarios()
        {
            var usuarios = await _service.GetAllAsync();
            var usuariosDTO = usuarios.Select(u => new UsuarioDTO
            {
                UsuarioID = u.UsuarioID,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                FechaRegistro = u.FechaRegistro,
                EstaActivo = u.EstaActivo
            }).ToList();

            return Ok(usuariosDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            var usuarioDTO = new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                EstaActivo = usuario.EstaActivo
            };

            return Ok(usuarioDTO);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CreateUsuario(UsuarioCreateDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Email = usuarioDTO.Email,
                Password = usuarioDTO.Password
            };

            await _service.AddAsync(usuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioID }, new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                EstaActivo = usuario.EstaActivo
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, UsuarioUpdateDTO usuarioDTO)
        {
            var existingUsuario = await _service.GetByIdAsync(id);
            if (existingUsuario == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Nombre))
                existingUsuario.Nombre = usuarioDTO.Nombre;

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Apellido))
                existingUsuario.Apellido = usuarioDTO.Apellido;

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Email))
                existingUsuario.Email = usuarioDTO.Email;

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Password))
                existingUsuario.Password = usuarioDTO.Password;

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
