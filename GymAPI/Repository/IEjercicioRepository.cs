using GymAPI.Models;

namespace GymAPI.Repositories
{
    public interface IEjercicioRepository
    {
        Task<List<Ejercicio>> GetAllAsync();
        Task<Ejercicio?> GetByIdAsync(int id);
        Task<List<Ejercicio>> GetByGrupoMuscularAsync(string grupoMuscular);
        Task<List<Ejercicio>> GetByEquipamientoAsync(bool requiereEquipamiento);
        Task<List<string>> GetGruposMusculares();
        Task<List<Ejercicio>> SearchAsync(string searchTerm);
        Task<int> AddAsync(Ejercicio ejercicio);
        Task UpdateAsync(Ejercicio ejercicio);
        Task DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByGrupoMuscularAsync(string grupoMuscular);
        Task<int> GetCountWithEquipmentAsync();
        Task<int> GetCountWithoutEquipmentAsync();
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null);
        Task<List<(int EjercicioID, string Nombre, int Veces)>> GetMostUsedAsync(int limit = 10);
    }
}