using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymAPI.Models;
using GymAPI.Services;
using GymAPI.DTOs;
using GymAPI.Settings;
using Microsoft.Extensions.Options;

namespace GymAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IUsuarioService usuarioService, IOptions<JwtSettings> jwtSettings)
        {
            _usuarioService = usuarioService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDTO)
        {
            try
            {
                // Verificar si el usuario ya existe
                var existingUser = await _usuarioService.GetByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Ya existe un usuario con este email" });
                }

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = registerDTO.Nombre,
                    Apellido = registerDTO.Apellido,
                    Email = registerDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                    EsAdmin = false // Los registros normales no son admin
                };

                await _usuarioService.AddAsync(usuario);

                // Generar token JWT
                var token = GenerateJwtToken(usuario);

                var usuarioDTO = new UsuarioDTO
                {
                    UsuarioID = usuario.UsuarioID,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    EstaActivo = usuario.EstaActivo,
                    FotoPerfilURL = usuario.FotoPerfilURL,
                    EsAdmin = usuario.EsAdmin
                };

                return Ok(new AuthResponseDTO
                {
                    User = usuarioDTO,
                    Token = token,
                    EsAdministrador = usuario.EsAdmin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
        {
            try
            {
                // Buscar usuario por email
                var usuario = await _usuarioService.GetByEmailAsync(loginDTO.Email);
                if (usuario == null)
                {
                    return BadRequest(new { message = "Credenciales inválidas" });
                }

                // Verificar contraseña
                if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, usuario.Password))
                {
                    return BadRequest(new { message = "Credenciales inválidas" });
                }

                // Verificar si el usuario está activo
                if (!usuario.EstaActivo)
                {
                    return BadRequest(new { message = "Cuenta desactivada" });
                }

                // Generar token JWT
                var token = GenerateJwtToken(usuario);

                var usuarioDTO = new UsuarioDTO
                {
                    UsuarioID = usuario.UsuarioID,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    EstaActivo = usuario.EstaActivo,
                    FotoPerfilURL = usuario.FotoPerfilURL,
                    EsAdmin = usuario.EsAdmin
                };

                return Ok(new AuthResponseDTO
                {
                    User = usuarioDTO,
                    Token = token,
                    EsAdministrador = usuario.EsAdmin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}")
            };

            // IMPORTANTE: Agregar claim de administrador
            if (usuario.EsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                claims.Add(new Claim("IsAdmin", "true"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim("IsAdmin", "false"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("verify")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> VerifyToken()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ApiResponse<UsuarioDTO>
                    {
                        Success = false,
                        Message = "Token inválido"
                    });
                }

                var usuario = await _usuarioService.GetByIdAsync(userId);
                if (usuario == null || !usuario.EstaActivo)
                {
                    return Unauthorized(new ApiResponse<UsuarioDTO>
                    {
                        Success = false,
                        Message = "Usuario no encontrado o inactivo"
                    });
                }

                var usuarioDTO = new UsuarioDTO
                {
                    UsuarioID = usuario.UsuarioID,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    EstaActivo = usuario.EstaActivo,
                    FotoPerfilURL = usuario.FotoPerfilURL,
                    EsAdmin = usuario.EsAdmin
                };

                return Ok(new ApiResponse<UsuarioDTO>
                {
                    Success = true,
                    Data = usuarioDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UsuarioDTO>
                {
                    Success = false,
                    Message = "Error interno del servidor"
                });
            }
        }
    }
}