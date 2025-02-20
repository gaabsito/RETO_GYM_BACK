using Microsoft.AspNetCore.Mvc;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using GymAPI.Settings;
using Microsoft.AspNetCore.Authorization;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _userService;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;

        public AuthController(IUsuarioService userService, IOptions<JwtSettings> jwtSettings, IEmailService emailService)
        {
            _userService = userService;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDto)
        {
            try
            {
                // Verificar si el email ya existe
                var existingUsers = await _userService.GetAllAsync();
                if (existingUsers.Any(u => u.Email.ToLower() == registerDto.Email.ToLower()))
                {
                    return BadRequest(new { message = "El email ya está registrado" });
                }

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = registerDto.Nombre.Trim(),
                    Apellido = registerDto.Apellido.Trim(),
                    Email = registerDto.Email.ToLower().Trim(),
                    Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    FechaRegistro = DateTime.UtcNow,
                    EstaActivo = true
                };

                // Añadir usuario y obtener el ID
                var userId = await _userService.AddAsync(usuario);
                usuario.UsuarioID = userId;

                // Generar token
                var token = GenerateJwtToken(usuario);

                // Crear DTO de respuesta
                var userDto = new UsuarioDTO
                {
                    UsuarioID = usuario.UsuarioID,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    EstaActivo = usuario.EstaActivo
                };

                return Ok(new AuthResponseDTO
                {
                    User = userDto,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al registrar usuario" });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDto)
        {
            try
            {
                // Buscar usuario por email directamente
                var user = await _userService.GetByEmailAsync(loginDto.Email.ToLower());

                // Verificar si el usuario existe
                if (user == null)
                {
                    return BadRequest(new { message = "Email o contraseña incorrectos" });
                }

                // Verificar si el usuario está activo
                if (!user.EstaActivo)
                {
                    return BadRequest(new { message = "La cuenta está desactivada" });
                }

                // Verificar la contraseña
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    return BadRequest(new { message = "Email o contraseña incorrectos" });
                }

                // Generar token
                var token = GenerateJwtToken(user);

                // Crear DTO de respuesta
                var userDto = new UsuarioDTO
                {
                    UsuarioID = user.UsuarioID,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Email = user.Email,
                    FechaRegistro = user.FechaRegistro,
                    EstaActivo = user.EstaActivo
                };

                return Ok(new AuthResponseDTO
                {
                    User = userDto,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al iniciar sesión" });
            }
        }

        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetDTO resetDto)
        {
            try
            {
                var user = await _userService.GetByEmailAsync(resetDto.Email.ToLower());
                if (user == null)
                {
                    return Ok(new { message = "Si el email existe, recibirás instrucciones para recuperar tu contraseña" });
                }

                var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                    .Replace("/", "_")
                    .Replace("+", "-");
                var expires = DateTime.UtcNow.AddHours(1);

                await _userService.UpdateResetTokenAsync(user.UsuarioID, token, expires);

                // Enviar email
                await _emailService.SendPasswordResetEmailAsync(
                    user.Email,
                    $"{user.Nombre} {user.Apellido}",
                    token
                );

                return Ok(new { message = "Se han enviado las instrucciones a tu correo electrónico" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al procesar la solicitud" });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetDto)
        {
            try
            {
                var user = await _userService.GetByResetTokenAsync(resetDto.Token);
                if (user == null || user.ResetPasswordExpires < DateTime.UtcNow)
                {
                    return BadRequest(new { message = "Token inválido o expirado" });
                }

                // Actualizar contraseña
                user.Password = BCrypt.Net.BCrypt.HashPassword(resetDto.Password);
                user.ResetPasswordToken = null;
                user.ResetPasswordExpires = null;

                await _userService.UpdateAsync(user);

                return Ok(new { message = "Contraseña actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al restablecer la contraseña" });
            }
        }

        [Authorize]
        [HttpGet("verify")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> VerifyToken()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    return Unauthorized(new ApiResponse<UsuarioDTO>
                    {
                        Success = false,
                        Message = "Token inválido"
                    });
                }

                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<UsuarioDTO>
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    });
                }

                if (!user.EstaActivo)
                {
                    return Unauthorized(new ApiResponse<UsuarioDTO>
                    {
                        Success = false,
                        Message = "La cuenta está desactivada"
                    });
                }

                var userDto = new UsuarioDTO
                {
                    UsuarioID = user.UsuarioID,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Email = user.Email,
                    FechaRegistro = user.FechaRegistro,
                    EstaActivo = user.EstaActivo
                };

                return Ok(new ApiResponse<UsuarioDTO>
                {
                    Success = true,
                    Data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UsuarioDTO>
                {
                    Success = false,
                    Message = "Error interno del servidor al verificar token"
                });
            }
        }

        private string GenerateJwtToken(Usuario user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UsuarioID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Nombre),
                new Claim(ClaimTypes.Surname, user.Apellido)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}