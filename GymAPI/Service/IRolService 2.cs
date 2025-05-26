using GymAPI.Models;

namespace GymAPI.Services
{
    public interface IRolService
    {
        Task<List<Rol>> GetAllRolesAsync();
        Task<Rol?> GetRolByIdAsync(int id);
        Task<Rol?> GetUserCurrentRolAsync(int userId);
        Task<(Rol? CurrentRol, int DiasEntrenados, int DiasParaSiguiente, double Progreso)> GetUserRolInfoAsync(int userId);
    }
}