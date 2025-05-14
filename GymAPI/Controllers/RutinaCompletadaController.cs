using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RutinaCompletadaController : ControllerBase
    {
        private readonly IRutinaCompletadaService _rutinaCompletadaService;
        private readonly IEntrenamientoService _entrenamientoService;

        public RutinaCompletadaController(IRutinaCompletadaService rutinaCompletadaService, IEntrenamientoService entrenamientoService)
        {
            _rutinaCompletadaService = rutinaCompletadaService;
            _entrenamientoService = entrenamientoService;
        }

        // GET: api/RutinaCompletada
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<RutinaCompletadaDTO>>>> GetRutinasCompletadas()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<List<RutinaCompletadaDTO>>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var rutinasCompletadas = await _rutinaCompletadaService.GetByUsuarioIdAsync(userId);
                var rutinasCompletadasDTO = rutinasCompletadas.Select(rc => new RutinaCompletadaDTO
                {
                    RutinaCompletadaID = rc.RutinaCompletadaID,
                    UsuarioID = rc.UsuarioID,
                    EntrenamientoID = rc.EntrenamientoID,
                    FechaCompletada = rc.FechaCompletada,
                    Notas = rc.Notas,
                    DuracionMinutos = rc.DuracionMinutos,
                    CaloriasEstimadas = rc.CaloriasEstimadas,
                    NivelEsfuerzoPercibido = rc.NivelEsfuerzoPercibido,
                    NombreEntrenamiento = rc.Entrenamiento?.Titulo,
                    DificultadEntrenamiento = rc.Entrenamiento?.Dificultad
                }).ToList();

                return Ok(new ApiResponse<List<RutinaCompletadaDTO>>
                {
                    Success = true,
                    Data = rutinasCompletadasDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<RutinaCompletadaDTO>>
                {
                    Success = false,
                    Message = "Error al obtener rutinas completadas: " + ex.Message
                });
            }
        }

        // GET: api/RutinaCompletada/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RutinaCompletadaDTO>>> GetRutinaCompletada(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<RutinaCompletadaDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var rutinaCompletada = await _rutinaCompletadaService.GetByIdAsync(id);
                if (rutinaCompletada == null)
                {
                    return NotFound(new ApiResponse<RutinaCompletadaDTO>
                    {
                        Success = false,
                        Message = "Rutina completada no encontrada"
                    });
                }

                // Verificar que la rutina pertenezca al usuario actual
                if (rutinaCompletada.UsuarioID != userId)
                {
                    return Forbid();
                }

                // Obtener información del entrenamiento
                var entrenamiento = await _entrenamientoService.GetByIdAsync(rutinaCompletada.EntrenamientoID);

                var rutinaCompletadaDTO = new RutinaCompletadaDTO
                {
                    RutinaCompletadaID = rutinaCompletada.RutinaCompletadaID,
                    UsuarioID = rutinaCompletada.UsuarioID,
                    EntrenamientoID = rutinaCompletada.EntrenamientoID,
                    FechaCompletada = rutinaCompletada.FechaCompletada,
                    Notas = rutinaCompletada.Notas,
                    DuracionMinutos = rutinaCompletada.DuracionMinutos,
                    CaloriasEstimadas = rutinaCompletada.CaloriasEstimadas,
                    NivelEsfuerzoPercibido = rutinaCompletada.NivelEsfuerzoPercibido,
                    NombreEntrenamiento = entrenamiento?.Titulo,
                    DificultadEntrenamiento = entrenamiento?.Dificultad
                };

                return Ok(new ApiResponse<RutinaCompletadaDTO>
                {
                    Success = true,
                    Data = rutinaCompletadaDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RutinaCompletadaDTO>
                {
                    Success = false,
                    Message = "Error al obtener rutina completada: " + ex.Message
                });
            }
        }

        // GET: api/RutinaCompletada/Entrenamiento/5
        [HttpGet("Entrenamiento/{entrenamientoId}")]
        public async Task<ActionResult<ApiResponse<List<RutinaCompletadaDTO>>>> GetRutinasCompletadasPorEntrenamiento(int entrenamientoId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<List<RutinaCompletadaDTO>>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var rutinasCompletadas = await _rutinaCompletadaService.GetByUsuarioIdAndEntrenamientoIdAsync(userId, entrenamientoId);
                var entrenamiento = await _entrenamientoService.GetByIdAsync(entrenamientoId);

                var rutinasCompletadasDTO = rutinasCompletadas.Select(rc => new RutinaCompletadaDTO
                {
                    RutinaCompletadaID = rc.RutinaCompletadaID,
                    UsuarioID = rc.UsuarioID,
                    EntrenamientoID = rc.EntrenamientoID,
                    FechaCompletada = rc.FechaCompletada,
                    Notas = rc.Notas,
                    DuracionMinutos = rc.DuracionMinutos,
                    CaloriasEstimadas = rc.CaloriasEstimadas,
                    NivelEsfuerzoPercibido = rc.NivelEsfuerzoPercibido,
                    NombreEntrenamiento = entrenamiento?.Titulo,
                    DificultadEntrenamiento = entrenamiento?.Dificultad
                }).ToList();

                return Ok(new ApiResponse<List<RutinaCompletadaDTO>>
                {
                    Success = true,
                    Data = rutinasCompletadasDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<RutinaCompletadaDTO>>
                {
                    Success = false,
                    Message = "Error al obtener rutinas completadas: " + ex.Message
                });
            }
        }

        // GET: api/RutinaCompletada/Resumen
        [HttpGet("Resumen")]
        public async Task<ActionResult<ApiResponse<ResumenRutinasDTO>>> GetResumen()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<ResumenRutinasDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                // Obtener datos para el resumen
                var totalRutinas = await _rutinaCompletadaService.GetTotalCountAsync(userId);
                var rutinasUltimaSemana = await _rutinaCompletadaService.GetCountLastWeekAsync(userId);
                var rutinasUltimoMes = await _rutinaCompletadaService.GetCountLastMonthAsync(userId);
                
                // Rutina más completada
                var rutinaFavorita = await _rutinaCompletadaService.GetMostCompletedWorkoutAsync(userId);
                
                // Obtener todas las rutinas completadas para calcular promedios
                var todasLasRutinas = await _rutinaCompletadaService.GetByUsuarioIdAsync(userId);
                
                // Calcular estadísticas
                var promedioEsfuerzo = todasLasRutinas
                    .Where(r => r.NivelEsfuerzoPercibido.HasValue)
                    .Select(r => r.NivelEsfuerzoPercibido.Value)
                    .DefaultIfEmpty(0)
                    .Average();
                
                var caloriasTotales = todasLasRutinas
                    .Where(r => r.CaloriasEstimadas.HasValue)
                    .Sum(r => r.CaloriasEstimadas.Value);
                
                var minutosTotales = todasLasRutinas
                    .Where(r => r.DuracionMinutos.HasValue)
                    .Sum(r => r.DuracionMinutos.Value);

                // Crear DTO de resumen
                var resumen = new ResumenRutinasDTO
                {
                    TotalRutinasCompletadas = totalRutinas,
                    RutinasUltimaSemana = rutinasUltimaSemana,
                    RutinasUltimoMes = rutinasUltimoMes,
                    PromedioEsfuerzo = Math.Round(promedioEsfuerzo, 1),
                    CaloriasTotales = caloriasTotales,
                    MinutosTotales = minutosTotales,
                    EntrenamientoIDMasRepetido = rutinaFavorita.EntrenamientoID > 0 ? rutinaFavorita.EntrenamientoID : null,
                    NombreEntrenamientoMasRepetido = !string.IsNullOrEmpty(rutinaFavorita.Nombre) ? rutinaFavorita.Nombre : null,
                    VecesCompletado = rutinaFavorita.Veces > 0 ? rutinaFavorita.Veces : null
                };

                return Ok(new ApiResponse<ResumenRutinasDTO>
                {
                    Success = true,
                    Data = resumen
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ResumenRutinasDTO>
                {
                    Success = false,
                    Message = "Error al obtener resumen de rutinas: " + ex.Message
                });
            }
        }

        // POST: api/RutinaCompletada
        [HttpPost]
        public async Task<ActionResult<ApiResponse<RutinaCompletadaDTO>>> CreateRutinaCompletada([FromBody] RutinaCompletadaCreateDTO rutinaCompletadaDTO)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<RutinaCompletadaDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                // Verificar que el entrenamiento existe
                var entrenamiento = await _entrenamientoService.GetByIdAsync(rutinaCompletadaDTO.EntrenamientoID);
                if (entrenamiento == null)
                {
                    return BadRequest(new ApiResponse<RutinaCompletadaDTO>
                    {
                        Success = false,
                        Message = "El entrenamiento no existe"
                    });
                }

                // Crear la entidad RutinaCompletada
                var rutinaCompletada = new RutinaCompletada
                {
                    UsuarioID = userId,
                    EntrenamientoID = rutinaCompletadaDTO.EntrenamientoID,
                    FechaCompletada = rutinaCompletadaDTO.FechaCompletada ?? DateTime.Now,
                    Notas = rutinaCompletadaDTO.Notas,
                    DuracionMinutos = rutinaCompletadaDTO.DuracionMinutos,
                    CaloriasEstimadas = rutinaCompletadaDTO.CaloriasEstimadas,
                    NivelEsfuerzoPercibido = rutinaCompletadaDTO.NivelEsfuerzoPercibido
                };

                // Guardar en la base de datos
                var id = await _rutinaCompletadaService.AddAsync(rutinaCompletada);

                // Crear DTO para la respuesta
                var responseDTO = new RutinaCompletadaDTO
                {
                    RutinaCompletadaID = rutinaCompletada.RutinaCompletadaID,
                    UsuarioID = rutinaCompletada.UsuarioID,
                    EntrenamientoID = rutinaCompletada.EntrenamientoID,
                    FechaCompletada = rutinaCompletada.FechaCompletada,
                    Notas = rutinaCompletada.Notas,
                    DuracionMinutos = rutinaCompletada.DuracionMinutos,
                    CaloriasEstimadas = rutinaCompletada.CaloriasEstimadas,
                    NivelEsfuerzoPercibido = rutinaCompletada.NivelEsfuerzoPercibido,
                    NombreEntrenamiento = entrenamiento.Titulo,
                    DificultadEntrenamiento = entrenamiento.Dificultad
                };

                return Ok(new ApiResponse<RutinaCompletadaDTO>
                {
                    Success = true,
                    Data = responseDTO,
                    Message = "Rutina completada registrada con éxito"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RutinaCompletadaDTO>
                {
                    Success = false,
                    Message = "Error al registrar rutina completada: " + ex.Message
                });
            }
        }

        // PUT: api/RutinaCompletada/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<RutinaCompletadaDTO>>> UpdateRutinaCompletada(int id, [FromBody] RutinaCompletadaUpdateDTO rutinaCompletadaDTO)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<RutinaCompletadaDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                // Obtener la rutina completada existente
                var rutinaCompletada = await _rutinaCompletadaService.GetByIdAsync(id);
                if (rutinaCompletada == null)
                {
                    return NotFound(new ApiResponse<RutinaCompletadaDTO>
                    {
                        Success = false,
                        Message = "Rutina completada no encontrada"
                    });
                }

                // Verificar que pertenece al usuario actual
                if (rutinaCompletada.UsuarioID != userId)
                {
                    return Forbid();
                }

                // Actualizar solo los campos proporcionados
                if (rutinaCompletadaDTO.FechaCompletada.HasValue)
                    rutinaCompletada.FechaCompletada = rutinaCompletadaDTO.FechaCompletada.Value;
                
                if (rutinaCompletadaDTO.Notas != null)
                    rutinaCompletada.Notas = rutinaCompletadaDTO.Notas;
                
                if (rutinaCompletadaDTO.DuracionMinutos.HasValue)
                    rutinaCompletada.DuracionMinutos = rutinaCompletadaDTO.DuracionMinutos;
                
                if (rutinaCompletadaDTO.CaloriasEstimadas.HasValue)
                    rutinaCompletada.CaloriasEstimadas = rutinaCompletadaDTO.CaloriasEstimadas;
                
                if (rutinaCompletadaDTO.NivelEsfuerzoPercibido.HasValue)
                    rutinaCompletada.NivelEsfuerzoPercibido = rutinaCompletadaDTO.NivelEsfuerzoPercibido;

                // Guardar los cambios
                await _rutinaCompletadaService.UpdateAsync(rutinaCompletada);

                // Obtener información del entrenamiento para la respuesta
                var entrenamiento = await _entrenamientoService.GetByIdAsync(rutinaCompletada.EntrenamientoID);

                // Crear DTO para la respuesta
                var responseDTO = new RutinaCompletadaDTO
                {
                    RutinaCompletadaID = rutinaCompletada.RutinaCompletadaID,
                    UsuarioID = rutinaCompletada.UsuarioID,
                    EntrenamientoID = rutinaCompletada.EntrenamientoID,
                    FechaCompletada = rutinaCompletada.FechaCompletada,
                    Notas = rutinaCompletada.Notas,
                    DuracionMinutos = rutinaCompletada.DuracionMinutos,
                    CaloriasEstimadas = rutinaCompletada.CaloriasEstimadas,
                    NivelEsfuerzoPercibido = rutinaCompletada.NivelEsfuerzoPercibido,
                    NombreEntrenamiento = entrenamiento?.Titulo,
                    DificultadEntrenamiento = entrenamiento?.Dificultad
                };

                return Ok(new ApiResponse<RutinaCompletadaDTO>
                {
                    Success = true,
                    Data = responseDTO,
                    Message = "Rutina completada actualizada con éxito"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<RutinaCompletadaDTO>
                {
                    Success = false,
                    Message = "Error al actualizar rutina completada: " + ex.Message
                });
            }
        }

        // DELETE: api/RutinaCompletada/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteRutinaCompletada(int id)
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

                // Obtener la rutina completada
                var rutinaCompletada = await _rutinaCompletadaService.GetByIdAsync(id);
                if (rutinaCompletada == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Rutina completada no encontrada"
                    });
                }

                // Verificar que pertenece al usuario actual
                if (rutinaCompletada.UsuarioID != userId)
                {
                    return Forbid();
                }

                // Eliminar la rutina completada
                await _rutinaCompletadaService.DeleteAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Rutina completada eliminada con éxito"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error al eliminar rutina completada: " + ex.Message
                });
            }
        }
    }
}