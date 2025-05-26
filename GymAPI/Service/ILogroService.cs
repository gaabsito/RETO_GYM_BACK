// GymAPI/Service/ILogroService.cs
using GymAPI.Models;
using GymAPI.DTOs;

namespace GymAPI.Services
{
    public interface ILogroService
    {
        Task<List<Logro>> GetAllLogrosAsync();
        Task<Logro?> GetLogroByIdAsync(int id);
        Task<List<UsuarioLogroDTO>> GetUsuarioLogrosAsync(int usuarioId);
        Task<List<LogroDTO>> GetLogrosDisponiblesAsync(int usuarioId);
        Task<UsuarioLogroDTO?> GetUsuarioLogroByIdAsync(int usuarioId, int logroId);
        Task ActualizarProgresoLogroAsync(int usuarioId, int logroId, int progreso);
        Task VerificarLogrosAsync(int usuarioId);
        Task<List<UsuarioLogroDTO>> GetLogrosRecientesAsync(int usuarioId, int cantidad = 5);
    }
}