using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task UpdateResetTokenAsync(int userId, string? token, DateTime? expires);
        Task<Usuario?> GetByResetTokenAsync(string token);
        Task<int> AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
    }
}
