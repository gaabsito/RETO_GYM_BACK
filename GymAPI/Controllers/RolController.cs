using GymAPI.DTOs;
using GymAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<RolDTO>>>> GetAllRoles()
        {
            try
            {
                var roles = await _rolService.GetAllRolesAsync();
                var rolesDTO = roles.Select(r => new RolDTO
                {
                    RolID = r.RolID,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Icono = r.Icono,
                    Color = r.Color,
                    DiasPorSemanaMinimo = r.DiasPorSemanaMinimo,
                    DiasPorSemanaMaximo = r.DiasPorSemanaMaximo
                }).ToList();

                return Ok(new ApiResponse<List<RolDTO>>
                {
                    Success = true,
                    Data = rolesDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<RolDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpGet("usuario")]
        public async Task<ActionResult<ApiResponse<UsuarioRolDTO>>> GetUserRole()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<UsuarioRolDTO>
                    {
                        Success = false,
                        Message = "Token inválido"
                    });
                }

                var (rolActual, diasEntrenados, diasParaSiguiente, progreso) = 
                    await _rolService.GetUserRolInfoAsync(userId);

                if (rolActual == null)
                {
                    return NotFound(new ApiResponse<UsuarioRolDTO>
                    {
                        Success = false,
                        Message = "No se pudo determinar el rol del usuario"
                    });
                }

                // Obtener el número de la semana actual para mostrar
                int numeroSemanaActual = System.Globalization.ISOWeek.GetWeekOfYear(DateTime.Now);

                var usuarioRolDTO = new UsuarioRolDTO
                {
                    UsuarioID = userId,
                    RolID = rolActual.RolID,
                    NombreRol = rolActual.Nombre,
                    Color = rolActual.Color,
                    Icono = rolActual.Icono,
                    FechaAsignacion = DateTime.Now,
                    DiasEntrenadosSemanales = diasEntrenados,  // Días únicos entrenados esta semana
                    DiasParaSiguienteRol = diasParaSiguiente,
                    ProgresoSiguienteRol = progreso,
                    SemanaActual = numeroSemanaActual
                };

                return Ok(new ApiResponse<UsuarioRolDTO>
                {
                    Success = true,
                    Data = usuarioRolDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UsuarioRolDTO>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }
    }
}