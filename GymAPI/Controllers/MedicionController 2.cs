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
    public class MedicionController : ControllerBase
    {
        private readonly IMedicionService _service;

        public MedicionController(IMedicionService service)
        {
            _service = service;
        }

        // GET: api/Medicion
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<MedicionDTO>>>> GetMediciones()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<List<MedicionDTO>>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var mediciones = await _service.GetByUsuarioIdAsync(userId);
                var medicionesDTO = mediciones.Select(m => new MedicionDTO
                {
                    MedicionID = m.MedicionID,
                    UsuarioID = m.UsuarioID,
                    Fecha = m.Fecha,
                    Peso = m.Peso,
                    Altura = m.Altura,
                    IMC = m.IMC,
                    PorcentajeGrasa = m.PorcentajeGrasa,
                    CircunferenciaBrazo = m.CircunferenciaBrazo,
                    CircunferenciaPecho = m.CircunferenciaPecho,
                    CircunferenciaCintura = m.CircunferenciaCintura,
                    CircunferenciaMuslo = m.CircunferenciaMuslo,
                    Notas = m.Notas
                }).ToList();

                return Ok(new ApiResponse<List<MedicionDTO>>
                {
                    Success = true,
                    Data = medicionesDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<MedicionDTO>>
                {
                    Success = false,
                    Message = "Error al obtener mediciones: " + ex.Message
                });
            }
        }

        // GET: api/Medicion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MedicionDTO>>> GetMedicion(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<MedicionDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var medicion = await _service.GetByIdAsync(id);
                if (medicion == null)
                {
                    return NotFound(new ApiResponse<MedicionDTO>
                    {
                        Success = false,
                        Message = "Medición no encontrada"
                    });
                }

                // Verificar que la medición pertenezca al usuario actual
                if (medicion.UsuarioID != userId)
                {
                    return Forbid();
                }

                var medicionDTO = new MedicionDTO
                {
                    MedicionID = medicion.MedicionID,
                    UsuarioID = medicion.UsuarioID,
                    Fecha = medicion.Fecha,
                    Peso = medicion.Peso,
                    Altura = medicion.Altura,
                    IMC = medicion.IMC,
                    PorcentajeGrasa = medicion.PorcentajeGrasa,
                    CircunferenciaBrazo = medicion.CircunferenciaBrazo,
                    CircunferenciaPecho = medicion.CircunferenciaPecho,
                    CircunferenciaCintura = medicion.CircunferenciaCintura,
                    CircunferenciaMuslo = medicion.CircunferenciaMuslo,
                    Notas = medicion.Notas
                };

                return Ok(new ApiResponse<MedicionDTO>
                {
                    Success = true,
                    Data = medicionDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<MedicionDTO>
                {
                    Success = false,
                    Message = "Error al obtener medición: " + ex.Message
                });
            }
        }

        // GET: api/Medicion/Resumen
        [HttpGet("Resumen")]
        public async Task<ActionResult<ApiResponse<List<MedicionResumenDTO>>>> GetResumen()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<List<MedicionResumenDTO>>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var resumen = await _service.GetResumenByUsuarioIdAsync(userId);
                var resumenDTO = resumen.Select(r => new MedicionResumenDTO
                {
                    Anio = r.Anio,
                    Mes = r.Mes,
                    PesoPromedio = r.PesoPromedio,
                    IMCPromedio = r.IMCPromedio,
                    GrasaPromedio = r.GrasaPromedio,
                    CinturaPromedio = r.CinturaPromedio
                }).ToList();

                return Ok(new ApiResponse<List<MedicionResumenDTO>>
                {
                    Success = true,
                    Data = resumenDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<MedicionResumenDTO>>
                {
                    Success = false,
                    Message = "Error al obtener resumen de mediciones: " + ex.Message
                });
            }
        }

        // POST: api/Medicion
        [HttpPost]
        public async Task<ActionResult<ApiResponse<MedicionDTO>>> CreateMedicion([FromBody] MedicionCreateDTO medicionDTO)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<MedicionDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                // Asegurar que el usuario solo pueda crear mediciones para sí mismo
                if (medicionDTO.UsuarioID != userId)
                {
                    return BadRequest(new ApiResponse<MedicionDTO>
                    {
                        Success = false,
                        Message = "No puedes crear mediciones para otro usuario"
                    });
                }

                var medicion = new Medicion
                {
                    UsuarioID = medicionDTO.UsuarioID,
                    Fecha = medicionDTO.Fecha ?? DateTime.Now,
                    Peso = medicionDTO.Peso,
                    Altura = medicionDTO.Altura,
                    PorcentajeGrasa = medicionDTO.PorcentajeGrasa,
                    CircunferenciaBrazo = medicionDTO.CircunferenciaBrazo,
                    CircunferenciaPecho = medicionDTO.CircunferenciaPecho,
                    CircunferenciaCintura = medicionDTO.CircunferenciaCintura,
                    CircunferenciaMuslo = medicionDTO.CircunferenciaMuslo,
                    Notas = medicionDTO.Notas
                };

                var id = await _service.AddAsync(medicion);
                
                // Obtener la medición recién creada con el IMC calculado
                var nuevaMedicion = await _service.GetByIdAsync(id);
                var nuevaMedicionDTO = new MedicionDTO
                {
                    MedicionID = nuevaMedicion.MedicionID,
                    UsuarioID = nuevaMedicion.UsuarioID,
                    Fecha = nuevaMedicion.Fecha,
                    Peso = nuevaMedicion.Peso,
                    Altura = nuevaMedicion.Altura,
                    IMC = nuevaMedicion.IMC,
                    PorcentajeGrasa = nuevaMedicion.PorcentajeGrasa,
                    CircunferenciaBrazo = nuevaMedicion.CircunferenciaBrazo,
                    CircunferenciaPecho = nuevaMedicion.CircunferenciaPecho,
                    CircunferenciaCintura = nuevaMedicion.CircunferenciaCintura,
                    CircunferenciaMuslo = nuevaMedicion.CircunferenciaMuslo,
                    Notas = nuevaMedicion.Notas
                };

                return CreatedAtAction(nameof(GetMedicion), new { id }, new ApiResponse<MedicionDTO>
                {
                    Success = true,
                    Data = nuevaMedicionDTO,
                    Message = "Medición creada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<MedicionDTO>
                {
                    Success = false,
                    Message = "Error al crear medición: " + ex.Message
                });
            }
        }

        // PUT: api/Medicion/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MedicionDTO>>> UpdateMedicion(int id, [FromBody] MedicionUpdateDTO medicionDTO)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<MedicionDTO>
                    {
                        Success = false,
                        Message = "Usuario no autorizado"
                    });
                }

                var existingMedicion = await _service.GetByIdAsync(id);
                if (existingMedicion == null)
                {
                    return NotFound(new ApiResponse<MedicionDTO>
                    {
                        Success = false,
                        Message = "Medición no encontrada"
                    });
                }

                // Verificar que la medición pertenezca al usuario actual
                if (existingMedicion.UsuarioID != userId)
                {
                    return Forbid();
                }

                // Actualizar solo los campos proporcionados
                if (medicionDTO.Fecha.HasValue)
                    existingMedicion.Fecha = medicionDTO.Fecha.Value;
                
                if (medicionDTO.Peso.HasValue)
                    existingMedicion.Peso = medicionDTO.Peso;
                
                if (medicionDTO.Altura.HasValue)
                    existingMedicion.Altura = medicionDTO.Altura;
                
                if (medicionDTO.PorcentajeGrasa.HasValue)
                    existingMedicion.PorcentajeGrasa = medicionDTO.PorcentajeGrasa;
                
                if (medicionDTO.CircunferenciaBrazo.HasValue)
                    existingMedicion.CircunferenciaBrazo = medicionDTO.CircunferenciaBrazo;
                
                if (medicionDTO.CircunferenciaPecho.HasValue)
                    existingMedicion.CircunferenciaPecho = medicionDTO.CircunferenciaPecho;
                
                if (medicionDTO.CircunferenciaCintura.HasValue)
                    existingMedicion.CircunferenciaCintura = medicionDTO.CircunferenciaCintura;
                
                if (medicionDTO.CircunferenciaMuslo.HasValue)
                    existingMedicion.CircunferenciaMuslo = medicionDTO.CircunferenciaMuslo;
                
                if (medicionDTO.Notas != null)
                    existingMedicion.Notas = medicionDTO.Notas;

                await _service.UpdateAsync(existingMedicion);

                // Obtener la medición actualizada con el IMC recalculado
                var medicionActualizada = await _service.GetByIdAsync(id);
                var medicionActualizadaDTO = new MedicionDTO
                {
                    MedicionID = medicionActualizada.MedicionID,
                    UsuarioID = medicionActualizada.UsuarioID,
                    Fecha = medicionActualizada.Fecha,
                    Peso = medicionActualizada.Peso,
                    Altura = medicionActualizada.Altura,
                    IMC = medicionActualizada.IMC,
                    PorcentajeGrasa = medicionActualizada.PorcentajeGrasa,
                    CircunferenciaBrazo = medicionActualizada.CircunferenciaBrazo,
                    CircunferenciaPecho = medicionActualizada.CircunferenciaPecho,
                    CircunferenciaCintura = medicionActualizada.CircunferenciaCintura,
                    CircunferenciaMuslo = medicionActualizada.CircunferenciaMuslo,
                    Notas = medicionActualizada.Notas
                };

                return Ok(new ApiResponse<MedicionDTO>
                {
                    Success = true,
                    Data = medicionActualizadaDTO,
                    Message = "Medición actualizada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<MedicionDTO>
                {
                    Success = false,
                    Message = "Error al actualizar medición: " + ex.Message
                });
            }
        }

        // DELETE: api/Medicion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMedicion(int id)
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

                var medicion = await _service.GetByIdAsync(id);
                if (medicion == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Medición no encontrada"
                    });
                }

                // Verificar que la medición pertenezca al usuario actual
                if (medicion.UsuarioID != userId)
                {
                    return Forbid();
                }

                await _service.DeleteAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Medición eliminada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error al eliminar medición: " + ex.Message
                });
            }
        }
    }
}