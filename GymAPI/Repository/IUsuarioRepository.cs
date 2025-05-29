using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface IUsuarioRepository
    {
        // Métodos básicos CRUD
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<int> AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        
        // Métodos de recuperación de contraseña
        Task UpdateResetTokenAsync(int userId, string? token, DateTime? expires);
        Task<Usuario?> GetByResetTokenAsync(string token);
        
        // Métodos adicionales para estadísticas del admin panel
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetAdminUsersCountAsync();
        Task<int> GetUsersRegisteredTodayAsync();
        Task<int> GetUsersRegisteredThisMonthAsync();
        
        // Métodos de validación
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    }
}