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
    }

}
