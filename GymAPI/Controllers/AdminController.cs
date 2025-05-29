// ===========================================
// 4. GymAPI/Controllers/AdminController.cs
// ===========================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;
using GymAPI.Repositories;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEjercicioRepository _ejercicioRepository;
        private readonly IEntrenamientoRepository _entrenamientoRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IUsuarioService usuarioService,
            IEjercicioRepository ejercicioRepository,
            IEntrenamientoRepository entrenamientoRepository,
            ILogger<AdminController> logger)
        {
            _usuarioService = usuarioService;
            _ejercicioRepository = ejercicioRepository;
            _entrenamientoRepository = entrenamientoRepository;
            _logger = logger;
        }

        // Verificar si el usuario actual es administrador
        private async Task<bool> IsCurrentUserAdmin()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return false;

            var user = await _usuarioService.GetByIdAsync(userId);
            return user?.EsAdmin == true;
        }

        // ============ GESTIÓN DE USUARIOS ============

        [HttpGet("usuarios")]
        public async Task<ActionResult<ApiResponse<List<AdminUsuarioDTO>>>> GetAllUsers()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                var usuarios = await _usuarioService.GetAllAsync();
                var usuariosDTO = usuarios.Select(u => new AdminUsuarioDTO
                {
                    UsuarioID = u.UsuarioID,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro,
                    EstaActivo = u.EstaActivo,
                    FotoPerfilURL = u.FotoPerfilURL,
                    EsAdmin = u.EsAdmin,
                    Edad = u.Edad,
                    Peso = u.Peso,
                    Altura = u.Altura
                }).ToList();

                return Ok(new ApiResponse<List<AdminUsuarioDTO>>
                {
                    Success = true,
                    Data = usuariosDTO
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, new ApiResponse<List<AdminUsuarioDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpPost("usuarios")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> CreateUser(AdminCreateUserDTO userDTO)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                _logger.LogInformation("🔥 Creando usuario: {Email}", userDTO.Email);

                var usuario = new Usuario
                {
                    Nombre = userDTO.Nombre,
                    Apellido = userDTO.Apellido,
                    Email = userDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                    EsAdmin = userDTO.EsAdmin,
                    EstaActivo = userDTO.EstaActivo
                };

                await _usuarioService.AddAsync(usuario);

                var usuarioResponse = new UsuarioDTO
                {
                    UsuarioID = usuario.UsuarioID,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    EstaActivo = usuario.EstaActivo,
                    EsAdmin = usuario.EsAdmin
                };

                _logger.LogInformation("✅ Usuario creado exitosamente: {UsuarioID}", usuario.UsuarioID);

                return Ok(new ApiResponse<UsuarioDTO>
                {
                    Success = true,
                    Data = usuarioResponse,
                    Message = "Usuario creado correctamente"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("❌ Error de validación al crear usuario: {Message}", ex.Message);
                return BadRequest(new ApiResponse<UsuarioDTO>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error inesperado al crear usuario");
                return StatusCode(500, new ApiResponse<UsuarioDTO>
                {
                    Success = false,
                    Message = $"Error al crear usuario: {ex.Message}"
                });
            }
        }

        [HttpPut("usuarios/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateUser(int id, AdminCreateUserDTO userDTO)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                _logger.LogInformation("🔥 Actualizando usuario ID: {UsuarioID}", id);

                var usuario = await _usuarioService.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    });
                }

                usuario.Nombre = userDTO.Nombre;
                usuario.Apellido = userDTO.Apellido;
                usuario.Email = userDTO.Email;
                usuario.EsAdmin = userDTO.EsAdmin;
                usuario.EstaActivo = userDTO.EstaActivo;

                if (!string.IsNullOrEmpty(userDTO.Password))
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                }

                await _usuarioService.UpdateAsync(usuario);

                _logger.LogInformation("✅ Usuario actualizado exitosamente: {UsuarioID}", id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Usuario actualizado correctamente"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("❌ Error de validación al actualizar usuario {UsuarioID}: {Message}", id, ex.Message);
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error inesperado al actualizar usuario {UsuarioID}", id);
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al actualizar usuario: {ex.Message}"
                });
            }
        }

        [HttpDelete("usuarios/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                _logger.LogInformation("🔥 INICIANDO eliminación de usuario ID: {UsuarioID}", id);

                // Verificar que no se está eliminando a sí mismo
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int currentUserId) && currentUserId == id)
                {
                    _logger.LogWarning("❌ Usuario {CurrentUserId} intentó eliminarse a sí mismo", currentUserId);
                    return BadRequest(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "No puedes eliminar tu propia cuenta"
                    });
                }

                // ELIMINAR USUARIO - Ahora con manejo de foreign keys
                await _usuarioService.DeleteAsync(id);

                _logger.LogInformation("✅ Usuario eliminado exitosamente: {UsuarioID}", id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Usuario eliminado correctamente"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("❌ Error de validación al eliminar usuario {UsuarioID}: {Message}", id, ex.Message);
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 ERROR CRITICO al eliminar usuario {UsuarioID}: {Message}", id, ex.Message);
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar usuario: {ex.Message}"
                });
            }
        }

        // ============ GESTIÓN DE EJERCICIOS ============

        [HttpGet("ejercicios")]
        public async Task<ActionResult<ApiResponse<List<EjercicioDTO>>>> GetAllExercises()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                var ejercicios = await _ejercicioRepository.GetAllAsync();
                var ejerciciosDTO = ejercicios.Select(e => new EjercicioDTO
                {
                    EjercicioID = e.EjercicioID,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    GrupoMuscular = e.GrupoMuscular,
                    ImagenURL = e.ImagenURL,
                    VideoURL = e.VideoURL,
                    EquipamientoNecesario = e.EquipamientoNecesario
                }).ToList();

                return Ok(new ApiResponse<List<EjercicioDTO>>
                {
                    Success = true,
                    Data = ejerciciosDTO
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ejercicios");
                return StatusCode(500, new ApiResponse<List<EjercicioDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpPost("ejercicios")]
        public async Task<ActionResult<ApiResponse<EjercicioDTO>>> CreateExercise(EjercicioCreateDTO ejercicioDTO)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                var ejercicio = new Ejercicio
                {
                    Nombre = ejercicioDTO.Nombre,
                    Descripcion = ejercicioDTO.Descripcion,
                    GrupoMuscular = ejercicioDTO.GrupoMuscular,
                    ImagenURL = ejercicioDTO.ImagenURL,
                    VideoURL = ejercicioDTO.VideoURL,
                    EquipamientoNecesario = ejercicioDTO.EquipamientoNecesario
                };

                await _ejercicioRepository.AddAsync(ejercicio);

                var ejercicioResponse = new EjercicioDTO
                {
                    EjercicioID = ejercicio.EjercicioID,
                    Nombre = ejercicio.Nombre,
                    Descripcion = ejercicio.Descripcion,
                    GrupoMuscular = ejercicio.GrupoMuscular,
                    ImagenURL = ejercicio.ImagenURL,
                    VideoURL = ejercicio.VideoURL,
                    EquipamientoNecesario = ejercicio.EquipamientoNecesario
                };

                return Ok(new ApiResponse<EjercicioDTO>
                {
                    Success = true,
                    Data = ejercicioResponse,
                    Message = "Ejercicio creado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ejercicio");
                return StatusCode(500, new ApiResponse<EjercicioDTO>
                {
                    Success = false,
                    Message = $"Error al crear ejercicio: {ex.Message}"
                });
            }
        }

        [HttpPut("ejercicios/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateExercise(int id, EjercicioUpdateDTO ejercicioDTO)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                var ejercicio = await _ejercicioRepository.GetByIdAsync(id);
                if (ejercicio == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Ejercicio no encontrado"
                    });
                }

                if (!string.IsNullOrEmpty(ejercicioDTO.Nombre))
                    ejercicio.Nombre = ejercicioDTO.Nombre;
                if (!string.IsNullOrEmpty(ejercicioDTO.Descripcion))
                    ejercicio.Descripcion = ejercicioDTO.Descripcion;
                if (!string.IsNullOrEmpty(ejercicioDTO.GrupoMuscular))
                    ejercicio.GrupoMuscular = ejercicioDTO.GrupoMuscular;
                if (!string.IsNullOrEmpty(ejercicioDTO.ImagenURL))
                    ejercicio.ImagenURL = ejercicioDTO.ImagenURL;
                if (!string.IsNullOrEmpty(ejercicioDTO.VideoURL))
                    ejercicio.VideoURL = ejercicioDTO.VideoURL;
                if (ejercicioDTO.EquipamientoNecesario.HasValue)
                    ejercicio.EquipamientoNecesario = ejercicioDTO.EquipamientoNecesario.Value;

                await _ejercicioRepository.UpdateAsync(ejercicio);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Ejercicio actualizado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar ejercicio {EjercicioID}", id);
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al actualizar ejercicio: {ex.Message}"
                });
            }
        }

        [HttpDelete("ejercicios/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteExercise(int id)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                await _ejercicioRepository.DeleteAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Ejercicio eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar ejercicio {EjercicioID}", id);
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar ejercicio: {ex.Message}"
                });
            }
        }

        // ============ GESTIÓN DE ENTRENAMIENTOS ============

        [HttpGet("entrenamientos")]
        public async Task<ActionResult<ApiResponse<List<EntrenamientoDTO>>>> GetAllWorkouts()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                var entrenamientos = await _entrenamientoRepository.GetAllAsync();
                var entrenamientosDTO = entrenamientos.Select(e => new EntrenamientoDTO
                {
                    EntrenamientoID = e.EntrenamientoID,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    DuracionMinutos = e.DuracionMinutos,
                    Dificultad = e.Dificultad,
                    ImagenURL = e.ImagenURL,
                    FechaCreacion = e.FechaCreacion,
                    Publico = e.Publico,
                    AutorID = e.AutorID
                }).ToList();

                return Ok(new ApiResponse<List<EntrenamientoDTO>>
                {
                    Success = true,
                    Data = entrenamientosDTO
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener entrenamientos");
                return StatusCode(500, new ApiResponse<List<EntrenamientoDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpDelete("entrenamientos/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteWorkout(int id)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                await _entrenamientoRepository.DeleteAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Entrenamiento eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar entrenamiento {EntrenamientoID}", id);
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar entrenamiento: {ex.Message}"
                });
            }
        }

        // ============ DASHBOARD INFO MEJORADO ============

        [HttpGet("dashboard")]
        public async Task<ActionResult<ApiResponse<object>>> GetDashboardInfo()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                // Usar los métodos optimizados del servicio
                var dashboardData = new
                {
                    TotalUsuarios = await _usuarioService.GetTotalUsersCountAsync(),
                    UsuariosActivos = await _usuarioService.GetActiveUsersCountAsync(),
                    TotalAdministradores = await _usuarioService.GetAdminUsersCountAsync(),
                    UsuariosRegistradosHoy = await _usuarioService.GetUsersRegisteredTodayAsync(),
                    UsuariosRegistradosEsteMes = await _usuarioService.GetUsersRegisteredThisMonthAsync(),
                    TotalEjercicios = (await _ejercicioRepository.GetAllAsync()).Count,
                    TotalEntrenamientos = (await _entrenamientoRepository.GetAllAsync()).Count,
                    EntrenamientosPublicos = (await _entrenamientoRepository.GetAllAsync()).Count(e => e.Publico)
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Data = dashboardData
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos del dashboard");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }
    }
}