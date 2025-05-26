// GymAPI/Controllers/LogroController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GymAPI.Services;
using GymAPI.DTOs;
using GymAPI.Models;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogroController : ControllerBase
    {
        private readonly ILogroService _logroService;

        public LogroController(ILogroService logroService)
        {
            _logroService = logroService;
        }

        // GET: api/Logro
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UsuarioLogroDTO>>>> GetLogrosUsuario()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<List<UsuarioLogroDTO>>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var logros = await _logroService.GetUsuarioLogrosAsync(userId);

                return Ok(new ApiResponse<List<UsuarioLogroDTO>>
                {
                    Success = true,
                    Data = logros
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<UsuarioLogroDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        // GET: api/Logro/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<ApiResponse<List<LogroDTO>>>> GetLogrosDisponibles()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<List<LogroDTO>>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var logros = await _logroService.GetLogrosDisponiblesAsync(userId);

                return Ok(new ApiResponse<List<LogroDTO>>
                {
                    Success = true,
                    Data = logros
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<LogroDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        // GET: api/Logro/recientes
        [HttpGet("recientes")]
        public async Task<ActionResult<ApiResponse<List<UsuarioLogroDTO>>>> GetLogrosRecientes([FromQuery] int cantidad = 5)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<List<UsuarioLogroDTO>>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var logros = await _logroService.GetLogrosRecientesAsync(userId, cantidad);

                return Ok(new ApiResponse<List<UsuarioLogroDTO>>
                {
                    Success = true,
                    Data = logros
                });
            }
            catch (Exception ex)
            {
                // Registrar el error para depuración
                Console.WriteLine($"Error en GetLogrosRecientes: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                return StatusCode(500, new ApiResponse<List<UsuarioLogroDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        // GET: api/Logro/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UsuarioLogroDTO>>> GetLogro(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<UsuarioLogroDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var logro = await _logroService.GetUsuarioLogroByIdAsync(userId, id);
                if (logro == null)
                {
                    return NotFound(new ApiResponse<UsuarioLogroDTO>
                    {
                        Success = false,
                        Message = "Logro no encontrado"
                    });
                }

                return Ok(new ApiResponse<UsuarioLogroDTO>
                {
                    Success = true,
                    Data = logro
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UsuarioLogroDTO>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        // POST: api/Logro/verificar
        [HttpPost("verificar")]
        public async Task<ActionResult<ApiResponse<bool>>> VerificarLogros()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                await _logroService.VerificarLogrosAsync(userId);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Verificación de logros completada"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }
    }
}