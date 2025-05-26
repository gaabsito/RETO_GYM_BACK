using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface IMedicionRepository
    {
        Task<List<Medicion>> GetAllAsync();
        Task<Medicion?> GetByIdAsync(int id);
        Task<List<Medicion>> GetByUsuarioIdAsync(int usuarioId);
        Task<List<MedicionResumen>> GetResumenByUsuarioIdAsync(int usuarioId);
        Task<int> AddAsync(Medicion medicion);
        Task UpdateAsync(Medicion medicion);
        Task DeleteAsync(int id);
    }
}