using GymAPI.Models;

namespace GymAPI.Services
{
    public interface IMedicionService
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