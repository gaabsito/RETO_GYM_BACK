// ===========================================
// 2. GymAPI/Service/IUsuarioService.cs
// ===========================================
using GymAPI.Models;

namespace GymAPI.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<int> AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        Task UpdateResetTokenAsync(int userId, string? token, DateTime? expires);
        Task<Usuario?> GetByResetTokenAsync(string token);
        
        // Métodos adicionales para estadísticas
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetAdminUsersCountAsync();
        Task<int> GetUsersRegisteredTodayAsync();
        Task<int> GetUsersRegisteredThisMonthAsync();
    }
}
