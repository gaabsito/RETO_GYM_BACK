using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;
using GymAPI.Repositories;
using GymAPI.Utilities;
using System.Text.Json;

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
        private readonly IEntrenamientoService _entrenamientoService;
        private readonly IEntrenamientoEjercicioService _entrenamientoEjercicioService;
        private readonly IImageService _imageService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IUsuarioService usuarioService,
            IEjercicioRepository ejercicioRepository,
            IEntrenamientoRepository entrenamientoRepository,
            IEntrenamientoService entrenamientoService,
            IEntrenamientoEjercicioService entrenamientoEjercicioService,
            IImageService imageService,
            ILogger<AdminController> logger)
        {
            _usuarioService = usuarioService;
            _ejercicioRepository = ejercicioRepository;
            _entrenamientoRepository = entrenamientoRepository;
            _entrenamientoService = entrenamientoService;
            _entrenamientoEjercicioService = entrenamientoEjercicioService;
            _imageService = imageService;
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
        public async Task<ActionResult<ApiResponse<List<UsuarioDTO>>>> GetAllUsers()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                var usuarios = await _usuarioService.GetAllAsync();
                var usuariosDTO = usuarios.Select(u => new UsuarioDTO
                {
                    UsuarioID = u.UsuarioID,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro,
                    EstaActivo = u.EstaActivo,
                    FotoPerfilURL = u.FotoPerfilURL,
                    EsAdmin = u.EsAdmin
                }).ToList();

                return Ok(new ApiResponse<List<UsuarioDTO>>
                {
                    Success = true,
                    Data = usuariosDTO
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, new ApiResponse<List<UsuarioDTO>>
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpPost("usuarios")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> CreateUser([FromBody] UsuarioCreateDTO userDTO)
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
                    EsAdmin = false, // Por defecto no admin
                    EstaActivo = true
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
        public async Task<ActionResult<ApiResponse<bool>>> UpdateUser(int id, [FromBody] JsonElement requestBody)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                // 🔍 DEBUGGING: Log completo del request
                var requestBodyString = requestBody.GetRawText();
                _logger.LogInformation("🔍 REQUEST BODY RAW: {RequestBody}", requestBodyString);

                // 🔍 PARSING MANUAL de los campos
                string? nombre = null;
                string? apellido = null; 
                string? email = null;
                string? password = null;
                int? edad = null;
                float? altura = null;
                float? peso = null;
                bool? esAdmin = null;
                bool? estaActivo = null;

                if (requestBody.TryGetProperty("nombre", out var nombreProperty))
                    nombre = nombreProperty.GetString();
                
                if (requestBody.TryGetProperty("apellido", out var apellidoProperty))
                    apellido = apellidoProperty.GetString();
                
                if (requestBody.TryGetProperty("email", out var emailProperty))
                    email = emailProperty.GetString();
                
                if (requestBody.TryGetProperty("password", out var passwordProperty))
                    password = passwordProperty.GetString();
                
                if (requestBody.TryGetProperty("edad", out var edadProperty) && edadProperty.ValueKind == JsonValueKind.Number)
                    edad = edadProperty.GetInt32();
                
                if (requestBody.TryGetProperty("altura", out var alturaProperty) && alturaProperty.ValueKind == JsonValueKind.Number)
                    altura = (float)alturaProperty.GetDouble();
                
                if (requestBody.TryGetProperty("peso", out var pesoProperty) && pesoProperty.ValueKind == JsonValueKind.Number)
                    peso = (float)pesoProperty.GetDouble();
                
                // 🚨 CAMPOS CRÍTICOS
                if (requestBody.TryGetProperty("esAdmin", out var esAdminProperty))
                {
                    if (esAdminProperty.ValueKind == JsonValueKind.True)
                        esAdmin = true;
                    else if (esAdminProperty.ValueKind == JsonValueKind.False)
                        esAdmin = false;
                }
                
                if (requestBody.TryGetProperty("estaActivo", out var estaActivoProperty))
                {
                    if (estaActivoProperty.ValueKind == JsonValueKind.True)
                        estaActivo = true;
                    else if (estaActivoProperty.ValueKind == JsonValueKind.False)
                        estaActivo = false;
                }

                _logger.LogInformation("🔍 DATOS PARSEADOS: Nombre={Nombre}, Apellido={Apellido}, Email={Email}, EsAdmin={EsAdmin}, EstaActivo={EstaActivo}", 
                    nombre, apellido, email, esAdmin, estaActivo);

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

                // Guardar estado anterior para logging
                var estadoAnterior = new
                {
                    EsAdmin = usuario.EsAdmin,
                    EstaActivo = usuario.EstaActivo,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email
                };

                _logger.LogInformation("🔍 ESTADO ANTERIOR: EsAdmin={EsAdmin}, EstaActivo={EstaActivo}", 
                    estadoAnterior.EsAdmin, estadoAnterior.EstaActivo);

                // Actualizar campos básicos
                if (!string.IsNullOrEmpty(nombre))
                {
                    _logger.LogInformation("🔄 Actualizando Nombre: {Anterior} -> {Nuevo}", usuario.Nombre, nombre);
                    usuario.Nombre = nombre;
                }
                
                if (!string.IsNullOrEmpty(apellido))
                {
                    _logger.LogInformation("🔄 Actualizando Apellido: {Anterior} -> {Nuevo}", usuario.Apellido, apellido);
                    usuario.Apellido = apellido;
                }
                
                if (!string.IsNullOrEmpty(email))
                {
                    _logger.LogInformation("🔄 Actualizando Email: {Anterior} -> {Nuevo}", usuario.Email, email);
                    usuario.Email = email;
                }

                // Actualizar contraseña si se proporciona
                if (!string.IsNullOrEmpty(password))
                {
                    _logger.LogInformation("🔐 Actualizando contraseña para usuario {UsuarioID}", id);
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(password);
                }

                // 🚨 CAMPOS CRÍTICOS DE ADMINISTRADOR
                if (esAdmin.HasValue)
                {
                    _logger.LogInformation("🔒 *** ACTUALIZANDO EsAdmin *** de {ValorAnterior} a {ValorNuevo} para usuario {UsuarioID}", 
                        estadoAnterior.EsAdmin, esAdmin.Value, id);
                    usuario.EsAdmin = esAdmin.Value;
                }
                else
                {
                    _logger.LogWarning("⚠️ EsAdmin NO se recibió en el DTO (es null)");
                }

                if (estaActivo.HasValue)
                {
                    _logger.LogInformation("🔄 *** ACTUALIZANDO EstaActivo *** de {ValorAnterior} a {ValorNuevo} para usuario {UsuarioID}", 
                        estadoAnterior.EstaActivo, estaActivo.Value, id);
                    usuario.EstaActivo = estaActivo.Value;
                }
                else
                {
                    _logger.LogWarning("⚠️ EstaActivo NO se recibió en el DTO (es null)");
                }

                // Campos opcionales de perfil
                if (edad.HasValue)
                    usuario.Edad = edad.Value;
                if (altura.HasValue)
                    usuario.Altura = altura.Value;
                if (peso.HasValue)
                    usuario.Peso = peso.Value;

                _logger.LogInformation("🔍 ANTES DE GUARDAR: EsAdmin={EsAdmin}, EstaActivo={EstaActivo}", 
                    usuario.EsAdmin, usuario.EstaActivo);

                // 💾 GUARDAR EN BASE DE DATOS
                await _usuarioService.UpdateAsync(usuario);

                _logger.LogInformation("💾 GUARDADO EN BASE DE DATOS COMPLETADO");

                // 🔍 VERIFICAR QUE SE GUARDÓ CORRECTAMENTE
                var usuarioActualizado = await _usuarioService.GetByIdAsync(id);
                _logger.LogInformation("✅ VERIFICACIÓN POST-ACTUALIZACIÓN: Usuario {UsuarioID} - EsAdmin: {EsAdmin}, EstaActivo: {EstaActivo}", 
                    id, usuarioActualizado?.EsAdmin, usuarioActualizado?.EstaActivo);

                if (usuarioActualizado?.EsAdmin != esAdmin)
                {
                    _logger.LogError("🚨 ERROR: EsAdmin NO se actualizó correctamente. Esperado: {Esperado}, Actual: {Actual}", 
                        esAdmin, usuarioActualizado?.EsAdmin);
                }

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
        public async Task<ActionResult<ApiResponse<EjercicioDTO>>> CreateExercise([FromBody] EjercicioCreateDTO ejercicioDTO)
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
        public async Task<ActionResult<ApiResponse<bool>>> UpdateExercise(int id, [FromBody] EjercicioUpdateDTO ejercicioDTO)
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

        [HttpPost("entrenamientos")]
        public async Task<ActionResult<ApiResponse<EntrenamientoDTO>>> CreateWorkout()
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                _logger.LogInformation("🔥 Creando entrenamiento desde admin...");

                // Obtener el ID del usuario actual
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int autorId))
                {
                    return BadRequest(new ApiResponse<EntrenamientoDTO>
                    {
                        Success = false,
                        Message = "Usuario no válido"
                    });
                }

                // Leer los datos del formulario
                string titulo = Request.Form["Titulo"].FirstOrDefault() ?? "";
                string descripcion = Request.Form["Descripcion"].FirstOrDefault() ?? "";
                string dificultad = Request.Form["Dificultad"].FirstOrDefault() ?? "";
                bool publico = bool.Parse(Request.Form["Publico"].FirstOrDefault() ?? "true");
                
                if (!int.TryParse(Request.Form["DuracionMinutos"].FirstOrDefault(), out int duracionMinutos))
                {
                    return BadRequest(new ApiResponse<EntrenamientoDTO>
                    {
                        Success = false,
                        Message = "Duración inválida"
                    });
                }

                // Validaciones básicas
                if (string.IsNullOrEmpty(titulo))
                {
                    return BadRequest(new ApiResponse<EntrenamientoDTO>
                    {
                        Success = false,
                        Message = "El título es requerido"
                    });
                }

                if (!new[] { "Fácil", "Media", "Difícil" }.Contains(dificultad))
                {
                    return BadRequest(new ApiResponse<EntrenamientoDTO>
                    {
                        Success = false,
                        Message = "Dificultad inválida"
                    });
                }

                // Procesar imagen si se proporciona
                string? imagenURL = null;
                var imagen = Request.Form.Files["Imagen"];
                if (imagen != null && imagen.Length > 0)
                {
                    // Validar la imagen
                    if (!FileValidationHelper.ValidateImageFile(imagen, out string errorMessage))
                    {
                        return BadRequest(new ApiResponse<EntrenamientoDTO>
                        {
                            Success = false,
                            Message = errorMessage
                        });
                    }

                    // Subir imagen a Cloudinary
                    imagenURL = await _imageService.UploadImageAsync(imagen);
                    _logger.LogInformation("🖼️ Imagen subida: {ImagenURL}", imagenURL);
                }

                // Crear el entrenamiento
                var entrenamiento = new Entrenamiento
                {
                    Titulo = titulo,
                    Descripcion = descripcion,
                    DuracionMinutos = duracionMinutos,
                    Dificultad = dificultad,
                    ImagenURL = imagenURL,
                    Publico = publico,
                    AutorID = autorId,
                    FechaCreacion = DateTime.Now
                };

                await _entrenamientoRepository.AddAsync(entrenamiento);
                _logger.LogInformation("✅ Entrenamiento creado con ID: {EntrenamientoID}", entrenamiento.EntrenamientoID);

                // Procesar ejercicios si se proporcionan
                await ProcessWorkoutExercises(entrenamiento.EntrenamientoID);

                var entrenamientoResponse = new EntrenamientoDTO
                {
                    EntrenamientoID = entrenamiento.EntrenamientoID,
                    Titulo = entrenamiento.Titulo,
                    Descripcion = entrenamiento.Descripcion,
                    DuracionMinutos = entrenamiento.DuracionMinutos,
                    Dificultad = entrenamiento.Dificultad,
                    ImagenURL = entrenamiento.ImagenURL,
                    FechaCreacion = entrenamiento.FechaCreacion,
                    Publico = entrenamiento.Publico,
                    AutorID = entrenamiento.AutorID
                };

                return Ok(new ApiResponse<EntrenamientoDTO>
                {
                    Success = true,
                    Data = entrenamientoResponse,
                    Message = "Entrenamiento creado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al crear entrenamiento");
                return StatusCode(500, new ApiResponse<EntrenamientoDTO>
                {
                    Success = false,
                    Message = $"Error al crear entrenamiento: {ex.Message}"
                });
            }
        }

        [HttpPut("entrenamientos/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateWorkout(int id)
        {
            if (!await IsCurrentUserAdmin())
                return Forbid();

            try
            {
                _logger.LogInformation("🔥 Actualizando entrenamiento ID: {EntrenamientoID}", id);

                var entrenamiento = await _entrenamientoRepository.GetByIdAsync(id);
                if (entrenamiento == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Entrenamiento no encontrado"
                    });
                }

                // Leer los datos del formulario
                string titulo = Request.Form["Titulo"].FirstOrDefault() ?? entrenamiento.Titulo;
                string descripcion = Request.Form["Descripcion"].FirstOrDefault() ?? entrenamiento.Descripcion;
                string dificultad = Request.Form["Dificultad"].FirstOrDefault() ?? entrenamiento.Dificultad;
                bool publico = bool.Parse(Request.Form["Publico"].FirstOrDefault() ?? entrenamiento.Publico.ToString());
                
                if (int.TryParse(Request.Form["DuracionMinutos"].FirstOrDefault(), out int duracionMinutos))
                {
                    entrenamiento.DuracionMinutos = duracionMinutos;
                }

                // Actualizar campos básicos
                entrenamiento.Titulo = titulo;
                entrenamiento.Descripcion = descripcion;
                entrenamiento.Dificultad = dificultad;
                entrenamiento.Publico = publico;

                // Procesar nueva imagen si se proporciona
                var imagen = Request.Form.Files["Imagen"];
                if (imagen != null && imagen.Length > 0)
                {
                    // Validar la imagen
                    if (!FileValidationHelper.ValidateImageFile(imagen, out string errorMessage))
                    {
                        return BadRequest(new ApiResponse<bool>
                        {
                            Success = false,
                            Message = errorMessage
                        });
                    }

                    // Eliminar imagen anterior si existe
                    if (!string.IsNullOrEmpty(entrenamiento.ImagenURL) && entrenamiento.ImagenURL.Contains("cloudinary.com"))
                    {
                        try
                        {
                            var uri = new Uri(entrenamiento.ImagenURL);
                            var pathSegments = uri.PathAndQuery.Split('/');
                            string fileName = pathSegments.Last().Split('.')[0];
                            string publicId = $"entrenamientos/{fileName}";
                            await _imageService.DeleteImageAsync(publicId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("⚠️ Error al eliminar imagen anterior: {Message}", ex.Message);
                        }
                    }

                    // Subir nueva imagen
                    entrenamiento.ImagenURL = await _imageService.UploadImageAsync(imagen);
                    _logger.LogInformation("🖼️ Nueva imagen subida: {ImagenURL}", entrenamiento.ImagenURL);
                }

                // Actualizar el entrenamiento
                await _entrenamientoRepository.UpdateAsync(entrenamiento);

                // Actualizar ejercicios del entrenamiento
                await ProcessWorkoutExercises(id);

                _logger.LogInformation("✅ Entrenamiento actualizado exitosamente: {EntrenamientoID}", id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Entrenamiento actualizado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al actualizar entrenamiento {EntrenamientoID}", id);
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al actualizar entrenamiento: {ex.Message}"
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
                _logger.LogInformation("🔥 INICIANDO eliminación de entrenamiento ID: {EntrenamientoID}", id);

                // Verificar que el entrenamiento existe
                var entrenamiento = await _entrenamientoRepository.GetByIdAsync(id);
                if (entrenamiento == null)
                {
                    _logger.LogWarning("❌ Entrenamiento {EntrenamientoID} no encontrado", id);
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Entrenamiento no encontrado"
                    });
                }

                // Eliminar imagen asociada si existe
                if (!string.IsNullOrEmpty(entrenamiento.ImagenURL))
                {
                    try
                    {
                        // Extraer el publicId de la URL de Cloudinary si es necesario
                        // Solo intentar eliminar si parece ser una URL de Cloudinary
                        if (entrenamiento.ImagenURL.Contains("cloudinary.com"))
                        {
                            var uri = new Uri(entrenamiento.ImagenURL);
                            var pathSegments = uri.PathAndQuery.Split('/');
                            if (pathSegments.Length > 0)
                            {
                                string fileName = pathSegments.Last().Split('.')[0];
                                string publicId = $"entrenamientos/{fileName}";
                                await _imageService.DeleteImageAsync(publicId);
                                _logger.LogInformation("🖼️ Imagen eliminada: {PublicId}", publicId);
                            }
                        }
                    }
                    catch (Exception imgEx)
                    {
                        // Log el error pero continúa con la eliminación del entrenamiento
                        _logger.LogWarning("⚠️ Error al eliminar imagen del entrenamiento {EntrenamientoID}: {Message}", 
                            id, imgEx.Message);
                    }
                }

                // Eliminar el entrenamiento (esto también eliminará las relaciones en EntrenamientoEjercicios por CASCADE)
                await _entrenamientoRepository.DeleteAsync(id);

                _logger.LogInformation("✅ Entrenamiento eliminado exitosamente: {EntrenamientoID}", id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Entrenamiento eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 ERROR CRITICO al eliminar entrenamiento {EntrenamientoID}: {Message}", id, ex.Message);
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar entrenamiento: {ex.Message}"
                });
            }
        }

        // Método helper para procesar ejercicios del entrenamiento
        private async Task ProcessWorkoutExercises(int entrenamientoId)
        {
            try
            {
                // Eliminar ejercicios existentes
                var ejerciciosExistentes = await _entrenamientoEjercicioService.GetByEntrenamientoIdAsync(entrenamientoId);
                foreach (var ejercicioExistente in ejerciciosExistentes)
                {
                    await _entrenamientoEjercicioService.DeleteAsync(ejercicioExistente.EntrenamientoID, ejercicioExistente.EjercicioID);
                }

                // Procesar nuevos ejercicios
                var ejerciciosCount = 0;
                while (Request.Form.ContainsKey($"Ejercicios[{ejerciciosCount}].EjercicioID"))
                {
                    if (int.TryParse(Request.Form[$"Ejercicios[{ejerciciosCount}].EjercicioID"], out int ejercicioId) &&
                        int.TryParse(Request.Form[$"Ejercicios[{ejerciciosCount}].Series"], out int series) &&
                        int.TryParse(Request.Form[$"Ejercicios[{ejerciciosCount}].Repeticiones"], out int repeticiones) &&
                        int.TryParse(Request.Form[$"Ejercicios[{ejerciciosCount}].DescansoSegundos"], out int descanso))
                    {
                        string notas = Request.Form[$"Ejercicios[{ejerciciosCount}].Notas"].FirstOrDefault() ?? "";

                        var entrenamientoEjercicio = new EntrenamientoEjercicio
                        {
                            EntrenamientoID = entrenamientoId,
                            EjercicioID = ejercicioId,
                            Series = series,
                            Repeticiones = repeticiones,
                            DescansoSegundos = descanso,
                            Notas = string.IsNullOrEmpty(notas) ? null : notas
                        };

                        await _entrenamientoEjercicioService.AddAsync(entrenamientoEjercicio);
                        _logger.LogInformation("➕ Ejercicio agregado: {EjercicioID} al entrenamiento {EntrenamientoID}", 
                            ejercicioId, entrenamientoId);
                    }

                    ejerciciosCount++;
                }

                _logger.LogInformation("✅ Procesados {Count} ejercicios para entrenamiento {EntrenamientoID}", 
                    ejerciciosCount, entrenamientoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al procesar ejercicios del entrenamiento {EntrenamientoID}", entrenamientoId);
                throw;
            }
        }

        // ============ DASHBOARD ============

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