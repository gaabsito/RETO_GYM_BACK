// GymAPI/Repository/IUsuarioLogroRepository.cs
using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface IUsuarioLogroRepository
    {
        Task<List<UsuarioLogro>> GetByUsuarioIdAsync(int usuarioId);
        Task<UsuarioLogro?> GetByUsuarioAndLogroIdAsync(int usuarioId, int logroId);
        Task<int> AddAsync(UsuarioLogro usuarioLogro);
        Task UpdateAsync(UsuarioLogro usuarioLogro);
        Task UpdateProgresoAsync(int usuarioId, int logroId, int progreso, bool desbloqueado);
        Task DeleteAsync(int id);
    }
}