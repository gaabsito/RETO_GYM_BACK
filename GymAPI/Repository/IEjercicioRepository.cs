using GymAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Repositories
{
    public interface IEjercicioRepository
    {
        Task<List<Ejercicio>> GetAllAsync();
        Task<Ejercicio?> GetByIdAsync(int id);
        Task AddAsync(Ejercicio ejercicio);
        Task UpdateAsync(Ejercicio ejercicio);
        Task DeleteAsync(int id);
    }
}
