using GymAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymAPI.Repositories
{
    public interface IEntrenamientoRepository
    {
        Task<List<Entrenamiento>> GetAllAsync();
        Task<Entrenamiento?> GetByIdAsync(int id);
        Task AddAsync(Entrenamiento entrenamiento);
        Task UpdateAsync(Entrenamiento entrenamiento);
        Task DeleteAsync(int id);
    }
}
