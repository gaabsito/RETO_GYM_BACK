using GymAPI.Models;

namespace GymAPI.Services
{
    public interface IEjercicioService
    {
        Task<List<Ejercicio>> GetAllAsync();
        Task<Ejercicio?> GetByIdAsync(int id);
        Task AddAsync(Ejercicio ejercicio);
        Task UpdateAsync(Ejercicio ejercicio);
        Task DeleteAsync(int id);
    }
}
